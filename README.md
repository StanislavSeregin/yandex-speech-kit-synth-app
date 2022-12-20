# Yandex SpeechKit synthesizer client

## Build single selfhosted app to `./release-build` via docker for:

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