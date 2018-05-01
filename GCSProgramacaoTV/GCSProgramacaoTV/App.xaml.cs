using GCSProgramacaoTV.Views;
using Prism;
using Prism.Ioc;
using Prism.Unity;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace GCSProgramacaoTV
{
    public partial class App : PrismApplication
    {
        /* 
         * The Xamarin Forms XAML Previewer in Visual Studio uses System.Activator.CreateInstance.
         * This imposes a limitation in which the App class must have a default constructor. 
         * App(IPlatformInitializer initializer = null) cannot be handled by the Activator.
         */
        public App() : this(null) { }

        public App(IPlatformInitializer initializer) : base(initializer) { }

        protected override async void OnInitialized()
        {
            InitializeComponent();

            //await NavigationService.NavigateAsync("NavigationPage/MainPage");
            await NavigationService.NavigateAsync("MasterDetailMainPage");
            //await NavigationService.NavigateAsync("RegistroTabbedPage");
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<MasterDetailMainPage>();
            containerRegistry.RegisterForNavigation<MasterPage>();
            containerRegistry.RegisterForNavigation<MainPage>();
            containerRegistry.RegisterForNavigation<DetalhePrograma>();
            containerRegistry.RegisterForNavigation<RegistroTabbedPage>();
            containerRegistry.RegisterForNavigation<LoginPage>();
            containerRegistry.RegisterForNavigation<RegistroPage>();
            containerRegistry.RegisterForNavigation<MeusLembretesPage>();
            containerRegistry.RegisterForNavigation<MinhasInformacoesPage>();
            containerRegistry.RegisterForNavigation<FavoritosPage>();
            containerRegistry.RegisterForNavigation<ConfiguracoesPage>();
        }
    }
}
