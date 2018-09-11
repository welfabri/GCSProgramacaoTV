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

        public abstract void OnNavigatedFrom(INavigationParameters parameters);

        public abstract void OnNavigatedTo(INavigationParameters parameters);

        public abstract void OnNavigatingTo(INavigationParameters parameters);
    }
}
