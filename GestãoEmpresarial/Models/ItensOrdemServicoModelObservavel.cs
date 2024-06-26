using GestãoEmpresarial.Models.Atributos;
using MicroMvvm;
using System;

namespace GestãoEmpresarial.Models
{
    public class ItensOrdemServicoModelObservavel : ObservableObject
    {
        public static ItemOrdemServicoModel MapearItemOrdemServicoModel(ItensOrdemServicoModelObservavel model)
        {
            return new ItemOrdemServicoModel()
            {
                CustoTotal = model.CustoTotal,
                Desconto = model.Desconto,
                IdItensOs = model.IdItensOs,
                IdOs = model.IdOs,
                Produto = model.Produto,
                Quantidade = model.Quantidade,
                ValUnitario = model.ValUnitario
            };
        }

        public static ItensOrdemServicoModelObservavel MapearItemOrdemServicoModel(ItemOrdemServicoModel model)
        {
            return new ItensOrdemServicoModelObservavel()
            {
                Desconto = model.Desconto,
                IdItensOs = model.IdItensOs,
                IdOs = model.IdOs,
                Produto = model.Produto,
                Quantidade = model.Quantidade,
                ValUnitario = model.ValUnitario
            };
        }

        public int IdOs { get; set; }
        public int IdItensOs { get; set; }
        private int _quantidade;

        [ColumnHeader("Nome Produto")]
        public string NomeProduto
        {
            get { return _produto?.Nome; }
        }

        [ColumnHeader("Código")]
        public string CodigoProduto
        {
            get { return _produto?.CodProduto; }
        }

        [ColumnHeader("Quantidade")]
        public int Quantidade
        {
            get { return _quantidade; }
            set
            {
                _quantidade = value;
                RaisePropertyChanged(nameof(Quantidade));
                RaisePropertyChanged(nameof(TotalItem));
            }
        }

        private decimal _valUnitario;

        [ColumnHeader("Valor Unitario")]
        public decimal ValUnitario
        {
            get { return _valUnitario; }
            set
            {
                _valUnitario = value;
                RaisePropertyChanged(nameof(ValUnitario));
                RaisePropertyChanged(nameof(TotalItem));
            }
        }

        public decimal _Desconto;

        [ColumnHeader("Desconto")]
        public decimal Desconto
        {
            get { return _Desconto; }
            set
            {
                _Desconto = value;
                RaisePropertyChanged(nameof(Desconto));
                RaisePropertyChanged(nameof(TotalItem));
            }
        }

        public decimal CustoTotal
        { get { return Quantidade * ValUnitario; } }

        public decimal DescontoValor
        { get { return Desconto <= 0 ? 0 : CustoTotal * Desconto / 100; } }

        public string Acao { get; set; }

        [ColumnHeader("Total Item")]
        public decimal TotalItem
        { get { return Math.Round(CustoTotal - DescontoValor, 2); } }

        private ProdutoModel _produto;

        public ProdutoModel Produto
        {
            get { return _produto; }
            set
            {
                _produto = value;
                if (_produto != null && ValUnitario == 0)
                    ValUnitario = _produto.ValorVenda;
                RaisePropertyChanged(nameof(Produto));
            }
        }
    }
}