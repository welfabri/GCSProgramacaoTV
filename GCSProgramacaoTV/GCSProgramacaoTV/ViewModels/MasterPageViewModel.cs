using GCSEntities.Classes;
using GCSProgramacaoTV.Model.Classes;
using GCSProgramacaoTV.Model.Eventos;
using GCSProgramacaoTV.Views;
using Prism.Commands;
using Prism.Events;
using Prism.Navigation;
using System.Collections.ObjectModel;
using Unity;

namespace GCSProgramacaoTV.ViewModels
{
    public class MasterPageViewModel : ViewModelBase
	{
        private string _clientImage;
        private string _clientName;
        private MasterPageItem _menuItemSelected;
        private bool _estaLogado;

        public string ClientImage
        {
            get { return _clientImage; }
            set { SetProperty(ref _clientImage, value); }
        }        

        public string ClientName
        {
            get { return _clientName; }
            set { SetProperty(ref _clientName, value); }
        }

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

        public bool EstaLogado { get { return _estaLogado; } set { SetProperty(ref _estaLogado, value); } }



        public MasterPageViewModel(INavigationService navigationService,
            IUnityContainer unityContainer,
            IEventAggregator eventAggregator) : base(navigationService,
            unityContainer, eventAggregator)
        {
            IniciaUsuario();
            CreateMenuItems();            
        }

        private void IniciaUsuario()
        {            
            if (this.UnityContainer.IsRegistered<Usuario>())
            {
                var u = this.UnityContainer.Resolve<Usuario>();
                this.ClientName = u.Nome;
                this.ClientImage = "https://thumbs.dreamstime.com/z/vector-o-%C3%ADcone-do-avatar-do-usu%C3%A1rio-para-site-ou-o-m%C3%B3bil-45836554.jpg";
                //                this.MenuItemSelected = this.MenuItems[0];

                short i = 0;

                foreach(var m in this.MenuItems)
                {
                    m.Visivel = i != 1;
                    i++;
                }
            } else
            {
                this.ClientName = "Não Registrado";
                this.ClientImage = "http://1.bp.blogspot.com/-OlBlCoFO0w8/VVE7CYvL7sI/AAAAAAAALAs/cMUrOZiC8xc/s1600/user.png";
            }
        }

        protected override void IniciaCommands()
        {
            this.MenuItemCmd = new DelegateCommand<MasterPageItem>(OnMenuItem);
        }

        private void OnMenuItem(MasterPageItem item)
        {
            
            if (item != null)
            {
                
                //if (item.TargetType != typeof(RegistroTabbedPage))
                //    this.EventAggregatorProperty.GetEvent<DetailClickEvent>().Publish(item.TargetType);
                //else*/
                    this.NavigationService.NavigateAsync("NavigationPage/" + item.TargetTypeName);
            }            
        }

        private void CreateMenuItems()
        {
            if (this.MenuItems == null)
            {
                this.MenuItems = new ObservableCollection<MasterPageItem>()
                {
                    new MasterPageItem().Preencher("Principal", "Resources/glyphicons_charts.png", nameof(MainPage), typeof(MainPage), true),
                    new MasterPageItem().Preencher("Entrar", "Resources/glyphicons_charts.png", nameof(RegistroTabbedPage), typeof(RegistroTabbedPage), true),
                    new MasterPageItem().Preencher("Meus Lembretes", "Resources/glyphicons_charts.png", nameof(RegistroTabbedPage), typeof(MainPage), false),
                    new MasterPageItem().Preencher("Minhas Informações", "Resources/glyphicons_charts.png", nameof(RegistroTabbedPage), typeof(MainPage), false),                    
                    new MasterPageItem().Preencher("Configurações", "Resources/glyphicons_charts.png", nameof(RegistroTabbedPage), typeof(MainPage), false)                        
                };                
            }

            //this.MenuItemSelected = this.MenuItems[0];
        }
    }
}
