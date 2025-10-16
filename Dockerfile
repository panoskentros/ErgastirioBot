FROM mcr.microsoft.com/playwright:v1.55.0-noble

# Install .NET SDK
RUN apt-get update && apt-get install -y curl && \
    curl -sSL https://dot.net/v1/dotnet-install.sh | bash /dev/stdin -c 8.0

# Set environment variables for .NET
ENV DOTNET_ROOT=/root/.dotnet
ENV PATH="$PATH:/root/.dotnet:/root/.dotnet/tools"

# Set workdir
WORKDIR /app

# Copy project
COPY . .

# Restore/build your project
RUN dotnet restore
RUN dotnet build

# Default command
CMD ["dotnet", "run"]

