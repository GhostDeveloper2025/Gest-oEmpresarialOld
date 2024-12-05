using GestãoEmpresarial.Repositorios;
using GestãoEmpresarial.Validations;
using GestãoEmpresarial.ViewModels;
using GestãoEmpresarial.Views;
using GestãoEmpresarial.Views.Cadastro;
using GestãoEmpresarial.Views.Layout;
using GestãoEmpresarial.Views.Login;
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
        internal static TRepositorio GetRepositorio<TRepositorio>()
        where TRepositorio : class
        {
            return Activator.CreateInstance(typeof(TRepositorio), LoginViewModel.Instancia.colaborador.IdFuncionario) as TRepositorio;
        }
        /// <summary>
        /// Retorna a tela completa (CadastroView), pronta para ser exibida e manipulada
        /// com todos os dados e validações configurados.
        /// </summary>
        /// <typeparam name="TViewModel"></typeparam>
        /// <typeparam name="TRepositorio"></typeparam>
        /// <typeparam name="TValidar"></typeparam>
        /// <typeparam name="TView"></typeparam>
        /// <param name="id"></param>
        /// <param name="args"></param>
        /// <returns></returns>
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
        /// <summary>
        /// Retorna a tela completa (CadastrosVendaOsView), pronta para ser exibida e manipulada
        /// com todos os dados e validações configurados.
        /// </summary>
        /// <typeparam name="TViewModel"></typeparam>
        /// <typeparam name="TRepositorio"></typeparam>
        /// <typeparam name="TValidar"></typeparam>
        /// <typeparam name="TView"></typeparam>
        /// <param name="id"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        private static CadastroGlobalView GetCadastroGlobalView<TViewModel, TRepositorio, TValidar, TView>(int? id, params object[] args)
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
            return new CadastroGlobalView(viewModel, view);
        }
        //Gera e devolve as Views de cadastro correta para Vendas ou Ordem de Serviço com base no nome fornecido.
        public static Func<UserControl> GetCadastroGlobalView(string name)
        {
            return () => CadastrosViews[name](null);
        }
        ///
        /// <summary>
        /// Retorna a tela completa (PesquisaView), pronta para ser exibida e manipulada
        /// com todos os dados e validações configurados.
        /// </summary>
        /// <typeparam name="TViewModel"></typeparam>
        /// <typeparam name="TRepositorio"></typeparam>
        /// <typeparam name="TValidar"></typeparam>
        /// <typeparam name="TView"></typeparam>
        /// <param name="id"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        private static PesquisaView GetPesquisaView<TViewModel, TRepositorio>(UIElement barraPesquisa, params object[] args)
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
            return new PesquisaView(viewModel, barraPesquisa);
        }

        public static Func<UserControl> GetCadastroView(string name)
        {
            return () => CadastrosViews[name](null);
        }

        internal static UserControl GetRelatorioView(string v)
        {
            return RelatoriosViews[v]();
        }

        public static Dictionary<string, Func<UserControl>> PaginasView = new Dictionary<string, Func<UserControl>>
        {
            { nameof(LayoutView), () => new LayoutView()  },
            { nameof(LoginView),  () => new LoginView()  }
        };

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
            { nameof(CadastroOrdemServicoViewModel), (id) => GetCadastroGlobalView<CadastroOrdemServicoViewModel, ROsDAL, OrdemServicoValidar, CadastroOrdemServicoView>
                            (
                                id,
                                new ItemOrdemServicoValidar(),
                                GetRepositorio<RCodigosDAL>(),
                                GetRepositorio<RItensOSDAL>()
                            ) },
            { nameof(CadastroVendaViewModel), (id) => GetCadastroGlobalView<CadastroVendaViewModel, RVendasDAL, VendaValidar, CadastroVendaView>(id, GetRepositorio<RCodigosDAL>(), GetRepositorio<RItensVendaDAL>(), new ItemVendaValidar()) },
            { nameof(RelatorioReciboOrdemServicoViewModel), (id) => new RelatorioReciboOs(new RelatorioReciboOrdemServicoViewModel(GetRepositorio<RRelatorioReciboDAL>(), id.Value)) },
            { nameof(RelatorioReciboVendaViewModel), (id) => new RelatorioReciboVenda(new RelatorioReciboVendaViewModel(GetRepositorio<RRelatorioReciboDAL>(), GetRepositorio<RClienteDAL>(), id.Value)) },
        };

        public static Dictionary<string, Func<UserControl>> PesquisaViews = new Dictionary<string, Func<UserControl>>()
        {
            { nameof(PesquisaClienteViewModel), () => GetPesquisaView<PesquisaClienteViewModel, RClienteDAL>(new BarraPesquisaClienteView()) },
            { nameof(PesquisaColaboradorViewModel), () => GetPesquisaView<PesquisaColaboradorViewModel, RColaboradorDAL>(new PesquisaBarraView()) },
            { nameof(PesquisaCategoriaViewModel), () => GetPesquisaView<PesquisaCategoriaViewModel, RCategoriaDAL>(new PesquisaBarraView()) },
            { nameof(PesquisaProdutoViewModel), () => GetPesquisaView<PesquisaProdutoViewModel, RProdutoDAL>(new BarraPesquisaProdutoView(), GetRepositorio<RCodigosDAL>()) },
            { nameof(PesquisaOrdemServicoViewModel), () => GetPesquisaView<PesquisaOrdemServicoViewModel, ROsDAL>(new BarraPesquisaOrdemServicoView(), GetRepositorio<RCodigosDAL>()) },
            { nameof(PesquisaVendaViewModel), () => GetPesquisaView<PesquisaVendaViewModel, RVendasDAL>(new BarraPesquisaVendaView(), GetRepositorio<RCodigosDAL>()) },
        };

        public static Dictionary<string, Func<UserControl>> RelatoriosViews = new Dictionary<string, Func<UserControl>>()
        {
            { nameof(RelatorioProdutoMaisVendidoViewModel), () => new RelatorioProdutoMaisVendido(new RelatorioProdutoMaisVendidoViewModel(GetRepositorio<RRelatorioProdutoMaisVendidoDAL>())) },
            { nameof(RelatorioHistoricoVendasViewModel), () => new RelatorioHistoricoVenda(new RelatorioHistoricoVendasViewModel(GetRepositorio<RRelatorioHistoricoVendasDAL>())) },
            { nameof(RelatorioComissaoViewModel), () => new RelatorioComissao(new RelatorioComissaoViewModel(GetRepositorio<RRelatorioComissaoVendaDAL>())) },
        };
    }
}
