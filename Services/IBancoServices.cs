﻿using projFront.Models;

namespace projFront.Services
{
    public interface IBancoServices
    {
        public string ValidarDelecao(Banco banco);
        public List<Banco> GetBancos();
        public List<Banco> GetBanco(int id);
    }
}
