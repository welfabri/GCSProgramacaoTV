﻿using GCSEntities.Classes;
using Prism.Events;
using Prism.Mvvm;
using Prism.Navigation;
using Unity;

namespace GCSProgramacaoTV.ViewModels
{
    public abstract class ViewModelBase : BindableBase, INavigationAware, IDestructible
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

            this.IniciaCommands();
        }

        protected abstract void IniciaCommands();

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

        protected bool UsuarioLogado()
        {
            return this.UnityContainer.IsRegistered<Usuario>();
        }

        protected int DevolveIdUsuario()
        {
            var u = this.UnityContainer.Resolve<Usuario>();
            return u != null ? u.Id : -1;
        }
    }
}
