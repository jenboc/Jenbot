FROM mcr.microsoft.com/dotnet/sdk:7.0 AS builder
WORKDIR /app
COPY . /app
RUN dotnet restore
RUN dotnet build --configuration Release
RUN ls -laR

FROM builder AS runtime
WORKDIR /app
COPY --from=builder /app/Jenbot/bin/Release/net7.0 .
CMD ./Jenbot