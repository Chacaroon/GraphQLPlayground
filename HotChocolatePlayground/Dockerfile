FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["HotChocolatePlayground/HotChocolatePlayground.csproj", "HotChocolatePlayground/"]
RUN dotnet restore "HotChocolatePlayground/HotChocolatePlayground.csproj"
COPY . .
WORKDIR "/src/HotChocolatePlayground"
RUN dotnet build "HotChocolatePlayground.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "HotChocolatePlayground.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HotChocolatePlayground.dll"]
