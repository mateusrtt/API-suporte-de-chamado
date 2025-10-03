# Guia para Rodar e configurar a API

## Pré-requisitos

* Visual Studio 2022/2023
* .NET 8 SDK instalado
* SQL Server 
* Verifique se o .NET 8 está instalado na sua máquina rode esse comando no cmd:

 ```
   dotnet --version

 ```

> ⚠️ `dotnet restore` **não instala o .NET**. Cada máquina precisa ter o .NET 8 SDK instalado caso não tenha instale: https://dotnet.microsoft.com/pt-br/download.

---

## Passo a Passo

### 1. Clonar o projeto

1. Copie a URL HTTPS do repositório do GitHub.
2. Abra o Visual Studio.
3. No menu, selecione:

   ```
   Arquivo → Abrir → Clonar Repositório
   
   ```
4. Cole a URL do repositório e selecione a pasta de destino.
5. Clique em **Clonar**.

### 3. Restaurar pacotes NuGet

1. Abra o **Console do Gerenciador de Pacotes**:
> Ferramentas → Gerenciador de Pacotes NuGet → Console do Gerenciador de Pacotes
  
2. Rode o comando:
   
    ```
   dotnet restore
    
   ```
> * Isso vai baixar todos os pacotes do projeto (Entity Framework, JWT, Swagger, etc.) nas versões especificadas

### 4. Configurar `appsettings.json`
Edite o arquivo `appsettings.json` com suas informações locais:
```json
"ConnectionStrings": {
"DefaultConnection": "Server=localhost;Database=MinhaApiDb;Trusted_Connection=True;TrustServerCertificate=True"
}
````
> * ConnectionStrings → ajuste servidor, banco, usuário e senha para as sua configurações

A conexão com o SQL Server pode ser feita de duas formas:

1. Autenticação do Windows (sem usuário/senha)
```json
"DefaultConnection": "Server=SEU_SERVIDOR;Database=SistemaChamadoPIM;Trusted_Connection=True;TrustServerCertificate=True"
````
2. Autenticação SQL Server (com usuário e senha)
```json
"DefaultConnection": "Server=SEU_SERVIDOR;Database=SistemaChamadoPIM;User Id=sa;Password=minhaSenha123;TrustServerCertificate=True"
````
> ⚠️ Escolha a forma que funcione melhor para a configuração do seu banco

### 5. Aplicar migrações (criar banco e tabelas)

1. No **Console do Gerenciador de Pacotes**, selecione **Infrastructure** como **Projeto Padrão** (Default Project).
   
   ```
   Update-Database
   
   ```
> Isso cria o banco e as tabelas com todos os relacionamentos definidos no EF Core. **Não cria dados, apenas estrutura.**

### 6. Rodar a API

* No Visual Studio, pressione **F5** para rodar a API
* A API é configurada com Swagger como página inicial, então ele abre automaticamente no navegador exibindo os endpoints 
* Use o postman para testar os endpoints


