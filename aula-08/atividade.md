# Atividade Prática 08: CRUD Síncrono Completo com Entity Framework Core

## Contexto do Problema
O recepcionista da Pousada Vista Bela já consulta e cadastra itens de consumo salvando-os diretamente no banco de dados. Agora, precisamos permitir a atualização cadastral (ex: correção de preços) e a exclusão definitiva de itens descontinuados diretamente no banco de dados relacional.

---

## Requisitos da Atividade

No arquivo `Controllers/ItensController.cs` do projeto `Hospedagem.API`:

1. Implementar o Endpoint de Atualização no Banco (PUT):
   - Rota: `PUT /api/itens/{id}`
   - Parâmetros: `id` (int) na rota e `[FromBody] Item itemAtualizado` no corpo.
   - Validações:
     - Se `itemAtualizado` for nulo: HTTP 400 BadRequest.
     - Se `id != itemAtualizado.Id`: HTTP 400 BadRequest (`"O ID da rota difere do ID do objeto informado."`).
     - Localizar o item no banco com `_context.Itens.Find(id)`. Se for nulo: HTTP 404 NotFound.
     - Validar se `Nome` não está vazio e `Preco > 0`. Se inválido: HTTP 400 BadRequest.
   - Ação: Atualizar as propriedades do item encontrado no banco (`Nome`, `Preco`, `Descricao`).
   - Persistência: Invocar `_context.SaveChanges()`.
   - Retorno de Sucesso: HTTP 204 NoContent.

2. Implementar o Endpoint de Exclusão no Banco (DELETE):
   - Rota: `DELETE /api/itens/{id}`
   - Parâmetro: `id` (int) na rota.
   - Validações:
     - Localizar o item no banco com `_context.Itens.Find(id)`. Se for nulo: HTTP 404 NotFound.
   - Ação: Remover o registro através de `_context.Itens.Remove(itemLocalizado)`.
   - Persistência: Invocar `_context.SaveChanges()`.
   - Retorno de Sucesso: HTTP 204 NoContent.

3. Validação e Testes no Swagger e no SGBD:
   - Abra o Swagger e altere o preço de um dos itens salvos na aula anterior.
   - Verifique se a resposta é HTTP 204.
   - Abra o SSMS ou SQLite e execute `SELECT * FROM Itens;` confirmando a alteração no banco.
   - Exclua um dos itens e valide a remoção física no banco de dados.
   - Tente excluir um ID inexistente e confirme a resposta 404 NotFound.
   - Deixe o código pronto para a correção coletiva das 22:00.
