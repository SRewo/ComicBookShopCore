FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS SDK
EXPOSE 80
EXPOSE 443
WORKDIR /app
COPY ./Publish ./
EXPOSE 80
EXPOSE 440
ENTRYPOINT ["dotnet", "ComicBookShopCore.Web.dll"]
