NerdDinner 1.0 Sample
----------
NerdDinner 1.0  is an ASP.NET Core sample application  based on the  [ASP.NET MVC  2009 project](http://www.nerddinner.com/) with the same name. This repository is a working single page application(SPA) sample that uses Angular 1 and ASP.NET Core 1.0. 
Set Up 
----------------------

**Prerequisite**
Download .NET Core for Windows, Linux, Mac for more options please visit the [dot.net](https://www.microsoft.com/net/download#core).   

**Running NerdDinner 1.0 Sample**

 1. Clone the sample-NerdDinner application

 ```sh
    git clone https://github.com/aspnet/sample-NerdDinner.git
```

 2. Navigate to your local copy of the sample-NerdDinner and, restore the packages specified in the project.json file.
 
 ```sh
    cd sample-NerdDinner
    dotnet restore 
```
![enter image description here](https://lh3.googleusercontent.com/-HWOSRC2Khbc/V6yTHSs2eYI/AAAAAAAAB1k/vvr4l2Gglm0OlGSMm_HIofsLucW_t7ZWgCLcB/s0/gitclonedotnetrestore2.gif "gitclonedotnetrestore2.gif")
 3. Navigate to the NerdDinner.web folder and run the application 
 
 ```sh
    cd NerdDinner.web
    dotnet run 
```
![dotnetrunlocalhost](https://cloud.githubusercontent.com/assets/2546640/17595124/ce408646-5fb9-11e6-9939-2248c9fdf3cd.gif)
 4. Navigate to` localhost:5000 `
![localhost](https://cloud.githubusercontent.com/assets/2546640/17595325/957160a0-5fba-11e6-9d8a-39df6da6486a.PNG)

 Ready to go sample
----------------------
 
1. NerdDinner 1.0 is [ASP.NET Core 1.0](https://docs.asp.net/en/latest/getting-started.html) application and will run on Windows, Linux,and Mac.

2.  This sample uses [SQLite](https://ef.readthedocs.io/en/latest/platforms/netcore/new-db-sqlite.html) backend.  
    - Notice that when you run the application `dotnet run`  the SQLite database is built 
![buildingthedb](https://cloud.githubusercontent.com/assets/2546640/17596795/0d078d96-5fc1-11e6-9506-0304c4155fb3.gif)

Thank you for using the sample-NerdDinner application.
