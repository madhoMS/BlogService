#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["BlogService/BlogService.csproj", "BlogService/"]
COPY ["BlogService.Infrastructure/BlogService.Infrastructure.csproj", "BlogService.Infrastructure/"]
COPY ["BlogService.Core/BlogService.Core.csproj", "BlogService.Core/"]
COPY ["BlogService.Application/BlogService.Application.csproj", "BlogService.Application/"]
COPY ["BlogService.Common/BlogService.Common.csproj", "BlogService.Common/"]
COPY ["BlogService.Domain/BlogService.Domain.csproj", "BlogService.Domain/"]
RUN dotnet restore "BlogService/BlogService.csproj"
COPY . .
WORKDIR "/src/BlogService"
RUN dotnet build "BlogService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BlogService.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BlogService.dll"]