using FluentMigrator;

namespace DIO.CatalogoJogos.Api.Migracoes
{
  [Migration(20211010172415)]
  public class _20211010172415_CriarTabelaProdutora : Migration
  {
    public override void Up()
    {
      Create.Table("Produtoras")
      .WithColumn("Id").AsGuid().PrimaryKey().Indexed().NotNullable()
      .WithColumn("Nome").AsString().NotNullable();
    }

    public override void Down()
    {
      Delete.Table("Produtoras");
    }
  }
}