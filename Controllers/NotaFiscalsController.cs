using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Xml.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using projFront.Data;
using projFront.Models;
using projFront.Repository;
using projFront.Services;
using projFront.ViewModels;
using Rotativa.AspNetCore;

namespace projFront.Controllers
{
    [Authorize(Roles = "Admin, Operador")]
    public class NotaFiscalsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly INotaFiscalServices _notaFiscalServices;
        private readonly IEmpresaServices _empresaServices;
        private readonly IBancoServices _bancoServices;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public NotaFiscalsController(AppDbContext context, INotaFiscalServices notaFiscalServices, IMapper mapper, IEmpresaServices empresaServices, IBancoServices bancoServices, IHttpContextAccessor httpContextAccessor, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _notaFiscalServices = notaFiscalServices;
            _mapper = mapper;
            _empresaServices = empresaServices;
            _bancoServices = bancoServices;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // GET: NotaFiscals
        public async Task<IActionResult> Index()
        {
            ViewData["PaginaSelecionada"] = "NotaFiscal";
            ViewData["DireitoUsuario"] = IdentificaDireitoUsuario();

            List<NotaFiscalViewModel> listaNotaFiscalViewModel = new List<NotaFiscalViewModel>();
            //_notaFiscalServices.RetornaListaNotaFiscal()

            if (_context.Empresas != null)
            {
                // Obter lista de NotaFiscal
                List<NotaFiscal> listaNotaFiscal = await _context.NotaFiscal.ToListAsync();

                // Mapear para a ViewModel
                //listaNotaFiscalViewModel = _mapper.Map<IEnumerable<NotaFiscalViewModel>>(listaNotaFiscal);

                foreach (var nota in listaNotaFiscal)
                {
                    var nfVm = transformaNotaficalVM(nota);
                    listaNotaFiscalViewModel.Add(nfVm);
                }

                // Criar um dicionário para mapear o ID da NotaFiscal para a inscrição estadual
                var inscricaoEstadualMap = listaNotaFiscal.ToDictionary(nf => nf.Id, nf => nf.IncricaoEstadual);

                // Atribuir a inscrição estadual utilizando o dicionário
                foreach (var nfViewModel in listaNotaFiscalViewModel)
                {
                    if (inscricaoEstadualMap.TryGetValue(nfViewModel.Id, out var inscricaoEstadual))
                    {
                        nfViewModel.Ie = inscricaoEstadual;
                    }
                }
            }

            var usuarioLogado = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            List<Empresa> listEmpresa = _empresaServices.GetEmpresas();

            List<Banco> listaBanco =  _bancoServices.ListaBancosPorUsuario(usuarioLogado);

            List<NotaFiscalViewModel> listaFiltrada = new List<NotaFiscalViewModel>();

            foreach (var notas in listaNotaFiscalViewModel)
            {
                int idEmpresaRelacionada = int.Parse(notas.IdEmpresa);
                Empresa empresa = listEmpresa.FirstOrDefault(x => x.IdEmpresa == idEmpresaRelacionada);
                notas.Empresa = new List<Empresa>();
                notas.Empresa.Add(empresa);
                foreach (var bancosDoUsuario in listaBanco)
                {

                    if(bancosDoUsuario.IdBanco == notas.IdBanco)
                        listaFiltrada.Add(notas);
                }
            }

            return View(listaFiltrada);
        }

        // GET: NotaFiscals/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            ViewData["PaginaSelecionada"] = "NotaFiscal";
            ViewData["DireitoUsuario"] = IdentificaDireitoUsuario();
            if (id == null || _context.NotaFiscal == null)
            {
                return NotFound();
            }

            var notaFiscal = await _context.NotaFiscal
                .FirstOrDefaultAsync(m => m.Id == id);
            if (notaFiscal == null)
            {
                return NotFound();
            }
            NotaFiscalViewModel notaFiscalVM = _mapper.Map<NotaFiscalViewModel>(notaFiscal);
            return View(notaFiscalVM);
        }

        // GET: NotaFiscals/Create
        public IActionResult Create()
        {
            ViewData["PaginaSelecionada"] = "NotaFiscal";
            ViewData["DireitoUsuario"] = IdentificaDireitoUsuario();

            List<Empresa> listEmpresa = new List<Empresa>();

            listEmpresa = _empresaServices.GetEmpresas();

            NotaFiscalViewModel notaFiscalViewModel = new NotaFiscalViewModel();

            notaFiscalViewModel.Empresa = listEmpresa;

            List<Banco> listBanco = new List<Banco>();

            var usuarioLogado = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            listBanco =_bancoServices.ListaBancosPorUsuario(usuarioLogado);

            notaFiscalViewModel.Banco = listBanco;

            return View(notaFiscalViewModel);
        }

