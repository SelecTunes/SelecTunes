RSpec.describe 'Register' do
  before(:each) do
    @r = HTTP.get('http://localhost:5000')
  end

  it 'successfully registers a host account' do
    expect(@r.code).to eq(200)
  end
end
