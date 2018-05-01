using GCSEntities.Classes;
using GCSProgramacaoTV.Model.Classes;
using GCSProgramacaoTV.Model.Eventos;
using GCSProgramacaoTV.Views;
using Prism.Commands;
using Prism.Events;
using Prism.Navigation;
using System;
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
        private readonly SubscriptionToken _tokenOnLoginOk;

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
            //Registra para pegar que o login está correto
            this._tokenOnLoginOk = eventAggregator.GetEvent<LoginEvent>().Subscribe(OnLoginOk);

            IniciaUsuario();
            CreateMenuItems();            
        }

        /// <summary>
        /// Recebe o evento e inicia
        /// </summary>
        /// <param name="page">não é usado</param>
        private void OnLoginOk()
        {
            IniciaUsuario();
        }

        private void IniciaUsuario()
        {            
            if (UsuarioLogado())
            {
                //Retira o evento para colocar o nome do usuário
                this.EventAggregatorProperty.GetEvent<DetailClickEvent>().Unsubscribe(this._tokenOnLoginOk);

                //Pega a instância do usuário registrada
                var u = this.UnityContainer.Resolve<Usuario>();
                this.ClientName = u.Nome;
                this.ClientImage = u.ImagemCliente;
                this.MenuItemSelected = this.MenuItems[0];

                short i = 0;

                if (this.MenuItems != null)
                    foreach(var m in this.MenuItems)
                    {
                        m.Visivel = i != 1;
                        i++;
                    }
            } else
            {
                this.ClientName = "Não Registrado";
                this.ClientImage = "http://queijonerd.pe.hu/gcs/images/user-u.jpg";
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
                    this.EventAggregatorProperty.GetEvent<DetailClickEvent>().Publish(item.TargetType);
                //else*/
                //    this.NavigationService.NavigateAsync("NavigationPage/" + item.TargetTypeName);
            }            
        }

        private void CreateMenuItems()
        {
            if (this.MenuItems == null)
            {
                this.MenuItems = new ObservableCollection<MasterPageItem>()
                {
                    new MasterPageItem().Preencher("Principal", "Resources/glyphicons_charts.png", nameof(MainPage), typeof(MainPage), true),
                    new MasterPageItem().Preencher("Entrar/Registrar", "Resources/glyphicons_charts.png", nameof(RegistroTabbedPage), typeof(RegistroTabbedPage), true),
                    new MasterPageItem().Preencher("Meus Lembretes", "Resources/glyphicons_charts.png", nameof(MeusLembretesPage), typeof(MeusLembretesPage), false),
                    new MasterPageItem().Preencher("Minhas Informações", "Resources/glyphicons_charts.png", nameof(MinhasInformacoesPage), typeof(MinhasInformacoesPage), false),
                    new MasterPageItem().Preencher("Meus Favoritos", "Resources/glyphicons_charts.png", nameof(FavoritosPage), typeof(FavoritosPage), false),
                    new MasterPageItem().Preencher("Configurações", "Resources/glyphicons_charts.png", nameof(ConfiguracoesPage), typeof(ConfiguracoesPage), true)                        
                };                
            }

            this._menuItemSelected = this.MenuItems[0];
        }
    }
}
