using FluentMigrator;

namespace DIO.CatalogoJogos.Api.Migracoes
{
  [Migration(20211010172626)]
  public class _20211010172626_CriarTabelaJogo : Migration
  {
    public override void Up()
    {
      Create.Table("Jogos")
      .WithColumn("Id").AsGuid().PrimaryKey().Indexed().NotNullable()
      .WithColumn("Nome").AsString().NotNullable()
      .WithColumn("Preco").AsDouble().NotNullable()
      .WithColumn("Categoria").AsString().NotNullable()
      .WithColumn("ProdutoraId").AsGuid().ReferencedBy("Produtoras", "Id").NotNullable();
    }

    public override void Down()
    {
      Delete.Table("Jogos");
    }
  }
}