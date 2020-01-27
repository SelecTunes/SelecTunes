require 'sinatra'
require 'http'
require 'json'
require 'base64'

set port: 8080

get '/' do
  config = File.readlines('config.txt')
  @client_id = config[0].delete "\r\n"
  @client_secret = config[1].delete "\r\n"

  if request.params.include? 'code'
    File.open('response.txt', 'w') do |f|
      f.write(request.params)
    end

    puts "YEET USER: #{@client_id} PASS: #{@client_secret}"

    r = HTTP.basic_auth(user: @client_id, pass: @client_secret)
      .post('https://accounts.spotify.com/api/token', form: {
        grant_type: 'authorization_code',
        code: request.params['code'],
        redirect_uri: 'http://localhost:8080/'
      })

    File.open('id.txt', 'w') do |f|
      f.write(r)
    end

    return erb :play
  end

  erb :index
end
