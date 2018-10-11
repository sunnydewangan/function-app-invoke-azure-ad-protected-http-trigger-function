[Net.ServicePointManager]::SecurityProtocol = [Net.SecurityProtocolType]::Tls12

$clientId = '{Client ID}' # Azure AD Client ID
$clientSecret = '{Client Secret}' #Azure AD Client Secret
$tokenEndpoint = 'https://login.microsoftonline.com/{TanantID}/oauth2/token'  #AD Endpoint where we request for token

$body = @{
    resource = $clientId
    client_id = $clientId
    client_secret = $clientSecret
    grant_type = "client_credentials"
}

# Making a POST call to get Token
$result = Invoke-RestMethod -Method Post -Uri $tokenEndpoint -Body $body
$token = $result.access_token
Write-Information "Token: $token"

$urlToAccess = "{Your HttpTrigger Function URL with Code}"

# Invoke HttpTrigger Function while passing access token
$response = Invoke-WebRequest -Uri $urlToAccess -Method "GET" -Headers @{ "Authorization" = "Bearer " + $token }

Write-Information ("Status code: {0}`r`nBody: {1}" -f $response.StatusCode, $response.Content)

#Remove-Variable * -ErrorAction SilentlyContinue; Remove-Module *; $error.Clear(); Clear-Host