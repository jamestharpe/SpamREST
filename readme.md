# SpamREST

SpamREST is an open source anti-spam microservice for RESTful applications. Spam is reported to SpamREST via a REST API which tells SpamREST where the spam can be found (the end-point). SpamREST collects the reports, then issues an `HTTP DELETE` to the end-points determined to be spam.

## Build and Run SpamREST locally

### Using `dotnet`

```bash
dotnet restore
dotnet run
```

### Using `docker`

#### Using the Published Image

```bash
docker run -it -p 5000:80 jibr/spamrest
```

#### Building the [Docker Image](https://hub.docker.com/r/jibr/spamrest/)

```bash
# Build it:
docker build -f Dockerfile.dev -t local/spamrest .
# Run it:
docker run -it -p 5000:80 local/spamrest
```

Note that `Dockerfile.dev` builds the application from your local codebase to create the image.