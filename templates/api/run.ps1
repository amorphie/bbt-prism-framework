<# Docker up #>

cd "./etc/docker/"
docker network create bbt-development
docker-compose up -d
cd ../..
 
<# Dapr run #>
dapr run --app-id "MyProjectName" --app-port "4200" --components-path "./etc/Dapr/Components" --dapr-grpc-port "42011" --dapr-http-port "42010"