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
        private readonly bool PodeInserir;

        private readonly AbstractValidator<ObjectBD> validador;
        protected readonly IDAL<ObjectBD> Repositorio;

        public CadastroViewModel(int? id, IDAL<ObjectBD> Repositorio)
        {
            //this.validador = validador;
            this.Repositorio = Repositorio;

            SaveCommand = new RelayCommandWithParameter(ExecutarSalvar, PodeExecutarSalvar);

            if (id.HasValue)
                ObjectoEditar = (ObjectoEditarView)Activator.CreateInstance(typeof(ObjectoEditarView), Repositorio.GetById(id.Value));
            else
            {
                //criamos um novo model
                ObjectoEditar = NovoObjectoEditar();
                PodeInserir = true;
            }
        }
        public virtual ObjectoEditarView NovoObjectoEditar()
        {
            return Activator.CreateInstance<ObjectoEditarView>(); //Ver Com O Professor se fica  o activator mesmo
        }

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
            return Repositorio.Insert(objBD);
        }

        public virtual void AtualizarObjectoBD()
        {
            var objBD = ObjectoEditar.DevolveObjectoBD();
            Repositorio.Update(objBD);
        }

        public bool PodeExecutarSalvar(object parameter)
        {
            return true; // TODO: AINDA TEM DE SER FEITO!!
            //var objBD = ObjectoEditar.DevolveObjectoBD();
            //var result = validador.Validate(objBD);
            //if (!result.IsValid)
            //{
            //    string errors = null;
            //    var count = 1;

            //    foreach (var failure in result.Errors)
            //    {
            //        errors += $"{count++} - {failure.ErrorMessage}\n";
            //    }
            //    //MessageBox.Show(errors, "Alerta De Validação De Registro", MessageBoxButton.OK, MessageBoxImage.Information);
            //}
            //return result.IsValid;
        }

        public void ExecutarSalvar(object parameter)
        {
            try
            {
                var text = "Atualizado";
                if (PodeInserir)
                {
                    InserirObjectoBD();
                    text = "Adicionado";
                }
                else
                    AtualizarObjectoBD();

                MessageBox.Show($" Registo {text} Com Sucesso!", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);

                //limpa os Campos
                ObjectoEditar = NovoObjectoEditar();
                RaisePropertyChanged(nameof(ObjectoEditar));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Registo Não Pode Ser Salvo", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}