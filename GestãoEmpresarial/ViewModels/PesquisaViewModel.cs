using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestãoEmpresarial.ViewModels
{
    /// <summary>
    /// Classe abstracta, que permite os seus filhos reimplementarem os seu metodos
    /// Esta classe permite estipular um tipo generico, para que seja feito o binding na datagrid
    /// </summary>
    public abstract class PesquisaViewModel<TDataGridModel>
    {
        /// <summary>
        /// Lista Filtrada - Binding só funcionar com property (propriedade) -> (get;set;)
        /// E o binding desta lista está na datagrid (da FiltroBaseView)
        /// </summary>
        public ObservableCollection<TDataGridModel> ListaDaGrid { get; set; }

        /// <summary>
        /// O objecto (do tipo generico que for escolhido no filho) que faz binding
        /// com a linha selecionada da datagrid
        /// </summary>
        public TDataGridModel ObjectoSelecionado { get; set; }
    }
}
