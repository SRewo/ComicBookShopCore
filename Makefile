include .env
ifndef DB_PASSWORD
$(Error The DB_PASSWORD variable is missing)
endif

build:
	@docker-compose build --build-arg DB_PASSWORD=$(DB_PASSWORD) $(APP)
start:
	@docker-compose up -d $(APP)
stop:
	@docker-compose stop $(APP)
prune:
	@docker system prune -af
restart:
	@make -s stop
	@make -s start
rebuild:
	@docker-compose build --no-cache --build-arg DB_PASSWORD=$(DB_PASSWORD)