# 1 Objetivos de negócio

O objetivo de negócio do sistema é modernizar o atendimento de chamados técnicos por meio do uso da inteligência artificial na automação de prioridade, agilizando a triagem, reduzindo o tempo de resposta e aumentando a produtividade. A solução também apoia a equipe técnica na priorização dos chamados, oferece visibilidade ao usuário com acompanhamento em tempo real e automatiza tarefas repetitivas para reduzir custos operacionais. Além disso, garante a conformidade com a LGPD, assegurando a proteção dos dados dos usuários.

# 1.1 Perfis de usuário e funcionalidades

## Usuário comum

**Funcionalidades:** Login, cadastro, abertura de chamados, acompanhamento dos seus chamados e respostas, gerenciamento de conta com edição de dados e exclusão de conta.

## Administrador

**Funcionalidades:** Login, cadastro, visualização da lista de todos os chamados, responder chamados, gerenciamento de conta com edição de dados e exclusão de conta.

# 2 TIPOS DE PROCESSOS

Apresentaremos os tipos de processos que compõem nosso sistema, divididos em processos primários, de apoio e de gestão. Demonstraremos como cada conjunto de processos contribui para o funcionamento.

## 2.1 Processos primários

- **Cadastro e login** - Permite que usuários e administradores acessem o sistema
- **Abertura de chamados** - Usuário comum cria chamados
- **Visualização de chamados e acompanhamento** - Usuário acompanha status, prioridade e resposta dos seus chamados
- **Visualização de todos os chamados** - Administradores tem acesso a lista de todos os chamados do sistema
- **Respostas de chamados** - Administradores responde os chamados
- **Gerenciamento de conta** - Permite visualizar, atualizar e excluir conta

## 2.2 Processos de apoio

- **Autenticação e autorização JWT** - Garante segurança e controla acesso
- **Integração com a OpenAI** - Responsável pela categorização de prioridade automática dos chamados
- **Criptografia e armazenamento seguro de senhas** - Protege credenciais e dados sensíveis dos usuários
- **Persistência de dados** - Gerencia o armazenamento, integridade e consultas de dados do sistema
- **Multiplataforma e sincronização de dados** - Permite acesso consistente em web mobile e desktop

## 2.3 Processos de gestão

- **Gestão de perfis de usuário e administrador** - Controle de permissões e papéis dentro da aplicação
- **Gestão de desempenho e disponibilidade** - Tempo de resposta e disponibilidade do sistema
- **Gestão de conformidade com a LGPD** - Garante privacidade e consentimento conforme a LGPD
- **Gestão de custos e automação operacional** - Reduz custos operacionais por meio de automação de processos
- **Gestão de qualidade e segurança** - Monitoramento de boas práticas, testes e confiabilidade do código

# 3 REQUISITOS DO SISTEMA

Serão apresentados os principais requisitos do sistema, divididos em requisitos de negócio, usuário, sistema, funcionais e não funcionais. Essa classificação permite organizar de forma clara o que o sistema deve realizar e as características técnicas que ele deve atender.

## 3.1 Requisitos de negócio

Os requisitos de negócio foram levantados com intuito de identificar claramente o problema a ser resolvido, as metas a serem alcançadas e os benefícios esperados com a solução proposta do nosso sistema de suporte de chamado

- **RN01 - Reduzir o tempo de triagem de chamados via IA**  
  Comprovação: Triagem manual de chamados é demorada

- **RN02 - Aumentar a produtividade da equipe técnica**  
  Comprovação: Apoiar a equipe técnica na tomada de decisões, destacando automaticamente os chamados mais críticos.

- **RN03 - Melhorar a experiência do usuário final**  
  Comprovação: Pouca visibilidade para o usuário final sobre o status dos chamados e garantir acompanhamento em tempo real.

- **RN04 - Reduzir custos operacionais através da automação**  
  Comprovação: Reduzir o trabalho manual da equipe de suporte, automatizando tarefas.

