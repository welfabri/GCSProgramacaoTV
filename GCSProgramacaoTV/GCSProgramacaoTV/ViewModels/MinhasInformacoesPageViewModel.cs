using GCSEntities.Classes;
using Prism.Events;
using Prism.Navigation;
using Unity;

namespace GCSProgramacaoTV.ViewModels
{
    public class MinhasInformacoesPageViewModel : ViewModelBase
	{
        public string Nome { get; set; }

        public string Email { get; set; }

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
                var u = this.UnityContainer.Resolve<Usuario>();
                
                this.Nome = u?.Nome;
                this.Email = u?.Email;
            }
        }

        protected override void IniciaCommands()
        {
        }
    }
}
