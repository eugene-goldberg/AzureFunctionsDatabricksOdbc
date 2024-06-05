# Use the official Microsoft Azure Functions base image
FROM mcr.microsoft.com/azure-functions/dotnet:4.0-appservice

# Install dependencies
RUN apt-get update && \
    apt-get install -y curl apt-transport-https unixodbc-dev && \
    curl https://packages.microsoft.com/keys/microsoft.asc | apt-key add - && \
    curl https://packages.microsoft.com/config/ubuntu/20.04/prod.list > /etc/apt/sources.list.d/mssql-release.list && \
    apt-get update && \
    ACCEPT_EULA=Y apt-get install -y msodbcsql17 && \
    apt-get clean && \
    rm -rf /var/lib/apt/lists/*

# Copy the local Simba Spark ODBC Driver .deb file into the image
COPY simbaspark_2.6.9.1009-2_amd64.deb /tmp/simbaspark_2.6.9.1009-2_amd64.deb

# Install dependencies required by Simba Spark ODBC Driver
RUN apt-get update && \
    apt-get install -y libsasl2-modules-gssapi-mit

# Install the Simba Spark ODBC Driver
RUN dpkg -i /tmp/simbaspark_2.6.9.1009-2_amd64.deb && \
    apt-get install -f -y && \
    rm /tmp/simbaspark_2.6.9.1009-2_amd64.deb

# Set environment variables for ODBC
ENV ODBCINI=/etc/odbc.ini
ENV ODBCSYSINI=/etc

# Copy the function app files to the image
COPY . /home/site/wwwroot

# Expose the port Azure Functions runtime listens to
EXPOSE 80

# Set up entry point
ENTRYPOINT [ "dotnet", "AzureFunctions.dll" ]

