FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS SDK
EXPOSE 80
EXPOSE 443
WORKDIR /app
COPY *.csproj ./
RUN dotnet restore
COPY . ./
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/core/asp:3.0-buster-slim
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "ComicBookShopCore.Web.dll"]
