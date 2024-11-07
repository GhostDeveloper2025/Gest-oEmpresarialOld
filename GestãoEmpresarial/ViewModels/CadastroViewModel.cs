using FluentValidation;
using GestãoEmpresarial.Interface;
using GestãoEmpresarial.Models;
using MicroMvvm;
using System;
using System.Linq;
using System.Threading.Tasks;
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
            Id = id;

            //SaveCommand = new RelayCommandWithParameter(ExecutarSalvar, PodeExecutarSalvar);
            // Usando o novo comando assíncrono
            SaveCommand = new RelayCommandWithParameterAsync(ExecutarSalvar, PodeExecutarSalvar);
            ObjectoEditar = NovoObjectoEditar();
        }

        public int? Id { get; set; }

        public DateTime DataCadastro { get; set; } = DateTime.Now;
        public DateTime DataFinalizacao { get; set; }

        /// <summary>
        /// Este objecto é usado para editar (é feito o bind nele), e posteriormente para guardar as alterações.
        /// </summary>
        public ObjectoEditarView ObjectoEditar { get; set; }

        /// <summary>
        /// Esta propriedade é a que faz bind (vinculo) ao botão Salvar.
        /// </summary>
        public ICommand SaveCommand { get; set; }

        public virtual ObjectoEditarView NovoObjectoEditar()
        {
            Type tipoEditar = typeof(ObjectoEditarView);
            if (Id.HasValue)
                return Activator.CreateInstance(tipoEditar, _repositorio.GetById(Id.Value), _validador) as ObjectoEditarView;
            else
                //criamos um novo model
                return Activator.CreateInstance(tipoEditar, _validador) as ObjectoEditarView;
        }

        public virtual async Task<int> InserirObjectoBDAsync()
        {
            var objBD = ObjectoEditar.DevolveObjectoBD();
            return await _repositorio.InsertAsync(objBD);
        }

        public virtual async Task AtualizarObjectoBDAsync()
        {
            var objBD = ObjectoEditar.DevolveObjectoBD();
            await _repositorio.UpdateAsync(objBD);
        }

        //public bool PodeExecutarSalvar(object parameter)
        //{
        //    var objBD = ObjectoEditar.DevolveObjectoBD();
        //    var result = _validador.Validate(objBD);
        //    return result.IsValid;
        //}

        //Botao Sempe Habilitado Para Validaçao Com Menssagem!
        public bool PodeExecutarSalvar(object parameter)
        {
            // Mantenha este método para retorno 'true', permitindo que o botão Salvar sempre esteja habilitado
            return true;
        }
        #region codigo antigo de salvamento com validaçao no textbox
        //public virtual async Task ExecutarSalvar(object parameter)
        //{
        //    try
        //    {
        //        var text = "Atualizado";
        //        if (_podeInserir)
        //        {
        //            await InserirObjectoBDAsync();
        //            text = "Adicionado";
        //        }
        //        else
        //        {
        //            await AtualizarObjectoBDAsync();
        //        }

        //        MessageBox.Show($"Registro {text} com sucesso!", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);

        //        // Limpa os campos
        //        Id = null; // obrigamos a limpar
        //        ObjectoEditar = NovoObjectoEditar();
        //        RaisePropertyChanged(nameof(ObjectoEditar));
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message, "Registro Não Pode Ser Salvo", MessageBoxButton.OK, MessageBoxImage.Error);
        //    }
        //}
        #endregion

        //Salvar Com Menssagem de validaçao 
        public virtual async Task ExecutarSalvar(object parameter)
        {
            try
            {
                var objBD = ObjectoEditar.DevolveObjectoBD();
                var result = _validador.Validate(objBD);

                // Verifica se os campos obrigatórios estão preenchidos
                if (!result.IsValid)
                {
                    // Se não for válido, exibe a mensagem de erro e sai do método
                    var erros = string.Join(Environment.NewLine, result.Errors.Select(e => e.ErrorMessage));
                    MessageBox.Show($"Por favor, preencha os seguintes campos:\n{erros}", "Alerta!", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var text = "Atualizado";
                if (_podeInserir)
                {
                    await InserirObjectoBDAsync();
                    text = "Adicionado";
                }
                else
                {
                    await AtualizarObjectoBDAsync();
                }

                MessageBox.Show($"Registro {text} com sucesso!", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);

                // Limpa os campos
                Id = null; // obrigamos a limpar
                ObjectoEditar = NovoObjectoEditar();
                RaisePropertyChanged(nameof(ObjectoEditar));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Registro Não Pode Ser Salvo", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}