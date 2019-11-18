FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build
WORKDIR /src
COPY ["ErrorIt.Api/ErrorIt.Api.csproj", "ErrorIt.Api/"]
RUN dotnet restore "ErrorIt.Api/ErrorIt.Api.csproj"
COPY . .
WORKDIR "/src/ErrorIt.Api"
RUN dotnet build "ErrorIt.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ErrorIt.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ErrorIt.Api.dll"]