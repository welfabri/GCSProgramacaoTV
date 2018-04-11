using Prism.Events;
using Prism.Navigation;
using Unity;

namespace GCSProgramacaoTV.ViewModels
{
    public class RegistroTabbedPageViewModel : ViewModelBase
	{
        public RegistroTabbedPageViewModel(INavigationService navigationService,
            IUnityContainer unityContainer,
            IEventAggregator eventAggregator) : base(navigationService, unityContainer, eventAggregator)
        {

        }

        protected override void IniciaCommands()
        {
            
        }
    }
}
