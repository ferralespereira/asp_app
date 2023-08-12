# To deploy and Install asp.net Core project

## Download repository installation package.
```
source  /etc/os-release
wget https://packages.microsoft.com/config/ubuntu/$VERSION_ID/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
```

## Install the package using dpkg command:
```
sudo dpkg -i packages-microsoft-prod.deb
```

## Install the .NET SDK with the following command
```
sudo apt update
sudo apt install -y dotnet-sdk-7.0
```

## Check for the version to confirm successful installation.
```
dotnet --version
```

## Clone the project
```
git clone https://github.com/ferralespereira/asp_app.git
```

## Configure the application deployment
```
dotnet publish --configuration Release
```

## Virtual Host Configuration
* Enable the necessary Apache modules with the following command
```
sudo a2enmod rewrite
sudo a2enmod proxy
sudo a2enmod proxy_http
sudo a2enmod request
sudo a2enmod headers
```

## Create a new configuration file
```
sudo nano /etc/apache2/sites-available/asp_app.conf
```
* Append the following details to the file
```
<VirtualHost *:*>
    RequestHeader set "X-Forwarded-Proto" expr=%{REQUEST_SCHEME}
</VirtualHost>

<VirtualHost *:80>
    ProxyPreserveHost On
    ProxyPass / http://127.0.0.1:5000/
    ProxyPassReverse / http://127.0.0.1:5000/
    ServerName asp.javierfoler.com
    ServerAlias *.asp.javierfoler.com
    ErrorLog  /var/log/apache2/asp_app_error.log
    CustomLog /var/log/apache2/asp_app_access.log common
</VirtualHost>
```

## Restart Apache to apply changes.
```
sudo systemctl restart apache2
```

## Monitor the App with systemd
* Create a service file:
```
sudo nano /etc/systemd/system/asp_app.service
```
* Append the following details to the file.

```
[Unit]
Description=Example .NET Web API App running on Ubuntu

[Service]
WorkingDirectory=/var/www/asp_app
ExecStart=/usr/bin/dotnet /var/www/asp_app/bin/Release/net7.0/asp_app.dll
Restart=always
# Restart service after 10 seconds if the dotnet service crashes:
RestartSec=10
KillSignal=SIGINT
SyslogIdentifier=dotnet-example
User=www-data
Environment=ASPNETCORE_ENVIRONMENT=Production 

[Install]
WantedBy=multi-user.target
```

* Then start and enable the service:
```
sudo systemctl start asp_app.service
sudo systemctl enable asp_app.service
```

## Check the status of the service.
```
systemctl status asp_app.service
```

##  Secure the Application
* Install the UFW package.
```
sudo apt install ufw
```
* Enable the firewall with the following command.
```
sudo ufw enable
```
* Configure Firewall to open only the ports needed for the app. In this case, ports 80 and 443 are used.
```
sudo ufw allow 80/tcp
sudo ufw allow 443/tcp
```
* Then check the status with the following command.
```
sudo ufw status
```

# OTHERS
* EF (Entity Framework) version
```
dotnet ef --version
```