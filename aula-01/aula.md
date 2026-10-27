# Aula 01: Introdução ao Ecossistema .NET, Web APIs e Primeiro Endpoint

- Ambiente: Windows 11, .NET SDK 8.0+, VS Code ou Visual Studio 2022.
- Indicadores: IND 1 (Configuração de ambiente) e IND 2 (Melhores práticas de back-end).
- Projeto Central de Demonstração (Professor): ControleEstoque.
- Projeto de Atividade dos Alunos: Hospedagem.

---

## 1. Roteiro do Tempo (19:00 às 22:30)

| Horário | Etapa | Foco do Professor |
|---|---|---|
| 19:00 - 19:30 | Acolhimento e Revisão | Recepção da turma, tolerância de chegada (20 min), verificação do .NET SDK instalado na máquina (`dotnet --version`), alinhamento dos objetivos da UC7. |
| 19:30 - 20:00 | Exposição Prática | O que é uma Web API RESTful; arquitetura cliente-servidor; anatomia de um projeto ASP.NET Core; arquivo `Program.cs` e primeiro Controller. |
| 20:00 - 20:30 | Problema Guiado | Criação da Web API do ControleEstoque via CLI; implementação do `StatusController` com endpoint simples de texto e endpoint com recepção de parâmetro de nome. Execução e teste no navegador/Swagger. |
| 20:30 - 20:45 | Intervalo | Pausa de 15 minutos para descanso. |
| 20:45 - 22:00 | Laboratório "Mão na Massa" | Alunos criam do zero o projeto Web API da Hospedagem e implementam o `PousadaController` com endpoints de status e boas-vindas com parâmetros. |
| 22:00 - 22:30 | Correção Coletiva e Fechamento | Demonstração do gabarito em tela, validação de requisições via Swagger e alinhamento para a Aula 02. |

---

## 2. Fundamentação Teórica Direta ao Ponto

### O que é uma Web API e por que adotá-la?
Uma Web API (Application Programming Interface) é uma camada de software construída sobre o protocolo HTTP que permite que clientes distintos (aplicativos desktop em C#, páginas web, dispositivos móveis) comuniquem-se com a retaguarda corporativa utilizando mensagens padronizadas em texto (habitualmente JSON).

Em vez de conectar a aplicação desktop diretamente com uma porta aberta no servidor de banco de dados (o que expõe credenciais e gera vulnerabilidades críticas de rede), a aplicação cliente conversa com a Web API. A Web API é a única responsável por aplicar regras de negócio, validar dados e consultar o banco de dados.

### Anatomia de uma Web API em .NET
Ao criar um projeto com suporte a controladores no ASP.NET Core, os seguintes elementos são gerados:
1. `Program.cs`: Ponto de entrada (entry-point) da aplicação. Configura o servidor Kestrel, registra serviços no container de injeção de dependência (`builder.Services`) e define o pipeline de requisições HTTP (`app.UseRouting()`, `app.MapControllers()`).
2. `Controllers/`: Diretório que agrupa as classes responsáveis por processar requisições HTTP e devolver respostas.
3. `appsettings.json`: Arquivo de configuração de ambiente (portas, strings de conexão, configurações de log).

---

## 3. Demonstração Prática Passo a Passo (Projeto ControleEstoque)

### Passo 1: Verificação da Instalação do .NET SDK
No terminal (PowerShell), execute:
```bash
dotnet --version
```
A saída esperada deve ser a versão do .NET instalada (ex: `8.0.x`).

### Passo 2: Criação da Solução e da Web API via CLI
Execute na raiz da pasta de trabalho:
```bash
# Criar pasta do projeto de demonstração
mkdir ControleEstoque
cd ControleEstoque

# Criar a aplicação Web API com template de Controllers
dotnet new webapi -n ControleEstoque.API -controllers

# Entrar no diretório da API
cd ControleEstoque.API
```

### Passo 3: Limpeza de Arquivos de Exemplo
Remova os arquivos gerados por padrão pelo template para iniciar com código limpo:
- Remova `WeatherForecast.cs`
- Remova `Controllers/WeatherForecastController.cs`

### Passo 4: Criação do Primeiro Controller (`StatusController.cs`)
Crie o arquivo `Controllers/StatusController.cs`:

```csharp
using Microsoft.AspNetCore.Mvc;

namespace ControleEstoque.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StatusController : ControllerBase
    {
        // 1. Endpoint mais simples possível: retorna texto plano via GET
        // Rota final: GET /api/status
        [HttpGet]
        public string GetStatus()
        {
            return "API do Sistema de Controle de Estoque operando com sucesso!";
        }

        // 2. Endpoint recebendo parâmetro via rota (Route Parameter)
        // Rota final: GET /api/status/saudacao/{operador}
        // Exemplo: GET /api/status/saudacao/Carlos
        [HttpGet("saudacao/{operador}")]
        public string ObterSaudacaoRota(string operador)
        {
            return $"Olá, {operador}! Bem-vindo ao Sistema de Controle de Estoque.";
        }

        // 3. Endpoint recebendo parâmetro via Query String
        // Rota final: GET /api/status/consulta?modulo=Vendas
        [HttpGet("consulta")]
        public string ConsultarModulo([FromQuery] string modulo)
        {
            if (string.IsNullOrWhiteSpace(modulo))
            {
                return "Módulo geral de estoque selecionado.";
            }

            return $"Módulo consultado: {modulo}. Status: Ativo.";
        }
    }
}
```

### Passo 5: Execução e Teste no Terminal
Execute o comando:
```bash
dotnet run
```
O console exibirá as URLs de escuta (habitualmente `http://localhost:5000` ou similar).

Abra o navegador ou use o Swagger em `http://localhost:<porta>/swagger` para testar os endpoints:
- `http://localhost:<porta>/api/status`
- `http://localhost:<porta>/api/status/saudacao/Aecio`
- `http://localhost:<porta>/api/status/consulta?modulo=Almoxarifado`

---

## 4. Orientações para a Atividade dos Alunos (20:45 às 22:00)

1. Publique o arquivo `atividade.md` para a turma.
2. Instrua os alunos a criarem o projeto `Hospedagem.API` do zero via CLI.
3. Circule pela sala auxiliando na sintaxe C#, uso do terminal e conferência das rotas.
4. Pontualmente às 22:00, inicie a projeção do arquivo `solucao.md` para correção coletiva.
