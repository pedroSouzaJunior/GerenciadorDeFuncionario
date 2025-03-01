# Etapa base para execução da aplicação
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

# Etapa de build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copie o arquivo de solução (certifique-se que o arquivo .sln esteja na raiz)
COPY GerenciadorFuncionariosSolution.sln . 

# Copie os arquivos .csproj de cada projeto
COPY GerenciadorFuncionarios.API/*.csproj GerenciadorFuncionarios.API/
COPY GerenciadorFuncionarios.Aplicacao/*.csproj GerenciadorFuncionarios.Aplicacao/
COPY GerenciadorFuncionarios.Dominio/*.csproj GerenciadorFuncionarios.Dominio/
COPY GerenciadorFuncionarios.Infraestrutura/*.csproj GerenciadorFuncionarios.Infraestrutura/

# Restaure as dependências
RUN dotnet restore "GerenciadorFuncionarios.API/GerenciadorFuncionarios.API.csproj"

# Copie o restante do código
COPY . .

# Compile a aplicação
WORKDIR "/src/GerenciadorFuncionarios.API"
RUN dotnet build "GerenciadorFuncionarios.API.csproj" -c Release -o /app/build

# Etapa de publicação
FROM build AS publish
RUN dotnet publish "GerenciadorFuncionarios.API.csproj" -c Release -o /app/publish

# Imagem final
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GerenciadorFuncionarios.API.dll"]
