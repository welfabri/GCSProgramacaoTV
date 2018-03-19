using GCSProgramacaoTV.Model.Classes;
using GCSProgramacaoTV.Views;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace GCSProgramacaoTV.ViewModels
{
	public class MasterPageViewModel : BindableBase
	{
        private string _clientImage = "https://media.licdn.com/mpr/mpr/shrinknp_200_200/AAEAAQAAAAAAAAvBAAAAJDFlOGU4YzNlLTdkZjItNGYwYS04ZWUwLTBlNzdkOGI0NTZlOA.jpg";
        public string ClientImage
        {
            get { return _clientImage; }
            set { SetProperty(ref _clientImage, value); }
        }

        private string _clientName = "Emiliano Soares";
        public string ClientName
        {
            get { return _clientName; }
            set { SetProperty(ref _clientName, value); }
        }

        private MasterPageItem _menuItemSelected;
        public MasterPageItem MenuItemSelected
        {
            get { return _menuItemSelected; }
            set
            {
                if (value != _menuItemSelected)
                {
                    SetProperty(ref _menuItemSelected, value);
                    OnMenuItem(value);
                }
            }
        }

        public ObservableCollection<MasterPageItem> MenuItems { get; set; }

        public DelegateCommand<MasterPageItem> MenuItemCmd { get; set; }

        public MasterPageViewModel(/*IUnityContainer unityContainer,
            INavigationService navigationService, IEventAggregator eventAggregator*/)
        {
            /*
            this._unityContainer = unityContainer;
            this._eventAggregator = eventAggregator;
            this._navigationService = navigationService;
            */
            CreateMenuItems();
            CreateCommands();
        }

        private void CreateCommands()
        {
            this.MenuItemCmd = new DelegateCommand<MasterPageItem>(OnMenuItem);
        }

        private void OnMenuItem(MasterPageItem item)
        {
            /*
            if (item != null)
            {
                this._eventAggregator.GetEvent<DetailClickEvent>().Publish(item.TargetType);
            }
            */
        }

        private void CreateMenuItems()
        {
            this.MenuItems = new ObservableCollection<MasterPageItem>()
            {
                new MasterPageItem()
                {
                    Title = "Principal",
                    IconSource = "Resources/glyphicons_charts.png",
                    TargetTypeName = "ClientMainPage_Main",
                    TargetType = typeof(DetalhePrograma)
                },
                new MasterPageItem()
                {
                    Title = "Configurações",
                    IconSource = "Resources/glyphicons_cogwheel.png",
                    TargetTypeName = "MainPage",
                    TargetType = typeof(MainPage)
                }
            };

            //this.MenuItemSelected = this.MenuItems[0];
        }
    }
}
