
## Contexto do Problema
Você foi contratado para iniciar o desenvolvimento do backend da "Pousada Vista Bela". O objetivo deste primeiro módulo é criar uma Web API RESTful em ASP.NET Core que sirva como ponto de comunicação entre as estações de atendimento da recepção e o servidor central.

Nesta primeira etapa, não utilizaremos banco de dados. Construiremos os endpoints iniciais para validar que o servidor está online e responder a saudações personalizadas para hóspedes e recepcionistas.

---

## Requisitos da Atividade

1. Criação do Projeto:
   - Crie uma pasta chamada `Hospedagem`.
   - Dentro da pasta, crie uma Web API em .NET usando a CLI com suporte a controllers chamada `Hospedagem.API`.
   - Exclua os arquivos de template padrão (`WeatherForecast.cs` e `Controllers/WeatherForecastController.cs`).

2. Criação do Controlador `PousadaController`:
   - Dentro da pasta `Controllers/`, crie a classe `PousadaController.cs`.
   - A classe deve herdar de `ControllerBase` e conter os atributos `[ApiController]` e `[Route("api/[controller]")]`.

3. Endpoints Obrigatórios:
   - Endpoint 1 (Status da Pousada):
     - Método HTTP: `GET`
     - Rota: `/api/pousada`
     - Retorno: String com o texto `"Sistema de Gerenciamento da Pousada Vista Bela em operacao."`
   - Endpoint 2 (Boas-vindas ao Hóspede via Rota):
     - Método HTTP: `GET`
     - Rota: `/api/pousada/boas-vindas/{nomeHospede}`
     - Parâmetro: `nomeHospede` recebido diretamente na rota.
     - Retorno: String com o texto formatado: `"Seja muito bem-vindo(a) a Pousada Vista Bela, {nomeHospede}! Desejamos uma excelente estadia."`
   - Endpoint 3 (Consulta de Unidade via Query String):
     - Método HTTP: `GET`
     - Rota: `/api/pousada/unidade?chale={numeroChale}`
     - Parâmetro: `chale` (número inteiro ou texto) recebido via query string.
     - Retorno: Se o parâmetro for informado, retornar `"Unidade Chale {numeroChale} selecionada para atendimento."`. Caso contrário, retornar `"Nenhum chale informado. Recepcao central ativa."`

4. Validação e Teste:
   - Execute o projeto através do comando `dotnet run`.
   - Abra o navegador ou utilize o Swagger e valide o funcionamento dos 3 endpoints.
   - Deixe o código pronto para a correção coletiva às 22:00.