        // POST: NotaFiscals/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( NotaFiscalViewModel notaFiscalVM)
        {
            ViewData["PaginaSelecionada"] = "NotaFiscal";
            ViewData["DireitoUsuario"] = IdentificaDireitoUsuario();
            notaFiscalVM.Empresa = _empresaServices.GetEmpresa(Convert.ToInt32(notaFiscalVM.IdEmpresa));
            notaFiscalVM.MensagemFisco = notaFiscalVM.Empresa[0].MensagemFisco;
            notaFiscalVM.Banco   = _bancoServices.GetBanco(notaFiscalVM.IdBanco);
            notaFiscalVM.UserName = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Name).Value;

            NotaFiscal notaFiscal = transformaNotafical(notaFiscalVM);
               
            _context.Add(notaFiscal);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            
        }

        private NotaFiscal transformaNotafical(NotaFiscalViewModel notaFiscalVM)
        {
            NotaFiscal notaFiscal = new NotaFiscal();
            notaFiscal.Id = notaFiscalVM.Id;
            notaFiscal.Nome = notaFiscalVM.Nome;
            notaFiscal.Cnpj = notaFiscalVM.Cnpj;
            notaFiscal.IncricaoEstadual = notaFiscalVM.Ie == null ? "" : notaFiscalVM.Ie;
            notaFiscal.Endereco = notaFiscalVM.Endereco;
            notaFiscal.Numero = notaFiscalVM.Numero;
            notaFiscal.Bairro = notaFiscalVM.Bairro;
            notaFiscal.NomeCidade = notaFiscalVM.NomeCidade;
            notaFiscal.Uf = notaFiscalVM.Uf;
            notaFiscal.Cep = notaFiscalVM.Cep;
            notaFiscal.NumeroTelefone = notaFiscalVM.NumeroTelefone == null ? "" : notaFiscalVM.NumeroTelefone;
            notaFiscal.DescricaoServico = notaFiscalVM.DescricaoServico;
			CultureInfo culturaBrasileira = new CultureInfo("pt-BR");
            decimal valorDecimal = 0;

            if (notaFiscalVM.ValorTotal.Contains(","))
            {
                notaFiscalVM.ValorTotal = notaFiscalVM.ValorTotal.Replace(",", ".");
            }

            if (!notaFiscalVM.ValorTotal.Contains("."))
            {
                notaFiscalVM.ValorTotal = notaFiscalVM.ValorTotal + ".00";
            }

            if (decimal.TryParse(notaFiscalVM.ValorTotal, NumberStyles.Any, CultureInfo.InvariantCulture, out valorDecimal))
			{
				Console.WriteLine("Valor decimal: " + valorDecimal); // Imprimirá 180.85
			}
			else
			{
				Console.WriteLine("Falha na conversão.");
			}
			notaFiscal.ValorTotal = Math.Round(valorDecimal, 2);
            notaFiscal.IdBanco = notaFiscalVM.IdBanco;
            notaFiscal.Agencia = notaFiscalVM.Banco[0].Agencia;
            notaFiscal.Conta = notaFiscalVM.Banco[0].TipoConta;
            notaFiscal.PixChave = notaFiscalVM.Banco[0].PixChave;
            notaFiscal.PixNumero = notaFiscalVM.Banco[0].PixNumero;
            notaFiscal.IdEmpresa = Convert.ToInt32( notaFiscalVM.IdEmpresa);
            notaFiscal.DataEmissao = notaFiscalVM.DataEmissao;
            notaFiscal.FaturaSerie = notaFiscalVM.FaturaSerie;
            notaFiscal.FaturaNumero = notaFiscalVM.FaturaNumero;
            notaFiscal.MensagemFisco = notaFiscalVM.MensagemFisco == null ? "" : notaFiscalVM.MensagemFisco;
            notaFiscal.FaturaNumero = notaFiscal.FaturaNumero == null ? 0 : notaFiscal.FaturaNumero;
            notaFiscal.FaturaSerie = notaFiscal.FaturaSerie == null ? "0" : notaFiscal.FaturaSerie;
            notaFiscal.NumeroTelefone = notaFiscal.NumeroTelefone == null ? "" : notaFiscal.NumeroTelefone;
            notaFiscal.UserName = notaFiscalVM.UserName;
            notaFiscal.Observacoes = notaFiscalVM.Observacoes == null ? "" : notaFiscalVM.Observacoes;
            notaFiscal.Vencimento = notaFiscalVM.Vencimento == null ? "" : notaFiscalVM.Vencimento;
            return notaFiscal;
        }

        private NotaFiscalViewModel transformaNotaficalVM(NotaFiscal notaFiscal)
        {
            NotaFiscalViewModel notaFiscalVM = new NotaFiscalViewModel();
            notaFiscalVM.Id = notaFiscal.Id;
            notaFiscalVM.Nome = notaFiscal.Nome;
            notaFiscalVM.Cnpj = notaFiscal.Cnpj;
            notaFiscalVM.Ie = notaFiscal.IncricaoEstadual == null ? "" : notaFiscal.IncricaoEstadual;
            notaFiscalVM.Endereco = notaFiscal.Endereco;
            notaFiscalVM.Numero = notaFiscal.Numero;
            notaFiscalVM.Bairro = notaFiscal.Bairro;
            notaFiscalVM.NomeCidade = notaFiscal.NomeCidade;
            notaFiscalVM.Uf = notaFiscal.Uf;
            notaFiscalVM.Cep = notaFiscal.Cep;
            notaFiscalVM.NumeroTelefone = notaFiscal.NumeroTelefone == null ? "" : notaFiscal.NumeroTelefone;
            notaFiscalVM.DescricaoServico = notaFiscal.DescricaoServico;
            notaFiscalVM.ValorTotal = Convert.ToString(notaFiscal.ValorTotal);
            notaFiscalVM.ValorTotal = notaFiscalVM.ValorTotal + "00000";
            decimal valor = decimal.Parse(notaFiscalVM.ValorTotal);
            notaFiscalVM.ValorTotal = valor.ToString("0.00");
            notaFiscalVM.IdBanco = notaFiscal.IdBanco;

            var empresas = _empresaServices.GetEmpresas(); // GetEmpresa(Convert.ToInt32(notaFiscal.IdEmpresa));
            var bancos = _bancoServices.GetBancos(); // (notaFiscal.IdBanco);

            var empresa = _empresaServices.GetEmpresa(Convert.ToInt32(notaFiscal.IdEmpresa));
            var banco = _bancoServices.GetBanco(notaFiscal.IdBanco);

            notaFiscalVM.Empresa = empresas;
            notaFiscalVM.Banco = bancos;

            if(banco != null)
            {
                notaFiscalVM.Agencia = banco[0].Agencia;
                notaFiscalVM.Conta = banco[0].TipoConta;
                notaFiscalVM.PixChave = banco[0].PixChave;
                notaFiscalVM.PixNumero = banco[0].PixNumero;
            }

            notaFiscalVM.IdEmpresa = Convert.ToString(notaFiscal.IdEmpresa);
            notaFiscalVM.DataEmissao = notaFiscal.DataEmissao;
            notaFiscalVM.FaturaSerie = notaFiscal.FaturaSerie;
            notaFiscalVM.FaturaNumero = notaFiscal.FaturaNumero;
            notaFiscalVM.MensagemFisco = notaFiscal.MensagemFisco;
            notaFiscalVM.FaturaNumero = notaFiscal.FaturaNumero == null ? 0 : notaFiscal.FaturaNumero;
            notaFiscalVM.FaturaSerie = notaFiscal.FaturaSerie == null ? "0" : notaFiscal.FaturaSerie;
            notaFiscalVM.NumeroTelefone = notaFiscal.NumeroTelefone == null ? "" : notaFiscal.NumeroTelefone;
            notaFiscalVM.UserName = notaFiscal.UserName;
			notaFiscalVM.Observacoes = notaFiscal.Observacoes;
            notaFiscalVM.Vencimento = notaFiscal.Vencimento;
			return notaFiscalVM;
        }

        // GET: NotaFiscals/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            ViewData["PaginaSelecionada"] = "NotaFiscal";
            ViewData["DireitoUsuario"] = IdentificaDireitoUsuario();
            if (id == null || _context.NotaFiscal == null)
            {
                return NotFound();
            }

            var notaFiscal = await _context.NotaFiscal.FindAsync(id);

            if (notaFiscal == null)
            {
                return NotFound();
            }
            NotaFiscalViewModel notaFiscalVM = transformaNotaficalVM(notaFiscal);

            ViewBag.EmpresaSelecionada = _empresaServices.GetEmpresa(Convert.ToInt32(notaFiscalVM.IdEmpresa));
            ViewBag.BancoSelecionado = _bancoServices.GetBanco(notaFiscalVM.IdBanco);

            return View(notaFiscalVM);
        }

        // POST: NotaFiscals/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, NotaFiscalViewModel notaFiscalViewModel)
        {
            ViewData["PaginaSelecionada"] = "NotaFiscal";
            ViewData["DireitoUsuario"] = IdentificaDireitoUsuario();
            notaFiscalViewModel.UserName = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Name).Value;
            notaFiscalViewModel.Banco = _bancoServices.GetBanco(notaFiscalViewModel.IdBanco);
            notaFiscalViewModel.Empresa = _empresaServices.GetEmpresa( Convert.ToInt32( notaFiscalViewModel.IdEmpresa));

            NotaFiscal notaFiscal = transformaNotafical(notaFiscalViewModel);
            
            if (id != notaFiscal.Id)
            {
                return NotFound();
            }

                try
                {
                    _context.Update(notaFiscal);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NotaFiscalExists(notaFiscal.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            

            NotaFiscalViewModel notaFiscalVM = _mapper.Map<NotaFiscalViewModel>(notaFiscal);

            return View(notaFiscalVM);
        }

        // GET: NotaFiscals/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            ViewData["PaginaSelecionada"] = "NotaFiscal";
            ViewData["DireitoUsuario"] = IdentificaDireitoUsuario();
            if (id == null || _context.NotaFiscal == null)
            {
                return NotFound();
            }

            var notaFiscal = await _context.NotaFiscal
                .FirstOrDefaultAsync(m => m.Id == id);
            if (notaFiscal == null)
            {
                return NotFound();
            }
            NotaFiscalViewModel notaFiscalVM = _mapper.Map<NotaFiscalViewModel>(notaFiscal);
            return View(notaFiscalVM);
        }

        // POST: NotaFiscals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            ViewData["PaginaSelecionada"] = "NotaFiscal";
            ViewData["DireitoUsuario"] = IdentificaDireitoUsuario();
            if (_context.NotaFiscal == null)
            {
                return Problem("Entity set 'AppDbContext.NotaFiscal'  is null.");
            }
            var notaFiscal = await _context.NotaFiscal.FindAsync(id);
            if (notaFiscal != null)
            {
                string mensagem = _notaFiscalServices.ValidarDelecao(notaFiscal);
                if (!string.IsNullOrEmpty(mensagem))
                {
                    NotaFiscalViewModel notaFiscalVM = _mapper.Map<NotaFiscalViewModel>(notaFiscal);
                    TempData["MensagemErro"] = mensagem;
                    return View(notaFiscalVM);
                }
                   
                _context.NotaFiscal.Remove(notaFiscal);
            }
            
            //await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // POST: NotaFiscals/RetornarUltimaNota/5
        [HttpGet]
        public NotaFiscalViewModel? RetornarUltimaNota( string cnpj)
        {            
            var notaFiscal = _notaFiscalServices.RetornarUltimaNota(cnpj);
            NotaFiscalViewModel notaFiscalVM = transformaNotaficalVM(notaFiscal);
            return notaFiscalVM;
        }

        // POST: NotaFiscals/Imprimir/5
        [HttpPost, ActionName("Imprimir")]
        [ValidateAntiForgeryToken]
        public string Imprimir(NotaFiscal notaFiscal)
        {
            return _notaFiscalServices.Imprimir(notaFiscal);
        }

        public async Task<ActionResult> ImprimePDF(int id)
        {

            if (id == null || _context.NotaFiscal == null)
            {
                TempData["MensagemErro"] = "Nota Fiscal não encontrada.";
                return NotFound();
            }

            var notaFiscal = await _context.NotaFiscal.FindAsync(id);
            _notaFiscalServices.Imprimir(notaFiscal);
            if (notaFiscal == null)
            {
                TempData["MensagemErro"] = "Nota Fiscal não encontrada.";
                return NotFound();
            }
            NotaFiscalViewModel notaFiscalVM = _mapper.Map<NotaFiscalViewModel>(notaFiscal);

			notaFiscalVM.ValorTotal = Convert.ToString(notaFiscal.ValorTotal);
			notaFiscalVM.ValorTotal = notaFiscalVM.ValorTotal + "00000";
			decimal valor = decimal.Parse(notaFiscalVM.ValorTotal);
			notaFiscalVM.ValorTotal = valor.ToString("0.00");
            var banco = _bancoServices.GetBanco(notaFiscalVM.IdBanco);
            notaFiscalVM.Banco = banco;
            notaFiscalVM.Ie = notaFiscal.IncricaoEstadual;
			notaFiscalVM.Empresa = _empresaServices.GetEmpresa(notaFiscal.IdEmpresa);
            notaFiscalVM.MensagemFisco = notaFiscalVM.Empresa[0].MensagemFisco;
            string nomePDF = "FATURA."+ notaFiscal.FaturaNumero + "_"+ notaFiscalVM.Nome;
            nomePDF = nomePDF.Replace(" ", "");
            TempData["MensagemSucesso"] = "Nota fiscal gerada com sucesso.";

            return new ViewAsPdf("Impressao", notaFiscalVM) { FileName = nomePDF+".pdf" };
        }

        private bool NotaFiscalExists(int id)
        {
          return (_context.NotaFiscal?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        private string IdentificaDireitoUsuario()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            string direito = "Operador";

            if (userId != null)
            {
                var userRoles = _userManager.GetRolesAsync(_userManager.FindByIdAsync(userId).GetAwaiter().GetResult());

                direito = userRoles.Result[0];
            }

            return direito;
        }
    }
}
