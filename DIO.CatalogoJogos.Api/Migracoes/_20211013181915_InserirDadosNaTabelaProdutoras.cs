using System;
using FluentMigrator;

namespace DIO.CatalogoJogos.Api.Migracoes
{
  [Migration(20211013181915)]
  public class _20211013181915_InserirDadosNaTabelaProdutoras : Migration
  {
    public override void Up()
    {
      Insert.IntoTable("Produtoras")
      .Row(
        new
        {
          Id = Guid.Parse("C8133002-F17A-465D-905B-F2EA6B69AF9B"),
          Nome = "EA Sports"
        });
    }

    public override void Down()
    {
      Delete.FromTable("Produtoras");
    }
  }
}