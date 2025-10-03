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

# Documentação da API - Endpoints

## AuthController - Autenticação

* POST /suporte/v1/auth/login
* Descrição: Realiza autenticação do usuário no sistema
* Permissão: Público 

Entrada:
```json
{
  "email": "string (obrigatório)",
  "senha": "string (obrigatório)"
}
````
Retorno:
```json
 {
  "dados": {
    "usuario": {
      "id": "number",
      "nome": "string",
      "email": "string"
    },
    "token": "string (JWT)"
  },
  "mensagem": "Login realizado com sucesso!",
  "status": true
}
````
## ChamadoController - Gestão de Chamados

* POST /suporte/v1/chamado
* Descrição: Cria um novo chamado 
* Permissão: Apenas usuários com role no token "Usuario"

Entrada:
```json
{
  "descricao": "string (obrigatório - descrição do problema)"
}
````
Retorno:
```json
{
  "dados": {
    "id": "number",
    "descricao": "string",
    "resposta": "string (pode ser null)",
    "status": "number (Pendente, Respondido)",
    "chamadoDataCriacao": "datetime",
    "usuarioId": "number",
    "prioridade": "number (0=Baixa, 1=Media, 2=Alta, 3=Analisando)",
    "nome": "string (nome do usuário)"
  },
  "mensagem": "Chamado criado com sucesso!",
  "status": true
}
````
* GET /suporte/v1/chamado/meus
* Descrição: Lista todos os chamados do usuário logado
* Permissão: Apenas usuários com role "Usuario"

Entrada: Nenhuma

Retorno:
```json
{
  "dados": [
    {
      "id": "number",
      "descricao": "string",
      "resposta": "string",
      "status": "number",
      "chamadoDataCriacao": "datetime",
      "usuarioId": "number",
      "prioridade": "number",
      "nome": "string"
    }
  ],
  "mensagem": "Chamados carregados com sucesso!",
  "status": true
}
````
* PUT /suporte/v1/chamado/{id}
* Descrição: Responde a um chamado existente
* Permissão: Apenas administradores (role "Administrador")

Parâmetros:
* id: number (ID do chamado a ser respondido)

Entrada:
```json
{
  "resposta": "string (obrigatório - resposta do administrador)"
}
````
Retorno:
```json
{
  "dados": {
    "id": "number",
    "descricao": "string",
    "resposta": "string",
    "status": "number (atualizado para respondido)",
    "chamadoDataCriacao": "datetime",
    "usuarioId": "number",
    "prioridade": "number",
    "nome": "string"
  },
  "mensagem": "Chamado respondido com sucesso!",
  "status": true
}
````
* GET /suporte/v1/chamado
* Descrição: Lista todos os chamados do sistema
* Permissão: Apenas administradores (role "Administrador")

Entrada: Nenhuma
Retorno:
```json
{
  "dados": [
    {
      "id": "01",
      "descricao": "string",
      "resposta": "string",
      "status": "number",
      "chamadoDataCriacao": "datetime",
      "usuarioId": "number",
      "prioridade": "number",
      "nome": "string"
    }
   { 
      "id": "02",
      "descricao": "string",
      "resposta": "string",
      "status": "number",
      "chamadoDataCriacao": "datetime",
      "usuarioId": "number",
      "prioridade": "number",
      "nome": "string"
    }
  ],
  "mensagem": "Todos os chamados carregados com sucesso!",
  "status": true
}
````
## UsuarioController - Gestão de Usuários

* POST /suporte/v1/usuario
* Descrição: Registra um novo usuário comum no sistema
* Permissão: Público

Entrada: 
```json
{
  "nome": "string (obrigatório)",
  "email": "string (obrigatório)",
  "senha": "string (obrigatório, 5-10 caracteres)",
  "confirmarSenha": "string (obrigatório)",
  "aceitouPoliticaPrivacidade": "boolean (obrigatório)"
}
````
Retorno:
```json
{
  "dados": {
    "id": "number",
    "nome": "string",
    "email": "string"
  },
  "mensagem": "Usuário cadastrado com sucesso!",
  "status": true
}
````
* POST /suporte/v1/usuario/admin
* Descrição: Registra um novo administrador no sistema
* Permissão: Apenas administradores existentes
  
Entrada: 
```json
{
  "nome": "string (obrigatório)",
  "email": "string (obrigatório)",
  "senha": "string (obrigatório, 5-10 caracteres)",
  "confirmarSenha": "string (obrigatório)",
  "aceitouPoliticaPrivacidade": "boolean (obrigatório)"
}
````
Retorno:
```json
{
  "dados": {
    "id": "number",
    "nome": "string",
    "email": "string"
  },
  "mensagem": "Administrador cadastrado com sucesso!",
  "status": true
}
````
* PUT /suporte/v1/usuario/{id}
* Descrição: Atualiza dados do usuário (apenas próprio usuário)
* Permissão: Proprietário da conta 
  
Parâmetros:
* id: number (ID do usuário a ser atualizado)

Entrada:
```json
{
  "nome": "string (opcional)",
  "email": "string (opcional)",
  "senhaAtual": "string (obrigatório apenas se alterar senha)",
  "novaSenha": "string (opcional, 5-10 caracteres)",
  "confirmarNovaSenha": "string (opcional)"
}
````
Retorno:
```json
{
  "dados": true,
  "mensagem": "Dados atualizados com sucesso!",
  "status": true
}
````
* DELETE /suporte/v1/usuario/{id}
* Descrição: Deleta conta do usuário (apenas próprio usuário)
* Permissão: Proprietário da conta ou administrador

Parâmetros:
* id: number (ID do usuário a ser deletado)

Entrada: Nenhuma

Retorno:
```json
{
  "dados": true,
  "mensagem": "Conta deletada com sucesso!",
  "status": true
}
````
## Notas Importantes

Autenticação

* Use o token JWT retornado no login no header: Authorization: Bearer {token}
* Tokens são necessários para todos os endpoints exceto login e registro de usuário comum

Status dos Chamados

O status é criado automaticamente sempre como pendente 
* Pendente
* Resspondido

Prioridade dos chamados

A prioridade sempre é definida automaticamente pela OpenIA 
* Baixa
* Média
* Alta
* Analisando (para casos de erro no serviço da OpenIA ou chamados irregulares)


