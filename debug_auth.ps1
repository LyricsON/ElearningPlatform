
$loginUrl = "http://localhost:5165/api/auth/login"
$usersUrl = "http://localhost:5165/api/users"

$body = @{
    email = "admin@elearning.local"
    password = "Admin123!"
} | ConvertTo-Json

Write-Host "Logging in..."
try {
    $response = Invoke-RestMethod -Uri $loginUrl -Method Post -Body $body -ContentType "application/json"
    $token = $response.token
    Write-Host "Login successful. Token received."
    
    # Decode token to show claims (simple parse without verification)
    $payloadIndex = $token.IndexOf(".") + 1
    $payloadLength = $token.LastIndexOf(".") - $payloadIndex
    # Add padding if needed
    $payload = $token.Substring($payloadIndex, $payloadLength)
    switch ($payload.Length % 4) {
        2 { $payload += "==" }
        3 { $payload += "=" }
    }
    $decodedBytes = [System.Convert]::FromBase64String($payload)
    $decodedText = [System.Text.Encoding]::UTF8.GetString($decodedBytes)
    Write-Host "Token Payload Claims:"
    Write-Host $decodedText

    Write-Host "Calling Users API with token..."
    $headers = @{ Authorization = "Bearer $token" }
    try {
        $usersResponse = Invoke-RestMethod -Uri $usersUrl -Method Get -Headers $headers
        Write-Host "Users API response received (Count: $($usersResponse.Count))"
    } catch {
        Write-Host "Error calling Users API:"
        Write-Host $_.Exception.Message
        if ($_.Exception.Response) {
             # Read the error stream
             $reader = New-Object System.IO.StreamReader($_.Exception.Response.GetResponseStream())
             $responseBody = $reader.ReadToEnd()
             Write-Host "Response Body: $responseBody"
        }
    }
} catch {
    Write-Host "Login Failed:"
    Write-Host $_.Exception.Message
     if ($_.Exception.Response) {
        $reader = New-Object System.IO.StreamReader($_.Exception.Response.GetResponseStream())
        $responseBody = $reader.ReadToEnd()
        Write-Host "Response Body: $responseBody"
    }
}
