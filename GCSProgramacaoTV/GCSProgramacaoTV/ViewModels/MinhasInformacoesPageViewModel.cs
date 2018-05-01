using GCSEntities.Classes;
using Prism.Events;
using Prism.Navigation;
using Unity;

namespace GCSProgramacaoTV.ViewModels
{
    public class MinhasInformacoesPageViewModel : ViewModelBase
	{
        private Usuario _usuarioVM;
        private string _nome, _email, _cpf, _sexo;

        public string Nome { get => _nome; set => SetProperty(ref _nome, value); }
        public string Email { get => _email; set => SetProperty(ref _email, value); }
        public string CPF { get => _cpf; set => SetProperty(ref _cpf, value); }
        public string Sexo { get => _sexo; set => SetProperty(ref _sexo, value); }

        public MinhasInformacoesPageViewModel(INavigationService navigationService,
            IUnityContainer unityContainer,
            IEventAggregator eventAggregator) : base(navigationService,
            unityContainer, eventAggregator)
        {
            IniciaTela();
        }

        private void IniciaTela()
        {
            if (this.UnityContainer.IsRegistered<Usuario>())
            {
                //Pega a instância do usuário registrada
                this._usuarioVM = this.UnityContainer.Resolve<Usuario>();
                this.Nome = this._usuarioVM.Nome;
                this.Email = this._usuarioVM.Email;
                this.CPF = this._usuarioVM.CPF;
                this.Sexo = this._usuarioVM.DescricaoSexo;
            } else
            {
                this.MostraMensagemToast("Erro ao tentar pegar dados do usuário.");
            }
        }

        protected override void IniciaCommands()
        {
        }
    }
}
