using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using Unity;

namespace GCSProgramacaoTV.ViewModels
{
    public class ViewModelBase : BindableBase, INavigationAware, IDestructible
    {
        protected INavigationService NavigationService { get; private set; }
        protected IEventAggregator EventAggregatorProperty { get; private set; }
        protected IUnityContainer UnityContainer { get; private set; }

        private string _title;
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public ViewModelBase(INavigationService navigationService,
            IUnityContainer unityContainer,
            IEventAggregator eventAggregator)
        {
            NavigationService = navigationService;
            EventAggregatorProperty = eventAggregator;
            UnityContainer = unityContainer;
        }

        //protected abstract void IniciaVM();

        public virtual void OnNavigatedFrom(NavigationParameters parameters)
        {
            
        }

        public virtual void OnNavigatedTo(NavigationParameters parameters)
        {
            
        }

        public virtual void OnNavigatingTo(NavigationParameters parameters)
        {
            
        }

        public virtual void Destroy()
        {
            
        }
    }
}
