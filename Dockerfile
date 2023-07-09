FROM mcr.microsoft.com/dotnet/sdk:7.0 AS builder
WORKDIR /app
COPY . /app
RUN nuget restore
RUN dotnet build --configuration Release

FROM build AS runtime
WORKDIR /app
COPY --from builder /app/bin/release .
CMD ./Jenbot