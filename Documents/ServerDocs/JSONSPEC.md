# JSON Specifications

## Party Logic Routes

### Route /api/party/joinparty - POST
Expects a JSON User Object and returns a 200 Success Status code upon join
```json
{
    "UserName":"string",
    "PhoneNumber":"string",
    "IsBanned":"false"
}
```

### Route /api/party/leave - POST
Expects a JSON User object and returns a 200 Success code
```json
{
    "UserName":"string",
    "PhoneNumber":"string",
    "IsBanned":"false"
}
```

## Song Routes

### Route /api/song/searchbysong
Expects a JSON Object in the following format. Returns a JSON Object
```json
{
    "UserName":"string",
    "PhoneNumber":"string",
    "IsBanned":"false"
}
```
