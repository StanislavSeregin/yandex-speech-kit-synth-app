# Restore web dependencies
FROM node:latest as web-dependencies
WORKDIR /src
COPY src/frontend/package.json src/frontend/package-lock.json ./
RUN npm ci

# Build web assets
FROM web-dependencies as web-build
COPY src/frontend/ .
RUN npm run build

# Build backend app
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS backend-build
WORKDIR /src
COPY src/backend/ .
WORKDIR /src/YandexSpeechKitSynthClient.Api
COPY --from=web-build /src/dist ./wwwroot/
RUN dotnet publish -c release -o /app -r win-x64 -p:PublishSingleFile=true --self-contained true

# Copy compiled app to local folder
FROM scratch AS export-stage
COPY --from=backend-build /app/YandexSpeechKitSynthClient.Api.exe .
