FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
RUN apt update && apt install -y nodejs
COPY ["DevSkill.Inventory.Web/*.csproj", "DevSkill.Inventory.Web/"]
COPY ["DevSkill.Inventory.Application/*.csproj", "DevSkill.Inventory.Application/"]
COPY ["DevSkill.Inventory.Domain/*.csproj", "DevSkill.Inventory.Domain/"]
COPY ["DevSkill.Inventory.Infrastructure/*.csproj", "DevSkill.Inventory.Infrastructure/"]
RUN dotnet restore "DevSkill.Inventory.Web/DevSkill.Inventory.Web.csproj"
COPY . .
WORKDIR "/src/DevSkill.Inventory.Web"
RUN dotnet build "DevSkill.Inventory.Web.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "DevSkill.Inventory.Web.csproj" -c Release -o /app

FROM build AS final
WORKDIR /app
COPY --from=publish /app .
EXPOSE 80
ENTRYPOINT ["dotnet", "DevSkill.Inventory.Web.dll"]