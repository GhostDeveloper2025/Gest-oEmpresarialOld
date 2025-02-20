using GestãoEmpresarial.Interface;
using GestãoEmpresarial.Utils;
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
    /// Classe abstracta, que permite os seus filhos reimplementarem os seus métodos
    /// Esta classe permite estipular um tipo genérico, para que seja feito o binding na DataGrid
    /// </summary>
    public abstract class PesquisaViewModel<TObjectoBD, TDataGridModel, TRepositorio> : ObservableObject, IPesquisaViewModel
        where TRepositorio : IDAL<TObjectoBD>
    {
        /// <summary>
        /// Lista Filtrada - Binding só funcionar com property (propriedade) -> (get;set;)
        /// E o binding desta lista está na DataGrid (da FiltroBaseView)
        /// </summary>
        private List<TDataGridModel> _listaDaGrid;
        public List<TDataGridModel> ListaDaGrid
        {
            get => new List<TDataGridModel>(_listaDaGrid);
        }

        /// <summary>
        /// O objeto (do tipo genérico que for escolhido no filho) que faz binding
        /// com a linha selecionada da DataGrid
        /// </summary>
        public TDataGridModel ObjectoSelecionado { get; set; }

        /// <summary>
        /// Mostra a quantidade de registros do DataGrid
        /// </summary>
        private int _numberOfRecords;
        public int NumberOfRecords
        {
            get { return _numberOfRecords; } // Obtém o número de registros
            set
            {
                _numberOfRecords = value; // Define o valor da variável privada
                RaisePropertyChanged(nameof(NumberOfRecords)); // Notifica a View sobre a mudança
            }
        }

        private bool _estaPensando;
        public bool EstaPensando
        {
            get { return _estaPensando; }
            set
            {
                _estaPensando = value;
                RaisePropertyChanged(nameof(EstaPensando));
            }
        }

        public ICommand PesquisarCommand { get; set; }
        public ICommand RemoverCommand { get; set; }
        public abstract int Id { get; }
        public abstract string NomeEditarView { get; }

        // O readonly só permite atribuir valor dentro do construtor.
        protected readonly TRepositorio Repositorio;

        // Campo privado que armazena o estado de foco do TextBox.
        // Inicializa como true para que o TextBox receba foco assim que a View for carregada.
        private bool _isTextBoxFocused = true;

        // Propriedade pública que permite o binding com a View (XAML). Neste Caso Para adicionar um foco no TextBox
        // Essa propriedade é observada pela View, e qualquer mudança nela reflete no controle associado.
        public bool IsTextBoxFocused
        {
            get => _isTextBoxFocused;
            set
            {
                _isTextBoxFocused = value;
                RaisePropertyChanged(nameof(IsTextBoxFocused));
            }
        }
        public PesquisaViewModel(TRepositorio repositorio)
        {
            Repositorio = repositorio;
            RemoverCommand = new RelayCommandWithParameterAsync(ExecutarRemoverAsync, PodeExecutarRemover);
            PesquisarCommand = new RelayCommandWithParameterAsync(ExecutarPesquisarAsync, PodeExecutarPesquisar);
            _listaDaGrid = new List<TDataGridModel>();
        }

        public virtual bool PodeExecutarRemover(object parameter)
        {
            return true;
        }

        public virtual async Task ExecutarRemoverAsync(object parameter)
        {
            // Implementar a lógica de remoção específica no ViewModel filho
        }
        public abstract TDataGridModel GetDataGridModel(TObjectoBD item);

        public abstract List<TObjectoBD> GetLista();

        /// <summary>
        /// Método virtual onde é possível que o filho altere o conteúdo do método.
        /// Só é usado no filho caso queiramos algum tipo de validação adicional.
        /// A única validação feita é "IsNullOrWhiteSpace(FiltroGlobal)"
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public abstract bool PodeExecutarPesquisar(object parameter);

        /// <summary>
        /// Método virtual que os filhos podem fazer override e ter a sua pesquisa específica
        /// </summary>
        /// <param name="parameter"></param>
        public async Task ExecutarPesquisarAsync(object parameter)
        {
            EstaPensando = true;
            try
            {
                await Task.Run(() =>
                {
                    _listaDaGrid.Clear();

                    var listaDaBd = GetLista(); // Obtém a lista de forma assíncrona
                    foreach (var item in listaDaBd)
                    {
                        if (typeof(TObjectoBD) != typeof(TDataGridModel))
                        {
                            var obj = GetDataGridModel(item);
                            _listaDaGrid.Add(obj);
                        }
                        else
                        {
                            var obj = (TDataGridModel)Convert.ChangeType(item, typeof(TDataGridModel));
                            _listaDaGrid.Add(obj);
                        }
                    }

                    NumberOfRecords = _listaDaGrid.Count;
                    RaisePropertyChanged(nameof(ListaDaGrid));
                });
            }
            finally
            {
                EstaPensando = false;
            }
        }
    }
}

