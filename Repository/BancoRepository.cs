﻿using projFront.Data;
using projFront.Models;

namespace projFront.Repository
{
    public class BancoRepository: IBancoRepository
    {
        public readonly AppDbContext _repo;

        public BancoRepository(AppDbContext repo)
        {
            _repo = repo;
        }
        public void Deletar(Banco banco)
        {
            _repo.Remove(banco);
            _repo.SaveChanges();
        }
    }
}
