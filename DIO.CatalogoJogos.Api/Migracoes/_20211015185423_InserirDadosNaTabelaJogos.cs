using System;
using FluentMigrator;

namespace DIO.CatalogoJogos.Api.Migracoes
{
  [Migration(20211015185423)]
  public class _20211015185423_InserirDadosNaTabelaJogos : Migration
  {
    public override void Up()
    {
      Insert.IntoTable("Jogos")
      .Row(
        new
        {
          Id = Guid.Parse("A4C69C2F-D2CA-4D03-AFD7-6022400DB8E9"),
          Nome = "FIFA 21",
          Preco = 288.29,
          Categoria = "Esportes",
          ProdutoraId = Guid.Parse("C8133002-F17A-465D-905B-F2EA6B69AF9B")
        });
    }

    public override void Down()
    {
      Delete.FromTable("Jogos");
    }
  }
}