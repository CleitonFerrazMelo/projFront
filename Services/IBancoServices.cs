using projFront.Models;

namespace projFront.Services
{
    public interface IBancoServices
    {
        public string ValidarDelecao(Banco banco);
        public List<Banco> GetBancos();
        public List<Banco> GetBanco(int id);
        void ValidaIncluiUsuario(List<string> listIdUsuario, int idBanco);
        public List<Banco> ListaBancosPorUsuario(string idUsuario);
    }
}
