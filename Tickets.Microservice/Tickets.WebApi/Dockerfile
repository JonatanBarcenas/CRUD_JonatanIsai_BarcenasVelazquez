#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Tickets.WebApi/Tickets.WebApi.csproj", "Tickets.WebApi/"]
COPY ["Tickets.ApplicationServices/Tickets.ApplicationServices.csproj", "Tickets.ApplicationServices/"]
COPY ["Tickets.Core/Tickets.Core.csproj", "Tickets.Core/"]
COPY ["Tickets.DataAccess/Tickets.DataAccess.csproj", "Tickets.DataAccess/"]
COPY ["Tickets.WebApi.Shared/Tickets.WebApi.Shared.csproj", "Tickets.WebApi.Shared/"]
RUN dotnet restore "Tickets.WebApi/Tickets.WebApi.csproj"
COPY . .
WORKDIR "/src/Tickets.WebApi"
RUN dotnet build "Tickets.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Tickets.WebApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Tickets.WebApi.dll"]