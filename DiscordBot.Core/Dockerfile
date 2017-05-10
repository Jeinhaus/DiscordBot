#FROM microsoft/dotnet:2.0.0-runtime
FROM microsoft/dotnet:2.0.0-preview1-runtime
ARG source
WORKDIR /app
COPY ${source:-obj/Docker/publish} .
ENTRYPOINT ["dotnet", "DiscordBot.Core.dll"]
