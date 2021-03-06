﻿using GCSEntities.Classes;
using GCSEntities.Services;
using GCSProgramacaoTV.Model.Eventos;
using GCSProgramacaoTV.Model.Interfaces;
using GCSProgramacaoTV.Views;
using Prism.Commands;
using Prism.Events;
using Prism.Navigation;
using System;
using System.Threading.Tasks;
using Unity;
using Xamarin.Forms;

namespace GCSProgramacaoTV.ViewModels
{
    public class LoginPageViewModel : ViewModelBase
	{
        private string _email, _senha;

        public string Email { get { return _email; } set { SetProperty(ref _email, value); } }

        public string Senha { get { return _senha; } set { SetProperty(ref _senha, value); } }

        public DelegateCommand EntrarCmd { get; set; }
        public DelegateCommand EsqueciASenhaCmd { get; set; }

        private string MensagemErro { set
            {
                DependencyService.Get<IMessage>().LongAlert(value);
            }
        }

        public LoginPageViewModel(INavigationService navigationService,
            IUnityContainer unityContainer,
            IEventAggregator eventAggregator) : base(navigationService, unityContainer, eventAggregator)
        {
            
        }

        protected override void IniciaCommands()
        {
            this.EntrarCmd = new DelegateCommand(async () => await DoEntrar(), CanEntrar)
                .ObservesProperty(() => this.Email)
                .ObservesProperty(() => this.Senha);
            this.EsqueciASenhaCmd = new DelegateCommand(() => Device.OpenUri(new Uri($"{GCSConstantes.HOSTPRINCIPAL}/eas.php?e={this.Email}")));
        }

        private bool CanEntrar()
        {
            return !String.IsNullOrWhiteSpace(this.Email) && !String.IsNullOrWhiteSpace(this.Senha);
        }

        private async Task DoEntrar()
        {
            Usuario u = await LoginService.Login(this.Email, this.Senha);            

            if (u != null)
            {
                this.UnityContainer.RegisterInstance<Usuario>(u);
                this.EventAggregatorProperty.GetEvent<LoginEvent>().Publish();
            }
            else
                this.MensagemErro = "Não foi possível entrar no sistema, usuário e/ou senha inválido(s)";
        }
    }
}
