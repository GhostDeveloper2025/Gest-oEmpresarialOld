using GestãoEmpresarial.Interface;
using GestãoEmpresarial.Models;
using GestãoEmpresarial.Repositorios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestãoEmpresarial.ViewModels
{
    internal class PesquisaVendaViewModel : PesquisaViewModel<VendaModel, DataGridVendaModel>
    {
        public Dictionary<int, string> TipoPagamentoList { get; internal set; }

        public override int Id => ObjectoSelecionado.IdVenda;

        public override string NomeEditarView => nameof(CadastroVendaViewModel);

        public PesquisaVendaViewModel(IDAL<VendaModel> Repositorio, RCodigosDAL RepositorioCodigos) : base(Repositorio)
        {
            TipoPagamentoList = new Dictionary<int, string>
            {
                { 0, null }
            };
            foreach (var codigo in RepositorioCodigos.GetListaTiposPagamentos())
            {
                TipoPagamentoList.Add(codigo.Id, codigo.Nome);
            }
        }

        public override DataGridVendaModel GetDataGridModel(VendaModel item)
        {
            return new DataGridVendaModel(item, TipoPagamentoList);
        }
    }
}
