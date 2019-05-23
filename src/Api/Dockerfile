FROM mcr.microsoft.com/dotnet/core/aspnet:2.2 AS base

    
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build
WORKDIR /app

COPY . ./

RUN dotnet restore Api/Api.csproj
RUN dotnet build Api/Api.csproj -c Release -o /out

FROM build AS publish
RUN dotnet publish Api/Api.csproj -c Release -o /out

FROM base AS final
RUN mkdir -p /usr/local/etc/openssl
RUN cd /usr/local/etc/openssl
WORKDIR /usr/local/etc/openssl
RUN curl https://curl.haxx.se/ca/cacert.pem -o cert.pem
WORKDIR /app
COPY --from=publish /out .
ENTRYPOINT ["dotnet", "Api.dll"]