- **RN05 - Garantir conformidade com a LGPD**  
  Comprovação: O sistema será em conformidade com a LGPD garantindo privacidade e rastreabilidade dos dados.

## 3.2 Requisitos de usuário

São formulados em linguagem natural e acessível, focando na experiência do usuário e nas funcionalidades que ele espera encontrar no sistema.

### Usuário comum

- **RU01 - Realizar cadastro no sistema**  
  Comprovação: Verificação de dados persistidos no banco de dados (usuário cadastrado com sucesso e senha armazenada com hash).

- **RU02 - Realizar login**  
  Comprovação: Autenticação com JWT.

- **RU03 - Fazer Chamados**  
  Comprovação: Abertura de chamados: Usuários comuns podem registrar chamados por meio de texto.

- **RU04 - Visualizar seus chamados e resposta**  
  Comprovação: Visualização dos chamados pelo usuário para acompanhar o status, detalhes e consultar respostas.

- **RU05 - Gerenciamento de conta**  
  Comprovação: Área para gerenciamento da sua própria conta.

- **RU06 - Atualização de dados (sub - requisito de RU05)**  
  Comprovação: Permitir que o usuário edite suas informações pessoais e salve.

- **RU07 - Excluir conta (sub - requisito de RU05)**  
  Comprovação: Permitir que o usuário exclua sua conta a qualquer momento.

### Administrador

- **RU08 (A) – Cadastrar novos administradores**  
  Comprovação: Apenas administradores já autenticados podem criar novos administradores.

- **RU09 (A) - Visualizar todos os chamados registrados**  
  Comprovação: Listar de todos os chamados dos usuários do sistema.

- **RU010 (A) - Responder chamados**  
  Comprovação: Administradores podem selecionar um chamado e adicionar uma resposta.

- **RU11 (A) - Gerenciamento de conta**  
  Comprovação: Área para gerenciamento da sua própria conta.

- **RU12 (A) - Atualização de dados (sub - requisito de RU11)**  
  Comprovação: Permitir que o usuário edite suas informações pessoais e salve.

- **RU13 (A) - Excluir conta (sub - requisito de RU11)**  
  Comprovação: Permitir que o usuário exclua sua conta a qualquer momento.

## 3.3 Requisitos de sistemas

Os requisitos de sistema do nosso projeto definem o conjunto de funcionalidades e restrições necessárias para que a aplicação atenda aos seus objetivos de maneira alinhada às necessidades dos usuários.

### 3.3.1 Diagrama de domínio

Diagrama de modelagem de domínio que representa os relacionamentos dentro do sistema por completo.

<img width="826" height="413" alt="image" src="https://github.com/user-attachments/assets/296f6b8d-26af-436c-8fd0-ddfec556e1cc" />

### 3.3.2 Diagrama de caso de uso

Diagrama de caso de uso que demonstra as principais interações entre os atores e as funcionalidades de todo o sistema.

<img width="895" height="540" alt="image" src="https://github.com/user-attachments/assets/203c4510-b617-43fc-878b-642258ca73fa" />

### 3.3.4 Diagrama de classe

Diagrama de classes que representa todas as entidades do sistema, seus atributos, métodos e os relacionamentos entre elas.

<img width="1247" height="600" alt="image" src="https://github.com/user-attachments/assets/8efbd6b1-1c90-4a49-ad0b-57566d7628ea" />

### 3.3.5 Diagrama de entidade e relacionamento

Diagrama entidade-relacionamento, que representa a estrutura lógica do banco de dados, mostrando como as entidades se conectam por meio de seus relacionamentos e chaves.

<img width="1261" height="458" alt="image" src="https://github.com/user-attachments/assets/81a32d9e-fcf1-4210-b13c-4e97135c053d" />

### 3.3.6 Lista de requisitos

Serão descritos os comportamentos dos requisitos de sistema, alguns incluem diagramas de sequência e atividade para mostrar os fluxos e interações

