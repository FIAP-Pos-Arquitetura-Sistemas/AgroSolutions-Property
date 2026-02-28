# AgroSolutions - Property

Serviço responsável pelo gerenciamento de propriedades rurais dentro da plataforma **AgroSolutions**.

Este microsserviço controla o cadastro e gestão das fazendas associadas aos usuários do sistema.

---

## Arquitetura

Este serviço segue os princípios de:

- Clean Architecture
- DDD (Domain-Driven Design)
- SOLID
- Separação por camadas:
  - API
  - Application
  - Domain
  - Infrastructure

---

## Tecnologias Utilizadas

- .NET 8
- ASP.NET Core Web API
- Entity Framework Core
- SQL Server
- Docker
- GitHub Actions (CI/CD)
- Swagger (OpenAPI)

---

## Responsabilidades do Serviço

- Cadastro de propriedades rurais
- Associação da propriedade a um usuário
- Atualização de dados da fazenda
- Consulta de propriedades por usuário
- Exclusão lógica (soft delete)

---

## Regras de Negócio

- Toda propriedade deve estar vinculada a um usuário válido
- Não permitir cadastro duplicado de propriedade para o mesmo usuário
- Validação de área mínima
- Controle de status da propriedade (Ativa/Inativa)

---

##  Como Executar Localmente

### 1 - Clonar repositório

- bash
  
git clone https://github.com/viniciusdelr/AgroSolutions-FarmService.git
cd AgroSolutions-FarmService

### Executando com Docker
docker build -t agrosolutions-farmservice .
docker run -p 8081:80 agrosolutions-farmservice

---

##  CI/CD

Este projeto utiliza GitHub Actions para:
- Build automático
- Criação de imagem Docker
- Publicação no Docker Hub

Pipeline localizado em:
.github/workflows/
