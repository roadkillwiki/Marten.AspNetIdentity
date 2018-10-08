These tests require a postgres server, the Docker command is:
		
docker run -d -p 5432:5432 --name aspnetidentity-postgres -e POSTGRES_USER=aspnetidentity -e POSTGRES_PASSWORD=aspnetidentity postgres