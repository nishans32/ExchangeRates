FROM mcr.microsoft.com/dotnet/sdk:3.1 as build 
ARG Version
WORKDIR /app

COPY ./ExchangeRates.Common.csproj ./
RUN dotnet restore  --verbosity detailed

COPY ./ ./
RUN dotnet build /p:Version=$Version
RUN dotnet pack /p:Version=$Version -c Release --no-restore --no-build -o /artifacts

ENTRYPOINT ["dotnet", "nuget", "push", "/artifacts/*.nupkg"]
CMD ["--source", "https://api.nuget.org/v3/index.json", "--api-key", "test"]

