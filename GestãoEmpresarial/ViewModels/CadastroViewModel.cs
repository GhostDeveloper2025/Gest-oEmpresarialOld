using FluentValidation;
using GestãoEmpresarial.Interface;
using GestãoEmpresarial.Models;
using MicroMvvm;
using System;
using System.Windows;
using System.Windows.Input;

namespace GestãoEmpresarial.ViewModels
{
    /// <summary>
    /// Vai servir de base (pai) para as classes viewmodel especificas do cadastro.
    /// ViewModel é colocado no DataContext da View.
    /// </summary>
    public class CadastroViewModel<ObjectBD, ObjectoEditarView> : ObservableObject, ICadastroViewModel
        where ObjectoEditarView : EditarBaseModel<ObjectBD>
    {
        /// <summary>
        /// Propriedade que faz bind para saber se pode editar ou inserir
        /// </summary>
        private readonly bool _podeInserir;

        private readonly AbstractValidator<ObjectBD> _validador;
        protected readonly IDAL<ObjectBD> _repositorio;

        public CadastroViewModel(int? id, AbstractValidator<ObjectBD> validador, IDAL<ObjectBD> repositorio)
        {
            _validador = validador;
            _repositorio = repositorio;
            _podeInserir = id.HasValue == false;

            SaveCommand = new RelayCommandWithParameter(ExecutarSalvar);
            ObjectoEditar = NovoObjectoEditar(id);
        }
        public virtual ObjectoEditarView NovoObjectoEditar(int? id)
        {
            Type tipoEditar = typeof(ObjectoEditarView);
            if (id.HasValue)
                return Activator.CreateInstance(tipoEditar, _repositorio.GetById(id.Value), _validador) as ObjectoEditarView;
            else
                //criamos um novo model
                return Activator.CreateInstance(tipoEditar, _validador) as ObjectoEditarView;
        }

        public int Id { get; set; }
        public DateTime DataCadastro { get; set; } = DateTime.Now;

        /// <summary>
        /// Este objecto é usado para editar (é feito o bind nele), e posteriormente para guardar as alterações.
        /// </summary>
        public ObjectoEditarView ObjectoEditar { get; set; }

        /// <summary>
        /// Esta propriedade é a que faz bind (vinculo) ao botão Salvar.
        /// </summary>
        public ICommand SaveCommand { get; set; }

        public virtual int InserirObjectoBD()
        {
            var objBD = ObjectoEditar.DevolveObjectoBD();
            return _repositorio.Insert(objBD);
        }

        public virtual void AtualizarObjectoBD()
        {
            var objBD = ObjectoEditar.DevolveObjectoBD();
            _repositorio.Update(objBD);
        }

        public void ExecutarSalvar(object parameter)
        {
            try
            {
                var text = "Atualizado";
                if (_podeInserir)
                {
                    InserirObjectoBD();
                    text = "Adicionado";
                }
                else
                    AtualizarObjectoBD();

                MessageBox.Show($" Registo {text} Com Sucesso!", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);

                //limpa os Campos
                ObjectoEditar = NovoObjectoEditar(null);
                RaisePropertyChanged(nameof(ObjectoEditar));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Registo Não Pode Ser Salvo", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}