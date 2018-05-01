using Prism.Events;
using Prism.Navigation;
using Unity;

namespace GCSProgramacaoTV.ViewModels
{
    public abstract class ViewModelNavigationBase : ViewModelBase, INavigationAware
    {
        public ViewModelNavigationBase(INavigationService navigationService,
           IUnityContainer unityContainer,
           IEventAggregator eventAggregator) : base (navigationService, unityContainer, eventAggregator)
        { 
        }

        public abstract void OnNavigatedFrom(NavigationParameters parameters);

        public abstract void OnNavigatedTo(NavigationParameters parameters);

        public abstract void OnNavigatingTo(NavigationParameters parameters);
    }
}
