using GestãoEmpresarial.Validations;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace GestãoEmpresarial.Models
{
    /// <summary>
    /// Este objecto é o que faz bind na View.
    /// </summary>
    public class EditarVendaModel : EditarBaseModel<VendaModel>
    {
        public EditarVendaModel(VendaValidar validar) : this(null, validar)
        {
            DataVenda = DateTime.Now;
            //DataFinalizacao = DateTime.Now;
        }

        public EditarVendaModel(VendaModel vendaModel, VendaValidar validar) : base(vendaModel, validar)
        {
            ItemVendaAdicionarPlanilha = new ItemVendaModelObservavel();
            if (ListItensVenda == null)
                ListItensVenda = new ObservableCollection<ItemVendaModelObservavel>();

            NumberOfRecords = ListItensVenda.Count;
            // Registre o evento CollectionChanged para atualizar o NumberOfRecords quando a coleção mudar
            ListItensVenda.CollectionChanged += (sender, e) => { NumberOfRecords = ListItensVenda.Count; };
        }

        public int IdVenda { get; set; }
        public bool Situacao { get; set; }
        public int IdCodigoTipoPagamento { get; set; }
        public DateTime DataVenda { get; set; }
        public DateTime? DataFinalizacao { get; set; }

        public ClienteModel Cliente { get; set; }

        public bool Cancelada { get; set; } // Nova propriedade

        public ItemVendaModelObservavel ItemVendaAdicionarPlanilha { get; set; }
        public ObservableCollection<ItemVendaModelObservavel> ListItensVenda { get; set; }

        public void AdicionarNaLista()
        {
            ListItensVenda.Add(ItemVendaAdicionarPlanilha);
            ItemVendaAdicionarPlanilha = new ItemVendaModelObservavel();
            RaisePropertyChanged(nameof(ItemVendaAdicionarPlanilha));
            AtualizarTotais();
        }
        public void RemoverDaLista(ItemVendaModelObservavel item)
        {
            ListItensVenda.Remove(item);
            AtualizarTotais();
        }

        private void AtualizarTotais()
        {
            RaisePropertyChanged(nameof(TotalDescontoProduto));
            //Aqui estamos a dizer, que no "ObjectoEditar" a propriedade SubtotalProduto foi alterada
            //E como tal o ecrã deve atualizar o seu valor
            RaisePropertyChanged(nameof(SubTotalProduto));
            RaisePropertyChanged(nameof(TotalProduto));
            RaisePropertyChanged(nameof(TotalVenda));
        }

        // Variável privada para armazenar o número de registros
        private int _numberOfRecords;

        public int NumberOfRecords
        {
            get { return _numberOfRecords; } // Obtém o número de registros
            set
            {
                _numberOfRecords = value; // Define o valor da variável privada
                RaisePropertyChanged(nameof(NumberOfRecords)); // Notifica a View sobre a mudança na propriedade NumberOfRecords
            }
        }

        public decimal _ValorFrete;

        public decimal ValorFrete
        {
            get { return _ValorFrete; }
            set
            {
                _ValorFrete = value;
                RaisePropertyChanged(nameof(ValorFrete));
                RaisePropertyChanged(nameof(TotalVenda));
            }
        }

        //total valor sem desconto
        //public decimal SubTotalProduto { get { return ListItensVenda.Sum(x => x.CustoTotal); } }

        //public decimal TotalVenda { get { return TotalProduto + ValorFrete; } }

        //public decimal TotalDescontoProduto { get { return Math.Round(ListItensVenda.Sum(x => x.DescontoValor), 2); } }

        //public decimal TotalProduto { get { return ListItensVenda.Sum(x => x.TotalItem); } } //total valor com desconto

        // Na classe EditarVendaModel, modifique as propriedades de cálculo:

        public decimal SubTotalProduto
        {
            get
            {
                return Math.Round(ListItensVenda.Sum(x => x.CustoTotal), 2, MidpointRounding.AwayFromZero);
            }
        }
        public decimal TotalDescontoProduto
        {
            get
            {
                return Math.Round(ListItensVenda.Sum(x => x.DescontoValor), 2, MidpointRounding.AwayFromZero);
            }
        }

        public decimal TotalProduto
        {
            get
            {
                return Math.Round(ListItensVenda.Sum(x => x.TotalItem), 2, MidpointRounding.AwayFromZero);
            }
        }

        public decimal TotalVenda
        {
            get
            {
                return Math.Round(TotalProduto + ValorFrete, 2, MidpointRounding.AwayFromZero);
            }
        }

        public override VendaModel DevolveObjectoBD()
        {
            return new VendaModel()
            {
                DataFinalizacao = DataFinalizacao,
                IdVenda = IdVenda,
                Situacao = Situacao,
                Cliente = Cliente,
                ValorFrete = ValorFrete,
                IdCodigoTipoPagamento = IdCodigoTipoPagamento,
                DataVenda = DataVenda,
                Cancelada = Cancelada, // Mapeia a nova propriedade
            };
        }

        protected override void SetPropriedadesDoObjectoBD(VendaModel obj)
        {
            var listDeObservaveis = obj.ListItensVenda.Select(a => ItemVendaModelObservavel.MapearItemVendaModel(a));
            ListItensVenda = new ObservableCollection<ItemVendaModelObservavel>(listDeObservaveis);
            IdVenda = obj.IdVenda;
            Situacao = obj.Situacao;
            DataFinalizacao = obj.DataFinalizacao;
            Cliente = obj.Cliente;
            ValorFrete = obj.ValorFrete;
            IdCodigoTipoPagamento = obj.IdCodigoTipoPagamento;
            Cancelada = obj.Cancelada; // Mapeia a nova propriedade
        }
    }
}