- **RS01 - Cadastro de administradores e usuários**  
  Dado que o sistema possui um endpoint único para cadastro de usuários/admin, quando uma requisição é enviada para essa rota, então o sistema deve verificar o contexto de autenticação. Se o usuário estiver autenticado e possuir a claim de administrador no token JWT, o sistema deve permitir o cadastro de um novo administrador; caso o usuário não esteja autenticado, o sistema deve permitir o cadastro de um novo usuário comum.

  Diagrama de atividade da rota de cadastro:

  <img width="1066" height="670" alt="image" src="https://github.com/user-attachments/assets/5fb55669-a414-4dd5-bcff-eb7094170185" />

  Diagrama de sequêmcia da rota de cadastro:

  <img width="735" height="747" alt="image" src="https://github.com/user-attachments/assets/5f5735c0-0302-424b-84d5-de7ae42a5316" />


- **RS02 - Login com autenticação JWT**  
  Dado que o usuário ou administrador realiza login, quando enviar credenciais válidas, então o sistema deverá autenticar e gerar um token JWT assinado com HmacSha512 e válido por 24 horas, contendo os dados de id, nome, e-mail e cargo.

  Notas: O sistema não manterá sessões em memória ou em banco, a autenticação será stateless.

   Diagrama de atividade da rota de login:

   <img width="1273" height="408" alt="image" src="https://github.com/user-attachments/assets/3ef8e45d-ee6d-452f-8ba9-bad92dc8da15" />

   Diagrama de sequêmcia da rota de login:

   <img width="1206" height="637" alt="image" src="https://github.com/user-attachments/assets/24761f0f-b5af-4ad7-b3c3-42f9a78627fa" />

 - **RS03 - Criação de chamados com prioridade automática e status inicial**  
  Dado que um usuário cria um chamado no sistema, quando o chamado for enviado para registro, então o sistema deve verificar se há conexão ativa com a internet. Se a conexão estiver disponível, o sistema permitirá a criação do chamado, e irá registrar com prioridade atribuída automaticamente pela IA e status inicial como “Pendente”.

 - **RS04 - Categorização de prioridade automática com IA**  
  Dado que um chamado está sendo criado, então o sistema deve definir a prioridade automaticamente durante a criação através da integração com a API externa da OpenAI para fazer a categorização usando uma lógica de classificação por meio de um prompt.

 - **RS05 - Fallback de categorização via IA**  
  Dado que a categorização automática do chamado falhe, retorne erro, a descrição seja irregular, incompreensível ou fora do escopo da área de TI, ou falta de conexão com a API externa da IA, então o sistema deve atribuir automaticamente a prioridade “Analisando” ao chamado. Garantindo que o fluxo de criação do chamado nunca seja interrompido por falhas externas.
Os diagramas a seguir representaram juntos os requisitos: RS03 RS04 RS05

   Diagrama de atividade da rota de fazer chamado:

   <img width="1118" height="607" alt="image" src="https://github.com/user-attachments/assets/c3e420b6-4bac-4b2a-87f3-a13edf9eebff" />

   Diagrama de sequêmcia da rota de fazer chamado:

   <img width="547" height="602" alt="image" src="https://github.com/user-attachments/assets/a712370a-d1b8-413a-8b25-40633cb5a7db" />

 - **RS06 - Exibição dos Chamados do usuário**  
  Dado que o usuário esteja autenticado no sistema, quando clicar na opção “Ver meus chamados”, então o sistema deverá exibir todos os chamados associados ao seu id, obtido a partir das informações contidas nas claims do token JWT. O backend utilizará o id presente no token para buscar no banco de dados apenas os chamados pertencentes ao usuário logado e retornará essas informações.

   Diagrama de atividade da rota meus chamados:

   <img width="1527" height="503" alt="image" src="https://github.com/user-attachments/assets/2d2477f7-bcd6-4c68-b3b0-ad9efa3ea9f0" />

   Diagrama de sequêmcia da rota meus chamados:

   <img width="1038" height="670" alt="image" src="https://github.com/user-attachments/assets/3456ebd2-e374-45cf-aca6-3c659568983a" />

  - **RS07 - Exibição de Todos os Chamados para o Administrador**  
  Dado que o administrador esteja autenticado no sistema, quando clicar na opção “Ver chamados”, então o sistema deverá exibir todos os chamados registrados no sistema. O backend validará as claims do token JWT para confirmar o perfil de administrador e em seguida, retornará a lista completa de chamados. O front-end será responsável por exibir todos os chamados e
