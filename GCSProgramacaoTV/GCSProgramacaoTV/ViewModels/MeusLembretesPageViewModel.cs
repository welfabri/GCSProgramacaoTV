using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity;

namespace GCSProgramacaoTV.ViewModels
{
	public class MeusLembretesPageViewModel : ViewModelBase
	{
        public MeusLembretesPageViewModel(INavigationService navigationService,
            IUnityContainer unityContainer,
            IEventAggregator eventAggregator) : base(navigationService,
            unityContainer, eventAggregator)
        {

        }

        protected override void IniciaCommands()
        {
            
        }
    }
}
