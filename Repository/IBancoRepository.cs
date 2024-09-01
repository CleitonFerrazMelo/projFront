using projFront.Models;

namespace projFront.Repository
{
    public interface IBancoRepository
    {
        public void Deletar(Banco banco);

        public bool RelacionadoNotaFiscal(int id);

        public List<Banco> GetBancos();
        public Banco GetBanco(int id);
        void ValidaIncluiUsuario(List<string> listIdUsuario, int idBanco);
        public List<Banco> ListaBancosPorUsuario(string idUsuario);
    }
}
