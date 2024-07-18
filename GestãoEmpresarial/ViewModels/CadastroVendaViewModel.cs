using GestãoEmpresarial.Interface;
using GestãoEmpresarial.Models;
using GestãoEmpresarial.Repositorios;
using GestãoEmpresarial.Validations;
using MicroMvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GestãoEmpresarial.ViewModels
{
    internal class CadastroVendaViewModel : CadastroViewModel<VendaModel, EditarVendaModel>
    {
        private readonly RCodigosDAL _codigosDal;
        private readonly RItensVendaDAL _itemVendaDal;
        private readonly ItemVendaValidar _itemValidador;
        public Dictionary<int, string> TiposPagamento { get; internal set; }

        public ICommand AdicionarItemVendaCommand { get; set; }
        public ICommand ApagarItemVendaCommand { get; set; }

        public CadastroVendaViewModel(int? id, VendaValidar validar, IDAL<VendaModel> repositorio, RCodigosDAL codigosDAL, RItensVendaDAL itensVendaDAL, ItemVendaValidar itemVendaValidar) 
            : base(id, validar, repositorio)
        {
            AdicionarItemVendaCommand = new RelayCommandWithParameter(ExecutarGuardarItemNaLista, CanExecuteAdicionarItem);
            ApagarItemVendaCommand = new RelayCommandWithParameter(ExecutarApagarItemNaLista, CanExecuteApagarItem);

            _itemVendaDal = itensVendaDAL;
            _codigosDal = codigosDAL;
            _itemValidador = itemVendaValidar;

            if (id.HasValue)
            {
                itensVendaDAL.GetByIdVenda(id.Value);
            }
            TiposPagamento = codigosDAL.GetListaTiposPagamentos().ToDictionary(b => b.Id, a => a.Nome);
        }

        public bool CanExecuteApagarItem(object parameter)
        {
            return true;
            //return _codigosDal.PodeApagarItem(ObjectoEditar.Status);
        }

        public void ExecutarApagarItemNaLista(object tag)
        {
            var item = (ItemVendaModelObservavel)tag;
            if (item.IdVenda > 0)
            {
                var objBD = ItemVendaModelObservavel.MapearItemVendaModel(item);
                _itemVendaDal.Delete(objBD);
            }
            ObjectoEditar.RemoverDaLista(item);
        }

        public bool CanExecuteAdicionarItem(object parameter)
        {
            var objBD = ItemVendaModelObservavel.MapearItemVendaModel(ObjectoEditar.ItemVendaAdicionarPlanilha);
            var result = _itemValidador.Validate(objBD);
            return result.IsValid;
        }

        public void ExecutarGuardarItemNaLista(object tag)
        {
            ObjectoEditar.AdicionarNaLista();
        }
    }
}
