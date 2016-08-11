NerdDinner 1.0 Sample
----------
NerdDinner 1.0  is an ASP.NET Core sample application is based on the  [ASP.NET MVC  2009 project](http://www.nerddinner.com/) with the same name. This repository is working single page application(SPA) sample that uses Angular 1 and ASP.NET Core 1.0. 
Set Up 
----------------------

**Prerequisite**
Download .NET Core for Windows, Linux, Mac, Docker for more options please visit the [dot.net](https://www.microsoft.com/net/download#core).   

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
 4. Navigate to  
```sh 
 localhost:5000 
```
![localhost](https://cloud.githubusercontent.com/assets/2546640/17595325/957160a0-5fba-11e6-9d8a-39df6da6486a.PNG)
Thank you for using the sample-NerdDinner application.
