using GestãoEmpresarial.Validations;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace GestãoEmpresarial.Models
{
    public class EditarOsModel : EditarBaseModel<OrdemServicoModel>
    {
        public EditarOsModel(OrdemServicoValidar validar) : this(null, validar)
        {
            DataEntrada = DateTime.Now;
        }

        public EditarOsModel(OrdemServicoModel osModel, OrdemServicoValidar validar) : base(osModel, validar)
        {
            ItemOsAdicionarPlanilha = new ItensOrdemServicoModelObservavel();

            if (ListItensOs == null)
                ListItensOs = new ObservableCollection<ItensOrdemServicoModelObservavel>();

            NumberOfRecords = ListItensOs.Count;
            // Registre o evento CollectionChanged para atualizar o NumberOfRecords quando a coleção mudar
            ListItensOs.CollectionChanged += (sender, e) => { NumberOfRecords = ListItensOs.Count; };
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

        public decimal _TotalMaoObra;

        public decimal TotalMaoObra
        {
            get { return _TotalMaoObra; }
            set
            {
                _TotalMaoObra = value;
                RaisePropertyChanged(nameof(TotalMaoObra));
                RaisePropertyChanged(nameof(TotalOS));
            }
        }

        public decimal SubTotalProduto //total valor sem desconto
        {
            get
            {
                return ListItensOs.Sum(x => x.CustoTotal); //Func<ItensOsModel (parametro entrada), decimal (valor de saida)>
                //return ListItensOs.Sum(SomaDoTotalDosProdutos);
                //return ListItensOs.Sum(a => SomaDoTotalDosProdutos(a));
            }
        }

        //public decimal TotalItemOrdemServicoPlanilha
        //{
        //    get
        //    {
        //        if (ItemOsAdicionarPlanilha.Produto == null)
        //            return 0;
        //        var CustoTotal = ItemOsAdicionarPlanilha.Quantidade * ItemOsAdicionarPlanilha.Produto.ValorVenda;
        //        var DescontoValor = ItemOsAdicionarPlanilha.Desconto <= 0 ? 0 : CustoTotal * ItemOsAdicionarPlanilha.Desconto / 100;
        //        return Math.Round(CustoTotal - DescontoValor, 2);
        //    }
        //}

        public decimal TotalOS { get { return TotalProduto + TotalMaoObra; } }

        public decimal TotalDescontoProduto { get { return Math.Round(ListItensOs.Sum(x => x.DescontoValor), 2); } }

        public decimal TotalProduto { get { return ListItensOs.Sum(x => x.TotalItem); } } //total valor com desconto

        public ItensOrdemServicoModelObservavel ItemOsAdicionarPlanilha { get; set; }
        public ObservableCollection<ItensOrdemServicoModelObservavel> ListItensOs { get; set; }

        public void AdicionarNaLista()
        {
            ListItensOs.Add(ItemOsAdicionarPlanilha);
            ItemOsAdicionarPlanilha = new ItensOrdemServicoModelObservavel();
            RaisePropertyChanged(nameof(ItemOsAdicionarPlanilha));
            AtualizarTotais();
        }

        public void RemoverDaLista(ItensOrdemServicoModelObservavel item)
        {
            ListItensOs.Remove(item);
            AtualizarTotais();
        }

        private void AtualizarTotais()
        {
            RaisePropertyChanged(nameof(TotalDescontoProduto));
            //Aqui estamos a dizer, que no "ObjectoEditar" a propriedade SubtotalProduto foi alterada
            //E como tal o ecrã deve atualizar o seu valor
            RaisePropertyChanged(nameof(SubTotalProduto));
            RaisePropertyChanged(nameof(TotalProduto));
            RaisePropertyChanged(nameof(TotalOS));
        }

        public override OrdemServicoModel DevolveObjectoBD()
        {
            return new OrdemServicoModel
            {
                TotalOS = TotalOS,
                TotalMaoObra = TotalMaoObra,
                TotalDescontoProduto = TotalDescontoProduto,
                TotalProduto = TotalProduto,
                SubTotalProduto = SubTotalProduto,
                IdOs = IdOs,
                Status = Status,
                Finalizado = Finalizado,
                Box = Box,
                Marca = Marca,
                Modelo = Modelo,
                Ferramenta = Ferramenta,
                DataEntrada = DataEntrada,
                DataFinalizacao = DataFinalizacao,
                Responsavel = Responsavel,
                Tecnico = Tecnico,
                Obs = Obs,
                Garantia = Garantia,
                Cliente = Cliente,
            };
        }

        protected override void SetPropriedadesDoObjectoBD(OrdemServicoModel obj)
        {
            var listDeObservaveis = obj.ListItensOs.Select(a => ItensOrdemServicoModelObservavel.MapearItemOrdemServicoModel(a));
            ListItensOs = new ObservableCollection<ItensOrdemServicoModelObservavel>(listDeObservaveis);
            TotalMaoObra = obj.TotalMaoObra;
            Status = obj.Status;
            IdOs = obj.IdOs;
            Finalizado = obj.Finalizado;
            Box = obj.Box;
            Marca = obj.Marca;
            Modelo = obj.Modelo;
            Ferramenta = obj.Ferramenta;
            DataEntrada = obj.DataEntrada;
            DataFinalizacao = obj.DataFinalizacao;
            Responsavel = obj.Responsavel;
            Tecnico = obj.Tecnico;
            Obs = obj.Obs;
            Garantia = obj.Garantia;
            Cliente = obj.Cliente;
        }

        public ColaboradorModel Responsavel { get; set; }
        public ColaboradorModel Tecnico { get; set; }
        public ClienteModel Cliente { get; set; }
        public int IdOs { get; set; }
        public bool Finalizado { get; set; }
        public int Status { get; set; }
        public string Box { get; set; }
        public int Marca { get; set; }
        public string Modelo { get; set; }
        public string Ferramenta { get; set; }
        public DateTime DataEntrada { get; set; }
        public DateTime? DataFinalizacao { get; set; }
        public string Obs { get; set; }
        public bool Garantia { get; set; }
    }
}