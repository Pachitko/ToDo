<h3 align="center">Login Page</h3>
<p align="center"><img src="https://user-images.githubusercontent.com/38788505/182655784-09f3d373-d628-4267-a417-4822f8e53e25.png"></p>

<h3 align="center">Main Page</h3>
<p align="center"><img src="https://user-images.githubusercontent.com/38788505/182656367-fad1dc1a-798c-44ce-a2aa-31f7cfa7ee3b.png"></p>

<h1 align="center">Fullstack To Do App</h1>

# Functionality:
- Login/registration
- CRUD operations with task and task lists
- Task due date; task recurrence; searching; adopted
- Responsive web design

# Technologies, tools, principles and specifications:
Backend:
- ASP.NET Core
- PostgreSQL
- CQRS

Frontend:
- TypeScript
- HTML/CSS
- WebPack
- NPM

OpenAPI specification
REST api

# Project structure:
## Layers:
- Core.Domain: entities
- Core.DomainServices: IApplicationDbContext instead of Repository and UnitOfWork patterns
- Core.Application - service abstractions and MediatR
- Infrastructure - DB, service implementations
- ToDoApi - REST api + ClientApp (React JS)
- ~~Tests not implemented~~
- ~~IdentityServer not implemented~~

## Backend libraries
- MediatR (CQRS implementation)
- Entity Framework Core + Identity
- Authentication.JwtBearer
- Swashbuckle (OpenAPI)
- FluentValidation
- AutoMapper
- StackExchangeRedis

## Frontend libraries:
- React JS
- react-router-dom
- redux
- axios (to REST API usage)
- styled-components (CSS in TS)
- react-google-login (Google OAuth 2.0 component for React)

# Installation (Docker)
Docker & Docker Compose v2.0+ must be installed

Tested System: Ubuntu 20.04/Windows 11

# How to use
