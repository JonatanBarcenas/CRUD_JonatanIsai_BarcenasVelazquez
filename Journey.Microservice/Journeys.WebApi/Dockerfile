#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Journeys.WebApi/Journeys.WebApi.csproj", "Journeys.WebApi/"]
COPY ["Journey.Core/Journeys.Core.csproj", "Journey.Core/"]
COPY ["Journey.DataAccess/Journeys.DataAccess.csproj", "Journey.DataAccess/"]
COPY ["Journeys.ApplicationServices/Journeys.ApplicationServices.csproj", "Journeys.ApplicationServices/"]
COPY ["Journeys.WebApi.Shared/Journeys.WebApi.Shared.csproj", "Journeys.WebApi.Shared/"]
RUN dotnet restore "Journeys.WebApi/Journeys.WebApi.csproj"
COPY . .
WORKDIR "/src/Journeys.WebApi"
RUN dotnet build "Journeys.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Journeys.WebApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Journeys.WebApi.dll"]