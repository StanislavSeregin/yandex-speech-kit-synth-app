# Restore web dependencies
FROM node:latest as web-dependencies
WORKDIR /src
COPY src/frontend/package.json src/frontend/package-lock.json ./
RUN npm ci

# Restore backend dependencies
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS backend-dependencies
ARG RUNTIME=win-x64
WORKDIR /src
COPY src/backend/*.sln .
COPY src/backend/App.Api/*.csproj ./App.Api/
COPY src/backend/App.Data/*.csproj ./App.Data/
COPY src/backend/App.YandexClient/*.csproj ./App.YandexClient/
RUN dotnet restore -r $RUNTIME -p:PublishReadyToRun=true

# Build web app
FROM web-dependencies as web-build
COPY src/frontend/ .
RUN npm run build

# Build backend app with injected web app
FROM backend-dependencies AS backend-build
ARG RUNTIME=win-x64
COPY src/backend/ .
WORKDIR /src/App.Api
COPY --from=web-build /src/dist ./wwwroot/
RUN dotnet publish -c release -o /app -r $RUNTIME -p:PublishSingleFile=true -p:PublishTrimmed=true -p:PublishReadyToRun=true --self-contained true --no-restore

# Copy compiled app to local folder
FROM scratch AS export-stage
COPY --from=backend-build /app/App.Api* .