21
disponibilizar filtros que permitam ao administrador visualizar separadamente os chamados pendentes e os chamados respondidos.

    Diagrama de atividade da mostrar todos os chamados:

    <img width="1352" height="526" alt="image" src="https://github.com/user-attachments/assets/007bb0b2-2bab-48af-97cd-cab2dda6d3ee" />

    Diagrama de sequêmcia da rota de mostrar todos os chamados:

    <img width="1022" height="662" alt="image" src="https://github.com/user-attachments/assets/4e7a5459-54c0-4316-bd47-9b79c949fdcd" />

 - **RS08 - Administradores Respondem Chamados**  
 Dado que o administrador esteja autenticado no sistema, quando selecionar um chamado e enviar uma resposta, o sistema deverá registrar essa resposta e atualizar o status do chamado para “Respondido”. O backend validará as claims do token JWT para confirmar o perfil de administrador e permitirá a execução da ação. Ao receber a requisição com o identificador do chamado e o conteúdo da resposta, o sistema buscará o chamado correspondente no banco de dados, verificará sua existência e caso encontrado, executará validações sobre a resposta enviada, garantindo que o texto possua entre 5 e 500 caracteres e não esteja vazio, além de verificar se o chamado ainda não foi respondido anteriormente.

   Diagrama de atividade da rota responder chamado:
 
   <img width="1427" height="678" alt="image" src="https://github.com/user-attachments/assets/4b48541c-0a55-4629-b75e-ceeb2c57c018" />

   Diagrama de sequêmcia da rota responder chamado:

   <img width="873" height="771" alt="image" src="https://github.com/user-attachments/assets/b240f42e-52fe-468c-afec-9859fc93e1c2" />

- **RS09 - Exclusão de conta**  
 Dado que o usuário/admin esteja autenticado no sistema, quando acessar a opção “Excluir conta” e confirmar a solicitação, o sistema deverá validar sua identidade e permitir a exclusão apenas da própria conta. O backend verificará o id do usuário a partir do token JWT e bloqueará qualquer tentativa de exclusão de contas de outros usuários. Ao receber a requisição, o sistema buscará o registro correspondente no banco de dados, validará sua existência e removerá permanentemente os dados do usuário

   Diagrama de atividade da rota excluir usuário:
 
   <img width="1380" height="546" alt="image" src="https://github.com/user-attachments/assets/894cda7d-16f2-423f-8c3c-7a873f92c7da" />

   Diagrama de sequêmcia da rota de excluir usuário:

   <img width="902" height="767" alt="image" src="https://github.com/user-attachments/assets/0b7f8903-e2e3-4063-827a-0e9914967c10" />

- **RS10 - Edição de Dados**  
  Dado que o usuário esteja autenticado no sistema, quando acessar a opção “Gerenciar conta” e enviar uma solicitação para atualizar seus dados, o sistema deverá validar sua identidade e permitir alterações apenas nas próprias informações. O backend verificará o id do usuário a partir do token JWT e bloqueará tentativas de modificar dados de outros usuários. Ao processar a requisição, o sistema validará a existência do usuário e verificará se o nome e e-mail possuem formato válido e não estão duplicados, além de exigir a senha atual para alteração de senha, garantindo que a nova possua entre 5 e 10 caracteres e coincida com a confirmação. Após as validações, o sistema atualizará os dados com sucesso.

   Diagrama de atividade de editar dados:
 
   <img width="1571" height="582" alt="image" src="https://github.com/user-attachments/assets/def9ecbf-d88b-4f56-9a54-4524f5587a2e" />

   Diagrama de sequêmcia da rota de editar dados:

   <img width="452" height="747" alt="image" src="https://github.com/user-attachments/assets/77b4e0ec-01a6-4d32-aa67-269252dee462" />

