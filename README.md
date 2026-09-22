# Medica+ API

Back-end em ASP.NET Core 8 + Entity Framework Core (SQLite), construído a partir do
front-end estático do projeto `Projeto-UC3-main` (Medica+).

## Módulos

| Módulo | Rotas | Requer login |
|---|---|---|
| Autenticação | `POST /api/auth/cadastro`, `POST /api/auth/login` | Não |
| Perfil | `GET/PUT/DELETE /api/usuarios/me` | Sim |
| Contatos | `GET/POST/PUT/DELETE /api/contatos` | Sim |
| Pressão arterial | `GET/POST/DELETE /api/pressao` | Sim |
| Humor | `GET/POST/DELETE /api/humor` | Sim |
| Suporte / visitas | `GET/POST /api/visitas`, `GET /api/visitas/proxima`, `PUT /api/visitas/{id}/reagendar`, `DELETE /api/visitas/{id}` | Sim |
| Assinatura (checkout) | `GET /api/assinaturas`, `POST /api/assinaturas/checkout` | Sim |

Os endpoints protegidos usam **JWT**: após `login`/`cadastro` você recebe um `token`
que deve ser enviado em `Authorization: Bearer {token}`. O ID do usuário nunca é
lido da URL — é sempre extraído do token, então um usuário nunca acessa dados de outro.

## Como rodar

Pré-requisitos: [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0).

```bash
cd MedicaMaisApp.Backend

# 1. Restaurar pacotes (baixa EF Core, JWT, Swagger do NuGet)
dotnet restore

# 2. Instalar a ferramenta de migrations do EF Core (uma vez só, globalmente)
dotnet tool install --global dotnet-ef

# 3. Criar a primeira migration (gera as tabelas a partir dos modelos em Modelos/)
dotnet ef migrations add InitialCreate

# 4. Rodar a API — o banco medicamais.db (SQLite) é criado/atualizado automaticamente
#    no startup (veja Program.cs: contexto.Database.Migrate())
dotnet run
```

A API sobe em `https://localhost:7xxx` (a porta exata aparece no console) e o Swagger
fica em `/swagger`, com botão **Authorize** para colar o token JWT e testar rotas
protegidas direto pelo navegador.

O arquivo `MedicaMaisApp.Backend.http` tem exemplos prontos de todas as chamadas
(pode ser executado no VS Code com a extensão "REST Client" ou no Rider/Visual Studio).

## Antes de ir para produção

- **Troque a chave em `appsettings.json` → `Jwt:Chave`** por um valor longo e aleatório,
  e mova-a para variável de ambiente / `dotnet user-secrets` — nunca deixe uma chave
  JWT real commitada no repositório.
- Restrinja o CORS em `Program.cs` (`AllowAnyOrigin()`) ao domínio real do front-end.
- O checkout em `AssinaturaService` **simula** a aprovação do pagamento (igual ao
  `checkout.html` original, que sempre mostra "Pagamento aprovado!"). Para produção,
  integre um gateway real (Stripe, Mercado Pago, PagSeguro etc.) antes de marcar a
  assinatura como `Aprovado`.
