# Gerenciador de Funcionários

Bem-vindo ao **Gerenciador de Funcionários**, um sistema completo para cadastro, edição, remoção e gestão de funcionários de uma empresa. A aplicação conta com controle de permissões, autenticação e interface intuitiva.

## 🚀 Tecnologias Utilizadas

O projeto utiliza as seguintes tecnologias e frameworks:

- **Backend:**
  - ASP.NET Core 8
  - Entity Framework Core (EF Core)
  - SQL Server
  - Swagger para documentação da API
  - JWT (JSON Web Token) para autenticação
  - Serilog para logs estruturados

- **Frontend:**
  - Angular 17
  - TypeScript
  - Angular Material
  - RxJS para manipulação de streams de dados
  - SCSS para estilos

- **Infraestrutura:**
  - Docker e Docker Compose para gerenciamento de containers
  - Banco de dados rodando em um container SQL Server

---

## ✅ Funcionalidades

- 👨‍💼 **Cadastro de Funcionários** com diferentes níveis de permissão (Admin e Usuário)
- 📊 **Listagem e Detalhamento** dos funcionários
- 🔧 **Edição** de informações
- ❌ **Exclusão** de funcionários (somente Admin pode excluir)
- 🔑 **Autenticação segura** com JWT
- 🏢 **Gestor**: Cada funcionário pode ter um gestor designado
- ♻️ **Integração com Docker** para execução simplificada

---

## 🛠️ Como Rodar o Projeto

### 📚 1. Clonar o Repositório

```bash
 git clone https://github.com/pedroSouzaJunior/GerenciadorDeFuncionario.git
 cd GerenciadorDeFuncionario
```

### 📂 2. Fazer Checkout na Branch Principal

```bash
git checkout master
```

### ⚙️ 3. Rodar o Docker

```bash
docker compose up --build
```

> **Observação**: O primeiro build pode levar alguns minutos, pois todas as dependências serão baixadas.

### 🚀 4. Acessar a Aplicação

A interface web estará disponível em:

```
http://localhost:4200
```

A API estará rodando em:

```
http://localhost:5000/api
```

---

## 🔐 Credenciais de Login

Para acessar a aplicação, utilize um dos seguintes usuários padrão:

### 👨‍💼 Administrador

```json
{
  "email": "admin@email.com",
  "senha": "admin123"
}
```

> **Permissões**: Pode criar, editar, visualizar e excluir funcionários.

### 👤 Usuário Comum

```json
{
  "email": "user@email.com",
  "senha": "user123"
}
```

> **Permissões**: Pode criar, editar e visualizar funcionários, mas **não pode excluir**.

---

## 🛠️ API Endpoints

### 🔓 Autenticação

- `POST /api/auth/login` - Realiza login e retorna o token JWT

### 👨‍💼 Funcionários

- `GET /api/funcionarios` - Lista todos os funcionários
- `GET /api/funcionarios/{id}` - Obtém um funcionário pelo ID
- `POST /api/funcionarios` - Cria um novo funcionário (Apenas Admin e Usuário)
- `PUT /api/funcionarios/{id}` - Edita um funcionário existente
- `DELETE /api/funcionarios/{id}` - Remove um funcionário (Apenas Admin)

---

## 🔧 Melhorias Futuras

O projeto está funcional, mas há pontos de melhoria que podem ser implementados no futuro:

### 💄 **Melhoria no Layout**
- Melhorar responsividade para diferentes telas
- Ajustar espaçamentos e UI no Angular Material
- Melhor experiência do usuário nos formulários

### 🚨 **Tratamento de Exceções no Frontend**
- Melhor exibição de mensagens de erro na UI
- Feedback mais claro quando uma ação falha

### 🧪 **Testes**
- **Testes unitários no Frontend** (Jasmine + Karma)
- **Testes de integração** para API

### 🚀 **Cache para Melhorar Performance**
- Implementar cache na API para evitar chamadas redundantes ao banco
- Uso do `Redis` como cache para melhorar desempenho

### 📜 **Melhoria nos Logs**
- Ajustar logs do backend para serem mais informativos
- Adicionar logs estruturados no frontend (exemplo: `ngx-logger`)


---

## 🌟 Agradecimentos

Obrigado por testar o **Gerenciador de Funcionários**! Caso tenha alguma dúvida ou sugestão, fique à vontade para contribuir. ✨

