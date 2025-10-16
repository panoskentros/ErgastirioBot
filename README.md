Download Docker from: https://www.docker.com/products/docker-desktop/

From CMD on Windows or Terminal for MacOS/Linux run ONLY FOR THE FIRST TIME:
docker build -t ergastiriogot .

To run the project run these commands:
docker run -it --rm -v /path/to/repo:/app -w /app ergastiriobot dotnet build
docker run -it --rm -v /path/to/repo:/app -w /app ergastiriobot


Example:
if the project is located c:/MyProjects/ErgastirioBot you should do:
docker run -it --rm -v c:/MyProjects/ErgastirioBot:/app  -w /app ergastiriobot dotnet build
docker run -it --rm -v c:/MyProjects/ErgastirioBot:/app  -w /app ergastiriobot

You can modify the code as per your liking and then you execute these two commands to build and run it.
