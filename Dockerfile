FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
# Railway injects the PORT environment variable. We set ASPNETCORE_HTTP_PORTS to it.
ENV ASPNETCORE_HTTP_PORTS=8080
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["Div.Link.Project01/Div.Link.Project01.Api.csproj", "Div.Link.Project01/"]
COPY ["Div.Link.Project01.BLL/Div.Link.Project01.BLL.csproj", "Div.Link.Project01.BLL/"]
COPY ["Div.Link.Project01.DAL/Div.Link.Project01.DAL.csproj", "Div.Link.Project01.DAL/"]

RUN dotnet restore "./Div.Link.Project01/Div.Link.Project01.Api.csproj"

COPY . .

WORKDIR "/src/Div.Link.Project01"
RUN dotnet build "./Div.Link.Project01.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Div.Link.Project01.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Div.Link.Project01.Api.dll"]