FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

EXPOSE 8080

#####################
#PUPPETEER RECIPE based on https://github.com/hardkoded/puppeteer-sharp/issues/1180#issuecomment-1015532968
RUN apt-get update && apt-get -f install && apt-get -y install wget gnupg2 apt-utils
RUN wget -q -O - https://dl.google.com/linux/linux_signing_key.pub | apt-key add -
RUN echo 'deb [arch=amd64] http://dl.google.com/linux/chrome/deb/ stable main' >> /etc/apt/sources.list
RUN apt-get update \
&& apt-get install -y google-chrome-stable --no-install-recommends --allow-downgrades fonts-ipafont-gothic fonts-wqy-zenhei fonts-thai-tlwg fonts-kacst fonts-freefont-ttf
#END PUPPETEER RECIPE
######################
ENV PUPPETEER_EXECUTABLE_PATH "/usr/bin/google-chrome-stable"

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["Converter.Service/Converter.Service.csproj", "Converter.Service/"]
RUN dotnet restore "Converter.Service/Converter.Service.csproj"

COPY ["WebApi/WebApi.csproj", "WebApi/"]
RUN dotnet restore "WebApi/WebApi.csproj"

COPY . .

WORKDIR "/src/WebApi"
RUN dotnet build "WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENV ASPNETCORE_ENVIRONMENT=Docker

ENTRYPOINT ["dotnet", "WebApi.dll"]