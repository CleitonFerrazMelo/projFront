using projFront.Models;

namespace projFront.Repository
{
    public interface IBancoRepository
    {
        public void Deletar(Banco banco);

        public bool RelacionadoNotaFiscal(string id);
    }
}
