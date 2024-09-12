using Microsoft.AspNetCore.Identity;
using Microsoft.Data.Sqlite;
using projFront.Data;
using projFront.Models;
using System.Data.SQLite;

namespace projFront.Repository
{
    public class BancoRepository : IBancoRepository
    {
        public readonly AppDbContext _repo;
        private readonly NotaFiscal _notaFiscal;
        public BancoRepository(AppDbContext repo)
        {
            _repo = repo;
        }
        public void Deletar(Banco banco)
        {
            _repo.Remove(banco);
            _repo.SaveChanges();
        }

        public bool RelacionadoNotaFiscal(int id)
        {
            return _repo.NotaFiscal.Where(x => x.IdBanco == id).Any();
        }

        public List<Banco> GetBancos()
        {
            return _repo.Bancos.ToList();
        }

        public Banco GetBanco(int id)
        {
            return _repo.Bancos.Where(x => x.IdBanco == id).FirstOrDefault();
        }

        public void ValidaIncluiUsuario(List<string> listIdUsuario, int idBanco)
        {
            foreach (var idUsario in listIdUsuario)
            {
                CadastrarUsuarioBanco(idBanco, idUsario);
            }
        }
        public void LimpaEIncluiUsuario(List<string> listIdUsuario, int idBanco)
        {
            List<string> listaUsuario = RetornaUsuarioPorBanco(idBanco);

            foreach (var idUsario in listaUsuario)
            {
                DeletarUsuarioBanco(idBanco, idUsario);
            }
            foreach (var idUsario in listIdUsuario)
            {
                CadastrarUsuarioBanco(idBanco, idUsario);
            }
        }

        public List<Banco> ListaBancosPorUsuario(string idUsuario)
        {
            List<Banco> listaBanco = new List<Banco>();
            try
            {
                using (var cmd = DbConnection().CreateCommand())
                {
                    cmd.CommandText = "select * from BancoUsuario where IDUsuario = @IDUsuario";
                    cmd.Parameters.AddWithValue("@IDUsuario", idUsuario);
                    cmd.ExecuteNonQuery();

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            List<int> listIdBanco = new List<int>();
                            
                            // Obtendo os índices das colunas pelos nomes
                            int idBancoUsuarioIndex = reader.GetOrdinal("IDBancoUsuario");
                            int idBancoIndex = reader.GetOrdinal("IDBanco");
                            int idUsuarioIndex = reader.GetOrdinal("IDUsuario");

                            // Atribuindo os valores às propriedades do objeto
                            var bancoUsuario = reader.GetInt32(idBancoUsuarioIndex);
                            var idBanco  = reader.GetInt32(idBancoIndex);
                            var usuario = reader.GetString(idUsuarioIndex);

                            listIdBanco.Add(idBanco);

                            foreach (var idbank in listIdBanco)
                            {
                                Banco banco = _repo.Bancos.FirstOrDefault(x => x.IdBanco == idbank);
                                listaBanco.Add(banco);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return listaBanco;
        }

        private static SQLiteConnection DbConnection()
        {
            var sqliteConnection = new SQLiteConnection("Data Source=baseTeste.db; ");
            sqliteConnection.Open();
            return sqliteConnection;
        }

        private void CadastrarUsuarioBanco(int idBanco, string idUsuario)
        {
            try
            {
                using (var cmd = DbConnection().CreateCommand())
                {
                    cmd.CommandText = "INSERT INTO BancoUsuario(IDBanco, IDUsuario ) values (@IDBanco, @IDUsuario)";
                    cmd.Parameters.AddWithValue("@IDBanco", idBanco);
                    cmd.Parameters.AddWithValue("@IDUsuario", idUsuario);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void DeletarUsuarioBanco(int idBanco, string idUsuario)
        {
            try
            {
                using (var cmd = DbConnection().CreateCommand())
                {
                    cmd.CommandText = "DELETE from BancoUsuario WHERE IDBanco = @IDBanco AND IDUsuario = @IDUsuario";
                    cmd.Parameters.AddWithValue("@IDBanco", idBanco);
                    cmd.Parameters.AddWithValue("@IDUsuario", idUsuario);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private List<string> RetornaUsuarioPorBanco(int idBanco)
        {
            List<string> listIdUsuario = new List<string>();
            try
            {
                using (var cmd = DbConnection().CreateCommand())
                {
                    cmd.CommandText = "select * from BancoUsuario where IDBanco = @IdBanco";
                    cmd.Parameters.AddWithValue("@IdBanco", idBanco);
                    cmd.ExecuteNonQuery();

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            

                            // Obtendo os índices das colunas pelos nomes
                            int idBancoUsuarioIndex = reader.GetOrdinal("IDBancoUsuario");
                            int idBancoIndex = reader.GetOrdinal("IDBanco");
                            int idUsuarioIndex = reader.GetOrdinal("IDUsuario");

                            // Atribuindo os valores às propriedades do objeto
                            string idUsuario = reader.GetString(idUsuarioIndex);

                            listIdUsuario.Add(idUsuario);

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return listIdUsuario;
        }

    }
}
