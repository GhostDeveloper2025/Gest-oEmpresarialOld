using GestãoEmpresarial.Interface;
using MicroMvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Navigation;

namespace GestãoEmpresarial.ViewModels
{
    /// <summary>
    /// Classe abstracta, que permite os seus filhos reimplementarem os seu metodos
    /// Esta classe permite estipular um tipo generico, para que seja feito o binding na datagrid
    /// </summary>
    public abstract class PesquisaViewModel<TObjectoBD, TDataGridModel> : ObservableObject, IPesquisaViewModel
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

        /// <summary>
        /// Esta propriedade (porque tem set e get), é a que faz o vinculo na view.
        /// Como tal os filhos tem de usar esta prop dentro do metodo "ExecutarPesquisar"
        /// </summary>
        public string FiltroGlobal { get; set; }

        /// <summary>
        /// Mostra a quantidade de resgistos do datagrid
        /// </summary>
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

        public ICommand PesquisarCommand { get; set; }
        public ICommand RemoverCommand { get; set; }

        // O readonly apenas podes colocar uma valor na variavel dentro do construtor.
        protected readonly IDAL<TObjectoBD> Repositorio;

        public PesquisaViewModel(IDAL<TObjectoBD> Repositorio)
        {
            this.Repositorio = Repositorio;
            RemoverCommand = new RelayCommandWithParameter(ExecutarRemover, PodeExecutarRemover);
            PesquisarCommand = new RelayCommandWithParameter(ExecutarPesquisar, PodeExecutarPesquisar);
            ListaDaGrid = new ObservableCollection<TDataGridModel>();
        }

        public virtual bool PodeExecutarRemover(object parameter)
        {
            return true;
        }

        public virtual void ExecutarRemover(object parameter)
        {
        }

        public virtual List<TObjectoBD> GetLista()
        {
            return Repositorio.List(FiltroGlobal);
        }

        public virtual TDataGridModel GetDataGridModel(TObjectoBD item)
        {
            return (TDataGridModel)Activator.CreateInstance(typeof(TDataGridModel), item);
        }

        /// <summary>
        /// metodo virtual onde é possivel que o filho altere o conteudo do metodo
        /// Só é usado no filho caso queiramos algum tipo de validação adicional
        /// A unica validação feita é "IsNullOrWhiteSpace(FiltroGlobal)"
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public virtual bool PodeExecutarPesquisar(object parameter)
        {
            if (string.IsNullOrWhiteSpace(FiltroGlobal))
                return false;
            return true;
        }

        /// <summary>
        /// metodo virtual que os filhos podem fazer override e ter a sua pesquisa especifica
        /// </summary>
        /// <param name="parameter"></param>
        public void ExecutarPesquisar(object parameter)
        {
            ListaDaGrid.Clear();
            if (string.IsNullOrWhiteSpace(FiltroGlobal) == false)
                FiltroGlobal = FiltroGlobal.Trim();

            var listaDaBd = GetLista();
            foreach (TObjectoBD item in listaDaBd)
            {
                if (typeof(TObjectoBD) != typeof(TDataGridModel))
                {
                    var obj = GetDataGridModel(item);
                    ListaDaGrid.Add(obj);
                }
                else
                {
                    var obj = (TDataGridModel)Convert.ChangeType(item, typeof(TDataGridModel));
                    ListaDaGrid.Add(obj);
                }
            }
            FiltroGlobal = null;
            RaisePropertyChanged(nameof(FiltroGlobal));
            // Atualize o NumberOfRecords com o novo valor
            NumberOfRecords = ListaDaGrid.Count;
        }

    }
}
