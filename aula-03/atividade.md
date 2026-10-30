## Contexto do Problema
Na "Pousada Vista Bela", os chalés contam com itens e produtos para consumo dos hóspedes (ex.: garrafa de água mineral, refrigerante, pacote de biscoitos, toalha extra).
Para que a recepção consiga listar, consultar e cadastrar esses itens, precisamos desenvolver a Model representativa de `Item` e um controlador com persistência em memória.

---

## Requisitos da Atividade

1. Criação da Model `Item` (`Models/Item.cs`):
   Crie a classe com as propriedades:
   - `Id` (int) - Identificador único
   - `Nome` (string) - Nome descritivo do item
   - `Preco` (decimal) - Preço unitário cobrado
   - `Descricao` (string) - Detalhes do item

2. Criação do Controlador `ItensController` (`Controllers/ItensController.cs`):
   - Adicione os atributos `[ApiController]` e `[Route("api/[controller]")]`.
   - Crie uma lista estática privada em memória (`private static readonly List<Item> _itens`) contendo previamente 3 itens cadastrados:
     - ID 1: "Agua Mineral 500ml", Preço: 5.00, Descrição: "Agua sem gas"
     - ID 2: "Refrigerante Lata", Preço: 8.50, Descrição: "Lata 350ml"
     - ID 3: "Pacote de Biscoito Artesanal", Preço: 12.00, Descrição: "Biscoito amanteigado caseiro"

3. Implementação dos Endpoints:
   - Endpoint 1 (Listagem Geral):
     - Método: `GET /api/itens`
     - Retorno: Resposta HTTP 200 OK contendo a lista completa de itens cadastrados.
   - Endpoint 2 (Busca por Identificador):
     - Método: `GET /api/itens/{id}`
     - Regra: Localizar o item na lista pelo ID.
     - Retorno:
       - Se encontrado: HTTP 200 OK com o item localizado.
       - Se não encontrado: HTTP 404 NotFound com a mensagem `"Item nao localizado."`.
   - Endpoint 3 (Cadastro de Novo Item):
     - Método: `POST /api/itens`
     - Parâmetro: `[FromBody] Item novoItem`
     - Validação Básica:
       - O nome não pode estar em branco.
       - O preço deve ser maior que zero.
       - Se inválido, responder com HTTP 400 BadRequest informando o erro.
     - Lógica de Incremento: Calcular o novo ID autoincremental baseado no maior ID existente.
     - Retorno de Sucesso: HTTP 201 Created apontando para a rota de busca por ID (`CreatedAtAction`).

4. Validação e Teste:
   - Execute com `dotnet run` e abra o Swagger.
   - Teste o `GET /api/itens` para checar os dados iniciais.
   - Teste o `POST /api/itens` cadastrando um novo produto (ex.: "Chocolate Barra").
   - Teste o `GET /api/itens/{id}` buscando pelo ID do item recém-adicionado e por um ID inexistente (ex.: 99).
   - Deixe o projeto pronto para a correção das 22:00.
