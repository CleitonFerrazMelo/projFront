﻿using projFront.Data;
using projFront.Models;

namespace projFront.Repository
{
    public class EmpresaRepository : IEmpresaRepository
    {
        public readonly AppDbContext _repo;
        private readonly NotaFiscal _notaFiscal;

        public EmpresaRepository(AppDbContext repo)
        {
            _repo = repo;
        }
        public void Deletar(Empresa empresa)
        {
            _repo.Remove(empresa);
            _repo.SaveChanges();
        }

        public Empresa GetEmpresa(int id)
        {
            return _repo.Empresas.Where(x => x.IdEmpresa == id).FirstOrDefault();
        }

        public List<Empresa> GetEmpresas()
        {
            return _repo.Empresas.ToList();
        }

        public bool RelacionadoNotaFiscal(int idEmpresa)
        {
            return _repo.NotaFiscal.Where(x => x.IdEmpresa == idEmpresa).Any();
        }
    }
}