- **RS11 - Exclusão de usuários vinculados ao chamado**  
  Dado que um usuário seja excluído do sistema, quando a exclusão for confirmada, então todos os chamados vinculados a esse usuário deverão ser removidos automaticamente, garantindo a integridade dos dados. Essa remoção ocorrerá por meio da configuração do relacionamento entre as entidades Usuário e Chamado no Entity Framework Core, utilizando a opção OnDelete(DeleteBehavior.Cascade), que assegura que, ao excluir um usuário, todos os chamados associados a ele sejam automaticamente deletados do banco de dados.

- **RS12 - Exibição de Dados da Conta via Token (Claims)**  
  Dado que o usuário ou administrador esteja autenticado, quando acessar a tela de “Gerenciar conta”, o sistema deverá exibir suas informações pessoais utilizando os dados contidos nas claims do token JWT, sem necessidade de nova consulta ao banco de dados. O token armazenará informações como id, nome e e-mail, que serão decodificadas pelo front-end para exibir os dados básicos da conta.

- **RS13 - API seguindo o estilo arquitetural REST**  
  Dado que a API será consumida por diferentes aplicações, quando novos endpoints forem desenvolvidos ou atualizados, então eles deverão seguir o estilo arquitetural REST, garantindo padronização, simplicidade e compatibilidade entre os sistemas. A API será implementada com rotas baseadas em recursos, utilizando métodos HTTP adequados e retornando dados em formato JSON acompanhados de códigos de status padronizados e a comunicação será stateless.

- **RS14 - Suporte Multiplataforma apenas para usuário comum**  
  Dado que o usuário utilize diferentes plataformas, quando interagir com o sistema, então os dados deverão ser consistentes, sincronizados e oferecer as mesmas funcionalidades em todos os ambientes. As aplicações cliente serão integradas à API, garantindo uniformidade na comunicação e nos dados. Cada plataforma utilizará tecnologias compatíveis com seu ambiente: Web com JavaScript, HTML, CSS e Bootstrap; Mobile com React Native; e Desktop com WPF.

- **RS15 - Banco de dados SQL server**  
  Dado que os dados precisam ser armazenados de forma estruturada e segura, quando ocorrer qualquer operação de leitura ou escrita, então essas informações deverão ser persistidas em um banco de dados. O sistema utilizará o SQL Server como gerenciador de banco de dados relacional.

- **RS16 - Respostas padronizadas da API**  
  Dado que a API precise responder às requisições realizadas pelos clientes, quando uma operação for concluída com sucesso, então a resposta deverá seguir um formato padrão contendo o status, a mensagem e os dados retornados, para facilitar o consumo pelo front-end. Esse padrão será aplicado automaticamente em todas as rotas da API, utilizando uma estrutura unificada e reutilizável que encapsulará as informações de sucesso e manterá consistência em todas as respostas

- **RS17 - Tratamento padronizado de erros da API**  
  Dado que podem ocorrer erros durante a execução das requisições, quando uma exceção controlada ou inesperada for lançada, então a API deverá retornar uma resposta padronizada contendo o status da operação, a mensagem de erro e o código HTTP apropriado. O tratamento de erros será centralizado em um ponto único da aplicação, responsável por capturar e interpretar exceções de diferentes camadas e convertê-las em uma estrutura padronizada, mantendo o mesmo formato das respostas de sucesso, com os campos status, mensagem e dados.

