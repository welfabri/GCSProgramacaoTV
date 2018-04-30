using GCSEntities.Classes;
using GCSEntities.Services;
using GCSProgramacaoTV.Model.Eventos;
using GCSProgramacaoTV.Model.Interfaces;
using GCSProgramacaoTV.Views;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unity;
using Xamarin.Forms;

namespace GCSProgramacaoTV.ViewModels
{
	public class RegistroPageViewModel : ViewModelBase
    {
        private string _email, _senha, _repetirSenha, _nome;
        private string _cpf;

        public string Email { get { return _email; } set { SetProperty(ref _email, value); } }

        public string Senha { get { return _senha; } set { SetProperty(ref _senha, value); } }

        public string RepetirSenha { get { return _repetirSenha; } set { SetProperty(ref _repetirSenha, value); } }

        public string Nome { get { return _nome; } set { SetProperty(ref _nome, value); } }

        public string Cpf { get { return _cpf; } set { SetProperty(ref _cpf, value); } }

        public DelegateCommand RegistrarCmd { get; set; }

        private string MensagemErro
        {
            set
            {
                DependencyService.Get<IMessage>().LongAlert(value);
            }
        }

        public RegistroPageViewModel(INavigationService navigationService,
            IUnityContainer unityContainer,
            IEventAggregator eventAggregator) : base(navigationService, unityContainer, eventAggregator)
        {

        }

        
        protected override void IniciaCommands()
        {
            this.RegistrarCmd = new DelegateCommand(async () => await DoRegistrar(), CanRegistrar)
                .ObservesProperty(() => this.Email)
                .ObservesProperty(() => this.Senha)
                .ObservesProperty(() => this.RepetirSenha)
                .ObservesProperty(() => this.Nome);
        }

        private bool CanRegistrar()
        {
            return !String.IsNullOrWhiteSpace(this.Email) && !String.IsNullOrWhiteSpace(this.Senha) 
                && !String.IsNullOrWhiteSpace(this.RepetirSenha) && !String.IsNullOrWhiteSpace(this.Nome);
        }

        private async Task DoRegistrar()
        {
            Usuario u = await LoginService.Registrar(this.Email, this.Senha, this.Nome, this.Cpf);

            if (u != null)
            {
                this.UnityContainer.RegisterInstance<Usuario>(u);
                this.EventAggregatorProperty.GetEvent<DetailClickEvent>().Publish(typeof(MainPage));
            }
            else
                this.MensagemErro = "Não foi possível registrar no sistema";
        }
    }    
}
