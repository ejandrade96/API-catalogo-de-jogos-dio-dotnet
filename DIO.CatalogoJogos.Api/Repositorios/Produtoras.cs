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
  public class Produtoras : IProdutoras
  {
    private readonly IConfiguration _config;

    private readonly string _connectionString;

    private readonly SqliteConnection _sqliteConnection;

    public Produtoras(IConfiguration config)
    {
      _config = config;
      _connectionString = $"Data Source={System.IO.Directory.GetCurrentDirectory()}/{_config.GetConnectionString("SqliteConnection")}";
      _sqliteConnection = new SqliteConnection(_connectionString);
    }

    public async Task<Produtora> Inserir(Produtora produtora)
    {
      await _sqliteConnection.ExecuteAsync("INSERT INTO Produtoras (ID, NOME) VALUES (@Id, @Nome)", produtora);
      await _sqliteConnection.CloseAsync();
      return produtora;
    }

    public async Task<List<Produtora>> Listar()
    {
      var produtoras = (await _sqliteConnection.QueryAsync("SELECT * FROM Produtoras")).Select(produtora => new Produtora(produtora.Nome) { Id = Guid.Parse((string)produtora.Id) }).ToList();
      await _sqliteConnection.CloseAsync();
      return produtoras;
    }

    public async Task<Produtora> ObterPorId(Guid id)
    {
      var produtora = await _sqliteConnection.QueryFirstOrDefaultAsync($"SELECT * FROM Produtoras WHERE UPPER(ID) = UPPER('{id}')");
      await _sqliteConnection.CloseAsync();
      return produtora != null ? new Produtora((string)produtora.Nome) { Id = Guid.Parse((string)produtora.Id) } : null;
    }

    public async Task Atualizar(Produtora produtora)
    {
      _ = await _sqliteConnection.ExecuteAsync($"UPDATE Produtoras SET Nome = '{produtora.Nome}' WHERE UPPER(ID) = UPPER('{produtora.Id}')");
      await _sqliteConnection.CloseAsync();
    }

    public async Task Remover(Guid id)
    {
      _ = await _sqliteConnection.ExecuteAsync($"DELETE FROM Produtoras WHERE UPPER(ID) = UPPER('{id}')");
      await _sqliteConnection.CloseAsync();
    }

    public void Dispose()
    {
      _sqliteConnection?.Close();
      _sqliteConnection?.Dispose();
    }
  }
}