using GestãoEmpresarial.Models;
using GestãoEmpresarial.Repositorios;
using MicroMvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestãoEmpresarial.ViewModels
{
    public class RelatorioReciboVendaViewModel : ObservableObject
    {
        private readonly RRelatorioReciboDAL _relatorioReciboDAL;
        private readonly RClienteDAL _rClienteDAL;

        public RelatorioReciboVendaViewModel(RRelatorioReciboDAL relatorioReciboDAL, RClienteDAL rClienteDAL, int id)
        {
            _relatorioReciboDAL = relatorioReciboDAL;
            _rClienteDAL = rClienteDAL;

            // Carregar os dados de forma assíncrona
            CarregarDadosAsync(id);
        }

        private async void CarregarDadosAsync(int id)
        {
            Relatorio = await _relatorioReciboDAL.ObterReciboVendaAsync(id);
            Cliente = await _rClienteDAL.GetByIdAsync(Relatorio.VendaModel.IdCliente);

            RaisePropertyChanged(nameof(Relatorio));
            RaisePropertyChanged(nameof(Cliente));
            RaisePropertyChanged(nameof(ListItemsObservaveis));
            RaisePropertyChanged(nameof(TotalProdutos));
            RaisePropertyChanged(nameof(SubTotalProdutos));
            RaisePropertyChanged(nameof(TotalDescontoProdutos));
            RaisePropertyChanged(nameof(TotalVendas));
            RaisePropertyChanged(nameof(ValorFretes));
            RaisePropertyChanged(nameof(NomeSituacao));
        }

        public ClienteModel Cliente { get; private set; }

        public RelatorioReciboVendaModel Relatorio { get; private set; }

        public ObservableCollection<ItemVendaModelObservavel> ListItemsObservaveis
        {
            get
            {
                return new ObservableCollection<ItemVendaModelObservavel>(
                    Relatorio?.VendaModel?.ListItensVenda?.Select(a => ItemVendaModelObservavel.MapearItemVendaModel(a)) ?? Enumerable.Empty<ItemVendaModelObservavel>()
                );
            }
        }

        private decimal TotalProduto => ListItemsObservaveis.Sum(x => x.TotalItem);

        private decimal SubTotalProduto => Relatorio?.VendaModel?.ListItensVenda?.Sum(x => x.CustoTotal) ?? 0;

        private decimal TotalDescontoProduto => Math.Round(Relatorio?.VendaModel?.ListItensVenda?.Sum(x => x.Desconto) ?? 0, 2);

        private decimal TotalVenda => TotalProduto + (Relatorio?.VendaModel?.ValorFrete ?? 0);

        public string TotalProdutos => FormatarValor("Total Produto", TotalProduto);

        public string SubTotalProdutos => FormatarValor("Subtotal Produto", SubTotalProduto);

        public string TotalVendas => FormatarValor("Total Venda", TotalVenda);

        public string TotalDescontoProdutos => FormatarValorPercentual("Total Desconto Produto", TotalDescontoProduto);

        public string ValorFretes => FormatarValor("Frete", Relatorio?.VendaModel?.ValorFrete ?? 0);

        public string NomeSituacao => Relatorio?.VendaModel?.Situacao == true ? "FINALIZADO" : "VENDA SALVA";

        // Função de formatação de valores monetários
        private string FormatarValor(string descricao, decimal valor)
        {
            return $"{descricao}: {valor:C}";
        }

        // Função de formatação de valores percentuais
        private string FormatarValorPercentual(string descricao, decimal valor)
        {
            return $"{descricao}: {(valor / 100):P2}";
        }
    }
}

