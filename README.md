# Autorizador
Sistema para autoriza��o de transa��es de uma conta, que tem como principal objetivo permitir ou negar opera��es com base em uma s�rie de regras de seguran�a
 
# Atividades
- [x] Implementar MVP da aplica��o
- [x] Implementar testes unit�rios
- [x] Revisar o c�digo
- [x] Documentar
- [x] Revisar documenta��o

#Decis�es t�cnicas
##Desenho da solu��o
Para o desenho da solu��o foi utilizada uma simplifica��o do princ�pio de "Clean Architecture", de modo que:
1 - Os contratos com a defini��o de neg�cio, e os seus modelos, est�o no n�cleo da aplica��o sem depend�ncia de outras camadas;
2 - Acima de neg�cios encontramos a camada de servi�o. Essa camada implementa todos os contratos definidos na camada inferior, e acessa os modelos de neg�cio necess�rios. Ela tamb�m define os contratos de toda infraestrutura necess�ria;
3 - E por �ltimo temos a camada de infraestrutura, que est� mais externa as demais, e tem acesso a todas as outras implementa��es;

Todas as camadas tem implementa��o independentes, respeitando apenas os contratos exigidos nas camadas subjacentes.

## Frameworks utilizados
O projeto principal da apli��o utiliza-se apenas das bibliotecas padr�es de projetos em .NET Core 3.1, de modo a se manter simples e leve.
Contudo o projeto de testes faz uso de algumas bibliotecas comuns, tais como:
- Microsoft.NET.Test.Sdk;
- Moq;
- NUnit;
- NUnit3TestAdapter;

# Executando o projeto
## Requisitos
Para executar o projeto � necess�rio ter instalado no ambiente local as seguintes ferramentas:
- Microsoft .NETCore SDK vers�o 3.1+;
- Microsoft.NETCore.App runtime vers�o 3.1+;

## Executar os comandos no terminal:
- Baixar as depend�ncias necess�rias: dotnet restore
- Compilar o projeto: dotnet build
- Executar o projeto com acesso ao teminal para entrada de informa��es na aplica��o: dotnet run --interactive



