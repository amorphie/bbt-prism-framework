#!/bin/sh

# Let's wait for the Vault server to start.
sleep 5

# Check your secret endpoint
SECRET_CHECK=$(curl -s -o /dev/null -w "%{http_code}" -X GET 'http://bbt-vault:8200/v1/secret/data/myprojectname-secret' -H "X-Vault-Token: admin")

# If there is no secret, create it and set the relevant keys.
if [ "$SECRET_CHECK" -ne 200 ]; then
  curl -X POST 'http://bbt-vault:8200/v1/secret/data/myprojectname-secret' \
  -H "Content-Type: application/json" \
  -H "X-Vault-Token: admin" \
  -d '{
    "data": {
      "App:CorsOrigins": "",
      "App:HealthCheckHost": "*:4200",
      "ConnectionStrings:Default": "Server=localhost,5432;Database=MyProjectNameDb;User ID=postgres;Password=postgres;",
      "AllowedHosts": "*"
    }
  }'
else
  echo "Secret 'myprojectname-secret' already exists."
fi