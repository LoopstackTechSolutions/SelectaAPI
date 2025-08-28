FROM mysql:8.0

# MySQL credentials
ENV MYSQL_ROOT_PASSWORD=root
ENV MYSQL_USER=selecta
ENV MYSQL_PASSWORD=selecta123
ENV MYSQL_DATABASE=dbSelecta

# Copy initialization script
COPY script.sql /docker-entrypoint-initdb.d/
