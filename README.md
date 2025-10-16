To run the project run these commands:
docker run -it --rm -v /path/to/repo:/app -w /app ergastiriobot dotnet build
docker run -it --rm -v /path/to/repo:/app -w /app ergastiriobot


Example:
if the project is located c:/MyProjects/ErgastirioBot you should do:
docker run -it --rm -v c:/MyProjects/ErgastirioBot  -w /app ergastiriobot dotnet build
docker run -it --rm -v c:/MyProjects/ErgastirioBot  -w /app ergastiriobot
