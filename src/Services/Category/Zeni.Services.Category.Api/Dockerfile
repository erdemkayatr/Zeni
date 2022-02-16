#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/Services/Category/Zeni.Services.Category.Api/Zeni.Services.Category.Api.csproj", "src/Services/Category/Zeni.Services.Category.Api/"]
COPY ["src/Services/Category/Zeni.Services.Category.Persistence/Zeni.Services.Category.Persistence.csproj", "src/Services/Category/Zeni.Services.Category.Persistence/"]
COPY ["src/Services/Category/Zeni.Services.Category.Domain/Zeni.Services.Category.Domain.csproj", "src/Services/Category/Zeni.Services.Category.Domain/"]
COPY ["src/Services/Category/Zeni.Services.Category.Application/Zeni.Services.Category.Application.csproj", "src/Services/Category/Zeni.Services.Category.Application/"]
RUN dotnet restore "src/Services/Category/Zeni.Services.Category.Api/Zeni.Services.Category.Api.csproj"
COPY . .
WORKDIR "/src/src/Services/Category/Zeni.Services.Category.Api"
RUN dotnet build "Zeni.Services.Category.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Zeni.Services.Category.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Zeni.Services.Category.Api.dll"]
