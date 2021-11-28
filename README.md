# Autorizador
Sistema para autorização de transações de uma conta, que tem como principal objetivo permitir ou negar operações com base em uma série de regras de segurança
 
# Atividades
- [x] Implementar MVP da aplicação
- [x] Implementar testes unitários
- [x] Revisar o código
- [x] Documentar
- [x] Revisar documentação

#Decisões técnicas
##Desenho da solução
Para o desenho da solução foi utilizada uma simplificação do princípio de "Clean Architecture", de modo que:
1 - Os contratos com a definição de negócio, e os seus modelos, estão no núcleo da aplicação sem dependência de outras camadas;
2 - Acima de negócios encontramos a camada de serviço. Essa camada implementa todos os contratos definidos na camada inferior, e acessa os modelos de negócio necessários. Ela também define os contratos de toda infraestrutura necessária;
3 - E por último temos a camada de infraestrutura, que está mais externa as demais, e tem acesso a todas as outras implementações;

Todas as camadas tem implementação independentes, respeitando apenas os contratos exigidos nas camadas subjacentes.

## Frameworks utilizados
O projeto principal da aplição utiliza-se apenas das bibliotecas padrões de projetos em .NET Core 3.1, de modo a se manter simples e leve.
Contudo o projeto de testes faz uso de algumas bibliotecas comuns, tais como:
- Microsoft.NET.Test.Sdk;
- Moq;
- NUnit;
- NUnit3TestAdapter;

# Executando o projeto
## Requisitos
Para executar o projeto é necessário ter instalado no ambiente local as seguintes ferramentas:
- Microsoft .NETCore SDK versão 3.1+;
- Microsoft.NETCore.App runtime versão 3.1+;

## Executar os comandos no terminal:
- Baixar as dependências necessárias: dotnet restore
- Compilar o projeto: dotnet build
- Executar o projeto com acesso ao teminal para entrada de informações na aplicação: dotnet run --interactive