- **RS18 - Entitiy Frameworck Core**  
  Dado que o acesso ao banco de dados deve ser eficiente, seguro e de fácil manutenção, quando a API realizar operações de persistência, então isso deverá ser feito de forma desacoplada e manutenível através do uso do Entity Framework Core. Esse framework permitirá o mapeamento entre as entidades do sistema e o banco de dados, simplificando a manutenção do código, promovendo a reutilização da lógica de acesso a dados e possibilitando o controle de versionamento por meio de migrações.

- **RS19 - Armazenamento seguro de senhas com hash e salt**  
  Dado que o sistema deve proteger as credenciais dos usuários, quando for necessário armazenar ou validar uma senha, então isso deve ser feito de forma segura, sem salvar a senha em texto claro no banco de dados, utilizando técnicas de criptografia adequada. Será adotado o algoritmo HMACSHA512 para gerar o hash da senha com um salt exclusivo por usuário, armazenando ambos no banco. Durante a autenticação, o sistema recalculará o hash e o comparará com o armazenado.

- **RS20 - Uso de enumeradores para Cargo, Prioridade e Status**  
  Dado que o sistema deve manter consistência nos valores de cargos de usuários, prioridades de chamados e status, quando esses dados forem criados ou atualizados, então deverão ser utilizados enumeradores (enums) para garantir que apenas valores válidos sejam aceitos. Serão definidos enumeradores para cada tipo: Cargo, com os valores “Usuário” e “Administrador”, Prioridade, com “Baixa”, “Média”, “Alta” e “Analisando” e Status, com “Pendente” e “Respondido.

- **RS21 - Arquitetura da API em camadas**  
  Dado que o sistema deve ser modular, escalável e de fácil manutenção, quando novas funcionalidades forem adicionadas ou alteradas, então a estrutura da API deverá seguir o padrão de arquitetura em camadas desacopladas, baseada no modelo de três camadas. A solução será organizada em projetos separados: API, responsável por expor os endpoints e consumir os serviços, Application que conterá a lógica de aplicação, Infrastructure responsável pela persistência e integração externa e Domain que terá as entidades e regras de negócio. Também haverá um projeto de testes unitários utilizando xUnit

- **RS22 - Arquitetura do front-end**  
  Dado que o sistema terá múltiplas aplicações front-end (web, mobile e desktop) que consomem a mesma API, quando novas funcionalidades ou telas forem implementadas, então a arquitetura deverá permitir organização modular, reutilização de componentes e manutenção facilitada. As aplicações web e desktop adotarão o padrão MVC (Model-View-Controller), enquanto a aplicação mobile seguirá o padrão MVVM (Model-View-ViewModel)

- **RS23 - Acesso de administradores apenas na aplicação web**  
  Dado que o usuário possua perfil de administrador e faça login, quando acessar o sistema, então ele deverá ter acesso apenas às funcionalidades administrativas disponíveis na aplicação web, sendo bloqueado o acesso a essas funções em plataformas mobile ou desktop. Todas as funcionalidades administrativas serão implementadas exclusivamente na versão web.

## 3.4 Requisitos funcionais

Os requisitos funcionais do sistema descrevem as ações que os usuários podem realizar e os comportamentos esperados da aplicação em resposta a essas ações.

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


      "prioridade": "number",
      "nome": "string"
    }
  ],Entrada: Nenhuma

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

POST /suporte/v1/usuario

Descrição:
Registra um novo usuário no sistema.
O backend decide automaticamente se o cadastro será de Usuário ou Administrador, conforme a permissão do usuário que está realizando a requisição.

Se a requisição for anônima:
➜ O usuário será cadastrado como Usuário.

Se quem está fazendo a requisição estiver autenticado como Administrador:
➜ O usuário será cadastrado como Administrador.

Permissão:

Público: Pode criar usuário comum
Administrador: Pode criar administrador

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
    "email": "string",
    "cargo": "string (Usuario ou Administrador)"
  },
  "mensagem": "Usuário cadastrado com sucesso!",
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


