FROM mcr.microsoft.com/dotnet/core/sdk:3.0 AS SDK
EXPOSE 80
EXPOSE 443
WORKDIR /build
COPY ComicBookShopCore.sln ./ComicBookShopCore.sln
COPY ComicBookShopCore.Web ./ComicBookShopCore.Web
COPY ComicBookShopCore.Data ./ComicBookShopCore.Data
RUN dotnet restore ComicBookShopCore.Web/*.csproj
RUN dotnet publish ComicBookShopCore.Web/*.csproj -c Release -o out

FROM mcr.microsoft.com/dotnet/core/aspnet:3.0 as build
WORKDIR /app
COPY --from=SDK /build/out .
ENTRYPOINT ["dotnet", "ComicBookShopCore.Web.dll", "--environment=Development"]
