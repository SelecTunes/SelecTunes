#!/usr/bin/env ruby
# frozen_string_literal: false

require 'http'
require 'yaml'
require 'zip'
require 'open3'
require 'erb'
require 'json'

def call_systemd(action, service)
  begin
    Open3.capture2('systemctl', action, service) do |o, s|
      unless s.exitstatus
        puts "Error stopping application. System returned !#{s} {#{o}}"
        exit 1
      end
    end
  rescue Errno::ENOENT
    puts "systemctl command not found. Cannot #{action} application on the platform."
  end
end

@tag =
  if (ARGV.length >= 2) && (ARGV.include? '-t')
    ARGV[(ARGV.index '-t') + 1] if ARGV.include? '-t'
  else
    'latest'
  end

@platform =
  if (ARGV.length >= 2) && (ARGV.include? '-p')
    ARGV[(ARGV.index '-p') + 1] if ARGV.include? '-p'
  else
    'linux'
  end

puts "Using tag !#{@tag}"
puts "Using platform !#{@platform}"

@settings =
  if File.exist? '/etc/st-update/settings.yml'
    YAML.safe_load (File.read '/etc/st-update/settings.yml'),
                   symbolize_names: true
  else
    {
      :gitlab_uri => 'https://git.linux.iastate.edu',
      :gitlab_token => '',
      :gitlab_project_id => '4397',
      :gitlab_artifacts_url => '/api/v4/projects/:id/jobs/:job/artifacts',
      :gitlab_pipeline_url => '/api/v4/projects/:id/pipelines/:pipeline_id/jobs',
      :gitlab_job_name => 'package',
      :deploy_folder => '/srv/SelecTunes.Deploy'
    }
  end

@app_settings =
  if File.exist? '/etc/st-update/app-settings.yml'
    YAML.safe_load (File.read '/etc/st-update/app-settings.yml'),
                   symbolize_names: true
  else
    {
      :client_id => '123123123123',
      :client_secret => '123123123123',
      :db_username => 'postgres',
      :db_password => '',
      :db_host => '127.0.0.1',
      :db_name => 'selectunes',
      :redis_url => 'localhost',
      :redirect_uri => 'https://localhost/api/auth/callback'
    }
  end

@header =
  if ARGV.include? '--job'
    'JOB-TOKEN'
  else
    'PRIVATE-TOKEN'
  end

@settings[:gitlab_token] = ARGV[(ARGV.index '-u') + 1] if ARGV.include? '-u'
@settings[:gitlab_job_name] = ARGV[(ARGV.index '-j') + 1] if ARGV.include? '-j'
@settings[:gitlab_project_id] = ARGV[(ARGV.index '-p') + 1] if ARGV.include? '-p'
@settings[:gitlab_pipeline_id] = ARGV[(ARGV.index '-l') + 1] if ARGV.include? '-l'

puts "Using settings !#{@settings}"
puts "Using app settings !#{@app_settings}"

puts '', 'Contacting Pipeline API'

@pipeline_url = "#{@settings[:gitlab_uri]}#{@settings[:gitlab_pipeline_url]}"
@pipeline_url.sub! ':id', @settings[:gitlab_project_id]
@pipeline_url.sub! ':pipeline_id', @settings[:gitlab_pipeline_id]

puts "Using pipeline uri !#{@pipeline_url}"

response = HTTP.headers(@header => @settings[:gitlab_token])
               .get @pipeline_url

json = JSON.parse(response.body, :symbolize_names => true)

@id = json.detect { |x| x[:name] == @settings[:gitlab_job_name] }[:id]

puts "Using Id !#{@id}", ''

@artifacts_uri = "#{@settings[:gitlab_uri]}#{@settings[:gitlab_artifacts_url]}"
@artifacts_uri.sub! ':id', @settings[:gitlab_project_id]
@artifacts_uri.sub! ':job', @id.to_s

puts "Using artifact uri !#{@artifacts_uri}", ''

FileUtils.mkdir_p @settings[:deploy_folder]

puts 'Downloading Build Artifacts'

@response = HTTP.headers(@header => @settings[:gitlab_token])
                .get @artifacts_uri

@artifacts_file = File.join @settings[:deploy_folder], 'artifacts.zip'

if @response.status.success?
  File.open @artifacts_file, 'wb' do |f|
    f.write(@response.body)
  end

  puts 'Artifacts downloaded successfully'
else
  puts "Error downloading artifacts. Server returned !#{@response.status} {#{@response.body}}"
  exit 1
end
exit 0
puts '', 'Unzipping artifacts.zip'
@build_folder = File.join @settings[:deploy_folder], 'build'

Zip::File.open @artifacts_file do |f|
  FileUtils.remove_dir @build_folder, true
  FileUtils.mkdir_p @build_folder

  f.each do |file|
    path = File.join @build_folder, file.name

    puts "Extracting #{file.name} to #{path}"

    f.extract(file, path) { true }
  end
end
puts 'Unzipped artifacts.zip', ''

puts 'Stopping Application'
call_systemd 'stop', 'selectunes.service'
puts 'Successfully stopped application', ''

puts 'Copying application files to run directory'
@run_folder = File.join @settings[:deploy_folder], 'run'
FileUtils.mkdir_p @run_folder
FileUtils.cp_r (File.join @build_folder, 'BackEnd', "build-#{@platform}/."), @run_folder
puts 'Files copied', ''

puts 'Generating Config'
source_file = File.join @run_folder, 'appsettings.erb'
render = ERB.new IO.read source_file

out = render.result_with_hash(@app_settings)
puts out

config_file = File.join @run_folder, 'appsettings.json'
File.open config_file, 'wb' do |f|
  f.write out
end
puts 'Config Generated', ''

puts 'Starting Application'
call_systemd 'start', 'selectunes.service'
puts 'Successfully started application'

printf "\nDone."
exit 0
