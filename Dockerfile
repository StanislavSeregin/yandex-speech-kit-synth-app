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
COPY src/backend/YandexSpeechKitSynthClient.Api/*.csproj ./YandexSpeechKitSynthClient.Api/
RUN dotnet restore -r $RUNTIME

# Build web app
FROM web-dependencies as web-build
COPY src/frontend/ .
RUN npm run build

# Build backend app with injected web app
FROM backend-dependencies AS backend-build
ARG RUNTIME=win-x64
COPY src/backend/ .
WORKDIR /src/YandexSpeechKitSynthClient.Api
COPY --from=web-build /src/dist ./wwwroot/
RUN dotnet publish -c release -o /app -r $RUNTIME -p:PublishSingleFile=true --self-contained true --no-restore

# Copy compiled app to local folder
FROM scratch AS export-stage
COPY --from=backend-build /app/YandexSpeechKitSynthClient.Api* .
