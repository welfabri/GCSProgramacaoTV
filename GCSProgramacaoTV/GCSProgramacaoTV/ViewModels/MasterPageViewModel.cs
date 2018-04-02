using GCSProgramacaoTV.Model.Classes;
using GCSProgramacaoTV.Views;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System.Collections.ObjectModel;

namespace GCSProgramacaoTV.ViewModels
{
    public class MasterPageViewModel : BindableBase
	{
        private string _clientImage = "http://1.bp.blogspot.com/-OlBlCoFO0w8/VVE7CYvL7sI/AAAAAAAALAs/cMUrOZiC8xc/s1600/user.png";
        public string ClientImage
        {
            get { return _clientImage; }
            set { SetProperty(ref _clientImage, value); }
        }

        private string _clientName = "Não Registrado";
        public string ClientName
        {
            get { return _clientName; }
            set { SetProperty(ref _clientName, value); }
        }

        private MasterPageItem _menuItemSelected;
        private IEventAggregator _eventAggregator;

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
            INavigationService navigationService,*/ IEventAggregator eventAggregator)
        {
            this._eventAggregator = eventAggregator;

            /*
            this._unityContainer = unityContainer;
            
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
            
            if (item != null)
            {
                //this._eventAggregator.GetEvent<DetailClickEvent>().Publish(item.TargetType);
            }
            
        }

        private void CreateMenuItems()
        {
            this.MenuItems = new ObservableCollection<MasterPageItem>()
            {
                new MasterPageItem()
                {
                    Title = "Principal",
                    //IconSource = "Resources/glyphicons_charts.png",
                    TargetTypeName = "MainPage",
                    TargetType = typeof(MainPage)
                },
                new MasterPageItem()
                {
                    Title = "Configurações",
                    //IconSource = "Resources/glyphicons_cogwheel.png",
                    TargetTypeName = "DetalhePrograma",
                    TargetType = typeof(DetalhePrograma)
                }
            };

            this.MenuItemSelected = this.MenuItems[0];
        }
    }
}
