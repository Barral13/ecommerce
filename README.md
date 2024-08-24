# Projeto de E-commerce

Este projeto é uma API para um sistema de e-commerce, fornecendo endpoints para gerenciar produtos e categorias, além de operações de login e registro de usuários.

## Endpoints da API

### Categoria

- **GET /v1/categories**
  - Obtém a lista de todas as categorias.
  
- **POST /v1/categories**
  - Cria uma nova categoria.
  
- **GET /v1/categories/{id}**
  - Obtém os detalhes de uma categoria específica.
  
- **PUT /v1/categories/{id}**
  - Atualiza os detalhes de uma categoria específica.
  
- **DELETE /v1/categories/{id}**
  - Remove uma categoria específica.

### Produto

- **GET /v1/products**
  - Obtém a lista de todos os produtos.
  
- **POST /v1/products**
  - Cria um novo produto.
  
- **GET /v1/products/{id}**
  - Obtém os detalhes de um produto específico.
  
- **PUT /v1/products/{id}**
  - Atualiza os detalhes de um produto específico.
  
- **DELETE /v1/products/{id}**
  - Remove um produto específico.
  
- **GET /v1/products/category/{category}**
  - Obtém a lista de produtos por categoria.

## Esquemas

Os seguintes View Models são utilizados para interações com a API:

- **EditorProductViewModel**
  - View Model para a criação e edição de produtos.
  
- **EditorViewModel**
  - View Model para a criação e edição de categorias.
  
- **LoginViewModel**
  - View Model para o login de usuários.
  
- **RegisterViewModel**
  - View Model para o registro de novos usuários.

## Requisitos

- **.NET 8**
- **Entity Framework Core**
- **SQLite**

## Configuração

1. Clone o repositório:
   ```bash
   git clone https://github.com/usuario/ecommerce.git
   ```

2. Navegue para o diretório do projeto:
   ```bash
   cd ecommerce
   ```

3. Restaure as dependências:
   ```bash
   dotnet restore
   ```

4. Crie o banco de dados e aplique as migrações:
   ```bash
   dotnet ef database update
   ```

5. Execute o projeto:
   ```bash
   dotnet run
   ```

## Uso

Acesse a API no endereço base `http://localhost:5262/v1/`.

Para testar os endpoints, você pode usar ferramentas como Postman ou cURL.

## Observações

- **Autorização**: As operações de criação, exclusão e atualização de produtos e categorias estão disponíveis apenas para usuários com o papel `admin`.
- **Swagger**: A documentação da API pode ser acessada e testada via Swagger, que fornece uma interface interativa para explorar os endpoints disponíveis.
- **Segurança dos Segredos**: As informações sensíveis, como strings de conexão, chaves JWT e chaves de API, são gerenciadas utilizando o `dotnet user-secrets` para garantir que não estejam diretamente no código-fonte. Configure os segredos com os seguintes comandos:
  
  ```bash
  dotnet user-secrets init
  dotnet user-secrets set "JwtKey" ""
  dotnet user-secrets set "ApiKeyName" ""
  dotnet user-secrets set "ApiKey" ""
  dotnet user-secrets set "ConnectionStrings:DefaultConnection" ""
  ```
