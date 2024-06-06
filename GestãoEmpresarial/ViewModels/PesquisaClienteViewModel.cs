using GestãoEmpresarial.Interface;
using GestãoEmpresarial.Models;

namespace GestãoEmpresarial.ViewModels
{
    internal class PesquisaClienteViewModel : PesquisaViewModel<ClienteModel, DataGridClienteModel>
    {
        public PesquisaClienteViewModel(IDAL<ClienteModel> Repositorio) : base(Repositorio)
        {
        }

        public override DataGridClienteModel GetDataGridModel(ClienteModel item)
        {
            return new DataGridClienteModel(item);
        }
    }
}