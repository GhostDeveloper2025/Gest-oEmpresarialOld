using GestãoEmpresarial.Interface;
using GestãoEmpresarial.Models;
using GestãoEmpresarial.Repositorios;
using GestãoEmpresarial.Validations;
using GestãoEmpresarial.Views.Cadastro;
using GestãoEmpresarial.Views.Pesquisa;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;

namespace GestãoEmpresarial.ViewModels
{
    public class MenuViewModel
    {
        public ObservableCollection<TreeviewMenu> Items { get; set; }

        private static TRepositorio GetRepositorio<TRepositorio>()
        where TRepositorio : class
        {
            return Activator.CreateInstance(typeof(TRepositorio), LoginViewModel.colaborador.IdFuncionario) as TRepositorio;
        }

        public MenuViewModel()
        {
            Items = new ObservableCollection<TreeviewMenu>()
    {
        new TreeviewMenu
        {
            Header = "Cadastros",
            Icon = PackIconKind.FolderPlus,
            Items = new TreeviewMenuCollection
            {
                { "Cliente", PackIconKind.AccountMultiplePlus, DI.GetCadastroView(nameof(CadastroClienteViewModel)) },
                { "Colaborador", PackIconKind.PersonChild, DI.GetCadastroView(nameof(CadastroColaboradorViewModel)) },
                { "Categoria", PackIconKind.TagMultiple, DI.GetCadastroView(nameof(CadastroCategoriaViewModel)) },
                { "Produto", PackIconKind.PackageCheck, DI.GetCadastroView(nameof(CadastroProdutoViewModel)) },
                { "OS", PackIconKind.HammerScrewdriver, DI.GetCadastroView(nameof(CadastroOrdemServicoViewModel)) },
                { "Venda", PackIconKind.CartArrowRight, DI.GetCadastroView(nameof(CadastroVendaViewModel)) },
            }
        },
        new TreeviewMenu
        {
            Header = "Pesquisar",
            Icon = PackIconKind.FileSearch,
            Items = new TreeviewMenuCollection
            {
                { "Cliente", PackIconKind.AccountSearch, DI.PesquisaViews[nameof(PesquisaClienteViewModel)] },
                { "Colaborador", PackIconKind.AccountSearch, DI.PesquisaViews[nameof(PesquisaColaboradorViewModel)] },
                { "Categoria", PackIconKind.TagSearch, DI.PesquisaViews[nameof(PesquisaCategoriaViewModel)] },
                { "Produto", PackIconKind.Package, DI.PesquisaViews[nameof(PesquisaProdutoViewModel)] },
                { "OS", PackIconKind.HammerWrench, DI.PesquisaViews[nameof(PesquisaOrdemServicoViewModel)] },
                { "Venda", PackIconKind.Cart, DI.PesquisaViews[nameof(PesquisaVendaViewModel)] },
            }
        },
        new TreeviewMenu
     {
         Header = "Relatórios",
         Icon = PackIconKind.ReportBox,
         Items = new TreeviewMenuCollection
         {
             
             { "Historico Vendas", PackIconKind.PersonAdd ,  () => DI.RelatoriosViews[nameof(RelatorioHistoricoVendasViewModel)]() },
             { "Venda de Produtos", PackIconKind.PersonAdd ,  () => DI.RelatoriosViews[nameof(RelatorioProdutoMaisVendidoViewModel)]() },
             { "Comissão", PackIconKind.PersonAdd ,  () => DI.RelatoriosViews[nameof(RelatorioComissaoViewModel)]() },
             { "Ordem Serviço", PackIconKind.PersonAdd ,  () => null },
         }
     }
        };
        }
    }
}