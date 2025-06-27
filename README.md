
# Desafio Técnico IngaCode  🚀

Sistema para controle de tarefas com apontamento de horas.

# Funcionalidades Principais
 
 #### Gerenciamento de Projetos 
 * Listar, criar, editar e excluir projetos.

#### Gerenciamento de Tarefas
* Listar, criar, editar e excluir tarefas.
* Associação de tarefas a projetos.

#### Time Tracking
 * Iniciar e parar apontamentos de tempo por tarefa e colaborador.

* Visualização de tempo gasto no dia e no mês (por colaborador).

* Validação de colisão de intervalos e limite de 24h/dia.

#### Colaboradores

* Geração de colaboradores a partir de usuários existentes.

#### Autenticação e Segurança

* JWT para autenticação de API.

* Senhas armazenadas com hash BCrypt.

# Tecnologias Utilizadas

## Backend
- **.NET 9 (C#)**

- **Entity Framework Core (Code First + Migrations)**

- **PostgreSQL**

- **AutoMapper**

- **Swashbuckle (Swagger)**

## Frontend

- **Vue 3 + TypeScript**

- **Vuetify 3**

- **Vite**

- **Axios**

### Infraestrutura

- **Docker & Docker Compose**

- **Nginx (servidor front-end)**
## 🐳 Como Executar o Projeto com Docker

### Requisitos:
- Docker e Docker Compose instalados

### Etapas:

1. Clone o repositório:
   ```bash
   git clone https://github.com/VictorF3671/ProjectSGTAH.git
   cd .\ProjectSGTAH\

2. Suba os containers:
   ```bash
   docker-compose up --build

3. Acesse

   Backend: http://localhost:8000/swagger

   Frontend: http://localhost:8080/login



## 🔑 Credenciais Padrão (Superusuário)
   ```bash
   Usuário: admin
   Senha: senha123
   ```

