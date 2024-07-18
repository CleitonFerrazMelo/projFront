using AutoMapper;
using projFront.Models;

namespace projFront.ViewModels.Mappings
{
    public class MapeamentoVMtoModel : Profile
    {
        public MapeamentoVMtoModel()
        {
            CreateMap<Banco, BancoViewModel>().ReverseMap();
            CreateMap<Empresa, EmpresaViewModel>().ReverseMap();
            CreateMap<NotaFiscal, NotaFiscalViewModel>().ReverseMap();
        }
    }
}
