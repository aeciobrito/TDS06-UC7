# Atividade Prática 07: Conexão do Controller com o Banco de Dados via EF Core

## Instruções Gerais
- Projeto de Referência: Sistema de Hospedagem ("Pousada Vista Bela")

---

## Contexto do Problema
Agora que a tabela `Itens` foi criada no banco de dados da Pousada Vista Bela através das migrations, precisamos remover a lista estática em memória do `ItensController` e conectar o controlador diretamente ao banco de dados utilizando o `HospedagemDbContext`.

---

## Requisitos da Atividade

No arquivo `Controllers/ItensController.cs` do projeto `Hospedagem.API`:

1. Remover a Lista Estática:
   - Remova o campo `private static readonly List<Item> _itens = ...`.

2. Injetar o `HospedagemDbContext`:
   - Declare um campo privado `private readonly HospedagemDbContext _context;`.
   - Crie o construtor público `public ItensController(HospedagemDbContext context)` que inicializa o campo `_context`.

3. Refatorar os Endpoints de Leitura:
   - `GET /api/itens`: Consultar a tabela no banco usando `_context.Itens.ToList()` e retornar HTTP 200 OK.
   - `GET /api/itens/{id}`: Buscar o item no banco pela chave primária usando `_context.Itens.Find(id)`. Se não encontrar, retornar HTTP 404 NotFound. Se encontrar, retornar HTTP 200 OK com o objeto.

4. Refatorar o Endpoint de Cadastro (POST):
   - `POST /api/itens`:
     - Validar se `novoItem` não é nulo, se `Nome` não está em branco e se `Preco` é maior que zero.
     - Chamar `_context.Itens.Add(novoItem)`.
     - Chamar `_context.SaveChanges()` para efetivar a gravação no banco de dados.
     - Responder com HTTP 201 Created apontando para `ObterPorId`.

5. Testes no Swagger e no Banco de Dados:
   - Execute a API com `dotnet run`.
   - Abra o Swagger e consulte `GET /api/itens` (deve vir vazio inicialmente).
   - Cadastre pelo menos 3 itens reais de frigobar (ex: "Agua Mineral", "Suco de Uva", "Castanha de Caju") usando o `POST /api/itens`.
   - Abra o SGBD (SSMS ou SQLite) e execute `SELECT * FROM Itens;` comprovando que os dados foram persistidos.
   - Reinicie a API e faça um novo `GET` comprovando que os dados continuam lá.
   - Deixe o código pronto para a correção das 22:00.
