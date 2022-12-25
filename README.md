# Yandex SpeechKit synth app

Cross-platform app for text-to-speech using yandex api.

## How to build

### Run shell command (docker required):

- Windows

```bash
docker build --file Dockerfile --output release-build --build-arg RUNTIME=win-x64 .
```

- Linux

```bash
docker build --file Dockerfile --output release-build --build-arg RUNTIME=linux-x64 .
```

- OSX

```bash
docker build --file Dockerfile --output release-build --build-arg RUNTIME=osx-x64 .
```

## How to use

- Run `./release-build/App.Api.exe` or `./release-build/App.Api` app
- Press `Web GUI` button to open