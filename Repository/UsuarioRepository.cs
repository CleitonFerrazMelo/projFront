using Microsoft.AspNetCore.Identity;
using projFront.Data;
using projFront.Models;
using System.Data.SQLite;

namespace projFront.Repository
{
    public class UsuarioRepository : IUsuarioRepository
    {
        public readonly AppDbContext _repo;
        private readonly UserManager<IdentityUser> _userManager;
        public UsuarioRepository(AppDbContext repo, UserManager<IdentityUser> userManager)
        {
            _repo = repo;
            _userManager = userManager;
        }

        public void Deletar(Usuario usuario)
        {
            _repo.Remove(usuario);
            _repo.SaveChanges();
        }

        public IdentityUser BuscarUserPorEmail(string email)
        {
            return _userManager.FindByEmailAsync(email).Result;
        }
        public List<IdentityUser> ListarTodosOsUsuariosAsync()
        {
            return _userManager.Users.ToList();
        }

        public List<IdentityUser> BuscarUserPorBanco(int IdBanco)
        {
            List<IdentityUser> listaUserPorBanco = new List<IdentityUser>();
            List<string> listIdUser = new List<string>();
            try
            {
                using (var cmd = DbConnection().CreateCommand())
                {
                    cmd.CommandText = "select * from BancoUsuario where IDBanco = @IdBanco";
                    cmd.Parameters.AddWithValue("@IdBanco", IdBanco);
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
                            var bancoUsuario = reader.GetInt32(idBancoUsuarioIndex);
                            var idBanco = reader.GetInt32(idBancoIndex);
                            var idUsuario = reader.GetString(idUsuarioIndex);

                            listIdUser.Add(idUsuario);

                            
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            foreach (string iduser in listIdUser)
            {
                IdentityUser usuario = _userManager.Users.FirstOrDefault(x => x.Id == iduser);
                listaUserPorBanco.Add(usuario);
            }

            return listaUserPorBanco;
        }
        private static SQLiteConnection DbConnection()
        {
            var sqliteConnection = new SQLiteConnection("Data Source=baseTeste.db; ");
            sqliteConnection.Open();
            return sqliteConnection;
        }
    }
}
