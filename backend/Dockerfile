# --- STAGE: development with hot‐reload ---
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS dev
WORKDIR /app

# Cópia do csproj e restore (cache de dependências)
COPY ["/ApiBackend/ApiBackend.csproj", "./"]
RUN dotnet restore "ApiBackend.csproj"

COPY . .

# 4) Habilita polling watcher para hot-reload em volumes montados
ENV DOTNET_USE_POLLING_FILE_WATCHER=1


# 6) Comando de inicialização: hot-reload
#    Aqui não sobra nenhuma linha iniciando com "dotnet" sem prefixo.
CMD ["dotnet", "watch", "run", "--project", "ApiBackend.csproj", "--no-launch-profile", "--no-hot-restart"]