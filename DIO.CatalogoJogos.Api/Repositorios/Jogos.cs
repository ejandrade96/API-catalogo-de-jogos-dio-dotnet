using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using DIO.CatalogoJogos.Api.Modelos;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;

namespace DIO.CatalogoJogos.Api.Repositorios
{
  public class Jogos : IJogos
  {
    private readonly IConfiguration _config;

    private readonly string _connectionString;

    private readonly SqliteConnection _sqliteConnection;

    public Jogos(IConfiguration config)
    {
      _config = config;
      _connectionString = $"Data Source={System.IO.Directory.GetCurrentDirectory()}/{_config.GetConnectionString("SqliteConnection")}";
      _sqliteConnection = new SqliteConnection(_connectionString);
    }

    public async Task<Jogo> Inserir(Jogo jogo)
    {
      await _sqliteConnection.ExecuteAsync("INSERT INTO Jogos (ID, NOME, PRECO, CATEGORIA, PRODUTORAID) VALUES (@Id, @Nome, @Preco, @Categoria, @ProdutoraId)", jogo);
      await _sqliteConnection.CloseAsync();
      return jogo;
    }

    public async Task<Jogo> ObterPorNomeEProdutoraId(string nome, Guid produtoraId)
    {
      var jogo = await _sqliteConnection.QueryFirstOrDefaultAsync($"SELECT * FROM Jogos WHERE UPPER(ProdutoraId) = UPPER('{produtoraId}') AND UPPER(NOME) = UPPER('{nome}')");
      await _sqliteConnection.CloseAsync();
      return jogo != null ? ToModel(jogo) : null;
    }

    public async Task<List<Jogo>> Listar(int pagina, int quantidade, Guid? produtoraId)
    {
      var query = "";

      if (produtoraId != Guid.Empty)
        query = $"SELECT * FROM Jogos WHERE UPPER(ProdutoraId) = UPPER('{produtoraId.Value}') ORDER BY ID LIMIT {quantidade} OFFSET {((pagina - 1) * quantidade)}";

      else
        query = $"SELECT * FROM Jogos ORDER BY ID LIMIT {quantidade} OFFSET {((pagina - 1) * quantidade)}"; ;

      var jogos = (await _sqliteConnection.QueryAsync(query) as List<dynamic>).Select<dynamic, Jogo>(jogo => ToModel(jogo)).ToList();
      await _sqliteConnection.CloseAsync();

      return jogos;
    }

    public async Task<Jogo> ObterPorId(Guid id)
    {
      var jogo = await _sqliteConnection.QueryFirstOrDefaultAsync($"SELECT * FROM Jogos WHERE UPPER(ID) = UPPER('{id}')");
      await _sqliteConnection.CloseAsync();
      return jogo != null ? ToModel(jogo) : null;
    }

    public async Task Atualizar(Jogo jogo)
    {
      _ = await _sqliteConnection.ExecuteAsync($"UPDATE Jogos SET Nome = '{jogo.Nome}', Preco = '{jogo.Preco}', Categoria = '{jogo.Categoria}' WHERE UPPER(ID) = UPPER('{jogo.Id}')");
      await _sqliteConnection.CloseAsync();
    }

    public async Task Remover(Guid id)
    {
      _ = await _sqliteConnection.ExecuteAsync($"DELETE FROM Jogos WHERE UPPER(ID) = UPPER('{id}')");
      await _sqliteConnection.CloseAsync();
    }

    private Jogo ToModel(dynamic jogoTipoDapper) => new Jogo((string)jogoTipoDapper.Nome, Convert.ToDouble(jogoTipoDapper.Preco), (string)jogoTipoDapper.Categoria, Guid.Parse((string)jogoTipoDapper.ProdutoraId)) { Id = Guid.Parse((string)jogoTipoDapper.Id) };

    public void Dispose()
    {
      _sqliteConnection?.Close();
      _sqliteConnection?.Dispose();
    }
  }
}