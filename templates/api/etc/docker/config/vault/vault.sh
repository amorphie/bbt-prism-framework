sleep 5 &&
curl -X POST 'http://bbt-vault:8200/v1/secret/data/myprojectname' -H "Content-Type: application/json" -H "X-Vault-Token: admin" -d '{ "data": 
 {
   "App:CorsOrigins": "",
   "App:HealthCheckHost": "*:4200",
   "ConnectionStrings:Default": "Server=localhost,5432;Database=MyProjectNameDb;User ID=postgres;Password=postgres;",
   "AllowedHosts": "*"
 } 
 }'