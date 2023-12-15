FROM mcr.microsoft.com/dotnet/sdk:6.0.302 AS build-env
WORKDIR /Source

# Copy everything
COPY . ./
# Restore as distinct layers
RUN dotnet restore 
# Build and publish a release
RUN dotnet publish -c Release -o /App --no-restore

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0.25
WORKDIR /App
COPY --from=build-env /App .
COPY ./BinksNoSake.API/Resources ./Resources
COPY ./BinksNoSake.API/BinksNoSake.db .
EXPOSE 5000
ENV ASPNETCORE_URLS=http://+:5000
ENTRYPOINT ["dotnet", "BinksNoSake.API.dll"]