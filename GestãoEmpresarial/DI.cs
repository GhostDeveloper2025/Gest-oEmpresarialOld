using GestãoEmpresarial.Repositorios;
using GestãoEmpresarial.Repositorios.GestãoEmpresarial.Repositorios;
using GestãoEmpresarial.Validations;
using GestãoEmpresarial.ViewModels;
using GestãoEmpresarial.Views.Cadastro;
using GestãoEmpresarial.Views.Pesquisa;
using GestãoEmpresarial.Views.Relatorios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace GestãoEmpresarial
{
    /// <summary>
    /// DI = Dependency Injection 
    /// Serve para injectar os parametros nos contructores, definnado como serão instanciadas novas classes
    /// </summary>
    internal static class DI
    {
        private static TRepositorio GetRepositorio<TRepositorio>()
        where TRepositorio : class
        {
            return Activator.CreateInstance(typeof(TRepositorio), LoginViewModel.colaborador.IdFuncionario) as TRepositorio;
        }

        private static CadastroView GetCadastroView<TViewModel, TRepositorio, TValidar, TView>(int? id, params object[] args)
            where TRepositorio : class
            where TViewModel : ICadastroViewModel
            where TView : UIElement
        {
            var repositorio = GetRepositorio<TRepositorio>();
            var validar = Activator.CreateInstance<TValidar>();
            var view = Activator.CreateInstance<TView>();
            List<object> listArgs = new List<object>()
            {
                id, validar, repositorio
            };
            foreach (var item in args)
            {
                listArgs.Add(item);
            }
            var viewModel = Activator.CreateInstance(typeof(TViewModel), listArgs.ToArray()) as ICadastroViewModel;
            return new CadastroView(viewModel, view);
        }

        private static PesquisaView GetPesquisaView<TViewModel, TRepositorio>(params object[] args)
            where TRepositorio : class
            where TViewModel : IPesquisaViewModel
        {
            var repositorio = GetRepositorio<TRepositorio>();
            List<object> listArgs = new List<object>()
            {
                repositorio
            };
            foreach (var item in args)
            {
                listArgs.Add(item);
            }
            var viewModel = Activator.CreateInstance(typeof(TViewModel), listArgs.ToArray()) as IPesquisaViewModel;
            return new PesquisaView(viewModel);
        }

        public static Func<UserControl> GetCadastroView(string name)
        {
            return () => CadastrosViews[name](null);
        }

        internal static UserControl GetRelatorioView(string v)
        {
            return RelatoriosViews[v]();
        }

        public static Dictionary<string, Func<int?, UserControl>> CadastrosViews = new Dictionary<string, Func<int?, UserControl>>()
        {
            { nameof(CadastroClienteViewModel), (id) => GetCadastroView<CadastroClienteViewModel, RClienteDAL, ClienteValidar, CadastroClienteView>(id) },
            { nameof(CadastroColaboradorViewModel), (id) => GetCadastroView<CadastroColaboradorViewModel, RColaboradorDAL, ColaboradorValidar, CadastroColaboradorView>(id) },
            { nameof(CadastroCategoriaViewModel), (id) => GetCadastroView<CadastroCategoriaViewModel, RCategoriaDAL, CategoriaValidar, CadastroCategoriaView>(id) },
            { nameof(CadastroProdutoViewModel), (id) => GetCadastroView<CadastroProdutoViewModel, RProdutoDAL, ProdutoValidar, CadastroProdutoView>
                            (
                                id,
                                GetRepositorio<RCodigosDAL>(),
                                GetRepositorio<RCategoriaDAL>(),
                                GetRepositorio<REstoqueDAL>()
                            ) },
            { nameof(CadastroOrdemServicoViewModel), (id) => GetCadastroView<CadastroOrdemServicoViewModel, ROsDAL, OrdemServicoValidar, CadastroOrdemServicoView>
                            (
                                id,
                                new ItemOrdemServicoValidar(),
                                GetRepositorio<RCodigosDAL>(),
                                GetRepositorio<RItensOSDAL>()
                            ) },
            { nameof(CadastroVendaViewModel), (id) => GetCadastroView<CadastroVendaViewModel, RVendasDAL, VendaValidar, CadastroVendaView>(id, GetRepositorio<RCodigosDAL>(), GetRepositorio<RItensVendaDAL>(), new ItemVendaValidar()) },
        };

        public static Dictionary<string, Func<UserControl>> PesquisaViews = new Dictionary<string, Func<UserControl>>()
        {
            { nameof(PesquisaClienteViewModel), () => GetPesquisaView<PesquisaClienteViewModel, RClienteDAL>() },
            { nameof(PesquisaColaboradorViewModel), () => GetPesquisaView<PesquisaColaboradorViewModel, RColaboradorDAL>() },
            { nameof(PesquisaCategoriaViewModel), () => GetPesquisaView<PesquisaCategoriaViewModel, RCategoriaDAL>() },
            { nameof(PesquisaProdutoViewModel), () => GetPesquisaView<PesquisaProdutoViewModel, RProdutoDAL>() },
            { nameof(PesquisaOrdemServicoViewModel), () => GetPesquisaView<PesquisaOrdemServicoViewModel, ROsDAL>(GetRepositorio<RCodigosDAL>()) },
            { nameof(PesquisaVendaViewModel), () => GetPesquisaView<PesquisaVendaViewModel, RVendasDAL>(GetRepositorio<RCodigosDAL>()) },
        };

        public static Dictionary<string, Func<UserControl>> RelatoriosViews = new Dictionary<string, Func<UserControl>>()
        {
            { nameof(RelatorioProdutoMaisVendidoViewModel), () => new RelatorioProdutoMaisVendido(new RelatorioProdutoMaisVendidoViewModel(GetRepositorio<RRelatorioProdutoMaisVendidoDAL>())) },
            { nameof(RelatorioHistoricoVendasViewModel), () => new RelatorioHistoricoVenda(new RelatorioHistoricoVendasViewModel(GetRepositorio<RRelatorioHistoricoVendasDAL>())) },
            { nameof(RelatorioComissaoViewModel), () => new RelatorioComissao(new RelatorioComissaoViewModel(GetRepositorio<RRelatorioComissaoVendaDAL>())) },
            { nameof(RelatorioReciboOrdemServicoViewModel), () => new RelatorioReciboOs(new RelatorioReciboOrdemServicoViewModel(GetRepositorio<RRelatorioReciboDAL>())) },
            { nameof(RelatorioReciboVendaViewModel), () => new RelatorioReciboVenda(new RelatorioReciboVendaViewModel(GetRepositorio<RRelatorioReciboDAL>())) },
        };
    }
}
