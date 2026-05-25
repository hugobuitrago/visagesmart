# VisageSmart

Estrutura inicial para um sistema de gestão de salões de beleza e barbearias com foco em baixo custo.

## Stack principal

- Backend: `.NET 10`, `ASP.NET Core Web API`, `Entity Framework Core`
- Banco de dados: `PostgreSQL` com Docker
- Frontend: `Vue 3`, `Vite`, `Tailwind CSS`, `Pinia`, `Vue Router`

## Estrutura

- `src/VisageSmart.Api`: API ASP.NET Core
- `src/VisageSmart.Application`: camada de aplicação
- `src/VisageSmart.Domain`: entidades e regras centrais
- `src/VisageSmart.Infrastructure`: persistência, autenticação e integrações
- `frontend`: Vue 3 + Vite + Tailwind + Pinia + Vue Router
- `docker-compose.yml`: banco PostgreSQL local

## Banco de dados local com Docker

1. Suba o PostgreSQL:
   `docker compose up -d`
2. A API já está configurada para usar:
   `Host=localhost;Port=5432;Database=visagesmart_db;Username=visagesmart;Password=visagesmart123`
3. Quando criarmos as entidades detalhadas, podemos gerar as migrations com EF Core.

## Por que PostgreSQL

- Gratuito e open source
- Excelente custo-benefício para projetos pequenos e médios
- Fácil de rodar localmente com Docker
- Mais próximo de um ambiente real de produção do que bancos locais embutidos

## Próximos passos

1. Criar a primeira migration do Entity Framework.
2. Detalhar os campos de cada módulo.
3. Conectar o frontend aos endpoints da API.
4. Adicionar seed inicial de usuários e permissões.
