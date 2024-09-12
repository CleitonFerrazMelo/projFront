using projFront.Models;

namespace projFront.Repository
{
    public interface IBancoRepository
    {
        public void Deletar(Banco banco);

        public bool RelacionadoNotaFiscal(int id);

        public List<Banco> GetBancos();
        public Banco GetBanco(int id);
        public void ValidaIncluiUsuario(List<string> listIdUsuario, int idBanco);
        public List<Banco> ListaBancosPorUsuario(string idUsuario);
        public void LimpaEIncluiUsuario(List<string> listIdUsuario, int idBanco);
    }
}
