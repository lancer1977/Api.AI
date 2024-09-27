#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build  
WORKDIR /src
COPY ["PolyhydraGames.Streaming.WebApi.csproj", "."]
COPY ["NuGet.Docker.Config", "./NuGet.Config"]


RUN dotnet restore "PolyhydraGames.Streaming.WebApi.csproj" --configfile NuGet.Config -v Detailed
COPY . .
WORKDIR "/src/."

RUN dotnet build "PolyhydraGames.Streaming.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PolyhydraGames.Streaming.WebApi.csproj" -c Release -o /app/publish  /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PolyhydraGames.Streaming.WebApi.dll"]