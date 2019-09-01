FROM mcr.microsoft.com/dotnet/core/sdk:3.0 AS SDK
EXPOSE 80
EXPOSE 443
WORKDIR /build
COPY ComicBookShopCore.sln ./ComicBookShopCore.sln
COPY ComicBookShopCore.WebAPI ./ComicBookShopCore.WebAPI
COPY ComicBookShopCore.Data ./ComicBookShopCore.Data
COPY ComicBookShopCore.Services ./ComicBookShopCore.Services
RUN dotnet restore ComicBookShopCore.WebAPI/*.csproj
RUN dotnet publish ComicBookShopCore.WebAPI/*.csproj -c Release -o out

    
FROM mcr.microsoft.com/dotnet/core/aspnet:3.0 as build
WORKDIR /app
COPY --from=SDK /build/out .
ARG DB_PASSWORD=@Dmin123
ENV CONNECTION_STRING=Server=db;Database=ComicBookShopCore;User=sa;Password=${DB_PASSWORD};
ENV ASPNETCORE_ENVIRONMENT=Development
ENTRYPOINT ["dotnet", "ComicBookShopCore.WebAPI.dll"]