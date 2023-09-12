# To deploy and Install asp.net Core project

* Download repository installation package.
```
source  /etc/os-release
wget https://packages.microsoft.com/config/ubuntu/$VERSION_ID/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
```

* Install the package using dpkg command:
```
sudo dpkg -i packages-microsoft-prod.deb
```

* Install the .NET SDK with the following command
```
sudo apt update
sudo apt install -y dotnet-sdk-7.0
```

* Check for the version to confirm successful installation.
```
dotnet --version
```

* Clone the project
```
git clone https://github.com/ferralespereira/asp_app.git
```

* Configure the application deployment
```
dotnet publish --configuration Release
```

### Virtual Host Configuration
* Enable the necessary Apache modules with the following command
```
sudo a2enmod rewrite
sudo a2enmod proxy
sudo a2enmod proxy_http
sudo a2enmod request
sudo a2enmod headers
```

* Create a new configuration file
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

* Restart Apache to apply changes.
```
sudo systemctl restart apache2
```

### Monitor the App with systemd
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

* Check the status of the service.
```
systemctl status asp_app.service
```

### Secure the Application
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

# Migrations and Connection to MySql Database
* Run the following .NET CLI commands:
```
dotnet tool uninstall --global dotnet-aspnet-codegenerator
dotnet tool install --global dotnet-aspnet-codegenerator
dotnet tool uninstall --global dotnet-ef
dotnet tool install --global dotnet-ef
```
* On macOS and Linux, export the scaffold tool path:
```
export PATH=$HOME/.dotnet/tools:$PATH
```
*  Install this 3 Packages
```
dotnet add EntityFrameworkCore
dotnet add Microsoft.EntityFrameworkCore.Tools
dotnet add Pomelo.EntityFrameworkCore.MySql
```
* Create a `Data` folder and inside Create `AppDbContext.cs` file and put this code in there
```
using Microsoft.EntityFrameworkCore;
using asp_app.Models;

namespace asp_app.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Employee> Employees { get; set; }



        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Replace with your connection string.
            var connectionString = "server=localhost;user=root;password=password;database=asp_app";
            
            // Update your MySql version here "new Version(*, *, *)"
            var serverVersion = new MySqlServerVersion(new Version(8, 0, 34));
            optionsBuilder.UseMySql(connectionString, serverVersion);
        }
    }
}
```
* Create `Emploee.cs` file into `Models` folder and put this code in there
```
namespace asp_app.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
    }
}
```
* Run this to Create the Migration
```
dotnet ef migrations add InitialCreate
```
* Run this to Update the migration into the Database
```
dotnet ef database update
```
* To Remove Migration
```
dotnet ef migrations remove
```

* References:

`https://zetbit.tech/categories/asp-dot-net-core/41/setup-mysql-connection-dot-net-7-asp-net-core`

`https://www.youtube.com/watch?v=W8Rjt54GBuo`

`https://learn.microsoft.com/en-us/aspnet/core/tutorials/first-mvc-app/adding-model?view=aspnetcore-7.0&tabs=visual-studio-code`

# OTHERS
* Version of my Framework
```
dotnet --version 
``` 
* EF (Entity Framework) version
```
dotnet ef --version
```
* List of package do I have on my project
```
dotnet list package 
``` 
* Adding dotnet Packages 
```
dotnet add package 
```
* Removing dotnet Packages 
```
dotnet remove package 
```
* Restores the dependencies and tools of a project
```
dotnet restore
```

