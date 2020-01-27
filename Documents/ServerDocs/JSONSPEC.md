# JSON Specifications

## Posting to server

### Route /api/auth/host/login
```json
{
    "SpotifyHash":"[Hash]",
    "PhoneNumber":"[Number]"
}
```

### Route /api/auth/guest/login

```json
{
    "JoinCode": Int32,
    "PhoneNumber":"[Number]"
}
```
