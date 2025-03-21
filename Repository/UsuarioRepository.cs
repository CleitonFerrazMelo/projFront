﻿using Microsoft.AspNetCore.Identity;
using projFront.Data;
using projFront.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SQLite;

namespace projFront.Repository
{
    public class UsuarioRepository : IUsuarioRepository
    {
        public readonly AppDbContext _repo;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public UsuarioRepository(AppDbContext repo, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _repo = repo;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public void Deletar(Usuario usuario)
        {
            _repo.Remove(usuario);
            _repo.SaveChanges();
        }

        public ApplicationUser BuscarUserPorEmail(string email)
        {
            return _userManager.FindByEmailAsync(email).Result;
        }
        public List<ApplicationUser> ListarTodosOsUsuariosAsync()
        {
            return _userManager.Users.ToList();
        }

        public List<ApplicationUser> BuscarUserPorBanco(int IdBanco)
        {
            List<ApplicationUser> listaUserPorBanco = new List<ApplicationUser>();
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
                ApplicationUser usuario = _userManager.Users.FirstOrDefault(x => x.Id == iduser);
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

        public Regra BuscarRegraPorUsuario(ApplicationUser nomeUsuario)
        {
            List<UsuarioRegra> listaUsuarioRegra = new List<UsuarioRegra>();
            var regra = _userManager.GetRolesAsync(nomeUsuario);
            Regra regraModel = new Regra();
            
            foreach (var item in regra.Result)
            {
                regraModel = BuscarRegraPorNome(item);
            }
            
            return regraModel;
        }

        public Regra BuscarRegraPorNome(string nomeRegra)
        {
            Regra regra = new Regra();
            if (nomeRegra != null)
            {
                IdentityRole identityRole = _roleManager.Roles.FirstOrDefault(x => x.Name == nomeRegra);
                
                regra.IdRegra = identityRole.Id;
                regra.Nome = identityRole.Name;
            }
            
            return regra;
        }
        
        public ApplicationUser BuscarUsuarioPorID(string id)
        {
            return _userManager.Users.FirstOrDefault(x => x.Id == id);
        }

        public List<IdentityRole> BuscarRegras()
        {
            return _roleManager.Roles.ToList();
        }

        public void LimparRegraUsuario(string id)
        {
            try
            {
                using (var cmd = DbConnection().CreateCommand())
                {
                    cmd.CommandText = "DELETE from AspNetUserRoles WHERE UserId = @UserId ";
                    cmd.Parameters.AddWithValue("@UserId", id);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AlterarDadosNota(ApplicationUser identityUser)
        {
            _userManager.UpdateAsync(identityUser);
            
        }

        public void CadastrarRegraNoUsuario(string userId, string roleId)
        {
            try
            {
                using (var cmd = DbConnection().CreateCommand())
                {
                    cmd.CommandText = "DELETE FROM AspNetUserRoles WHERE UserId = @UserId";
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            try
            {
                using (var cmd = DbConnection().CreateCommand())
                {
                    

                    cmd.CommandText = "INSERT INTO AspNetUserRoles (UserId, RoleId ) values (@UserId, @RoleId)";
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    cmd.Parameters.AddWithValue("@RoleId", roleId);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool ValidarNumeroNotaFiscal(ApplicationUser identityUser)
        {
            bool usuarioDiferente = false;
            try
            {
                List<ApplicationUser> listaIdentityUser = _userManager.Users.Where(x => x.NumeroDaNotaAtual >= identityUser.NumeroDaNotaAtual && x.UltimoNumeroDaNota <= identityUser.NumeroDaNotaAtual).ToList();
                foreach(ApplicationUser item in listaIdentityUser)
                {
                    if (item.Id != identityUser.Id)
                    {
                        usuarioDiferente = true;
                        break;
                    }
                }
                if (!usuarioDiferente)
                {
                    listaIdentityUser = _userManager.Users.Where(x => x.NumeroDaNotaAtual >= identityUser.UltimoNumeroDaNota && x.UltimoNumeroDaNota <= identityUser.UltimoNumeroDaNota).ToList();
                    foreach (ApplicationUser item in listaIdentityUser)
                    {
                        if (item.Id != identityUser.Id)
                        {
                            usuarioDiferente = true;
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return usuarioDiferente;
        }
    }
}
