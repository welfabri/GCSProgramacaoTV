using GCSEntities.Services;
using GCSProgramacaoTV.Model;
using GCSProgramacaoTV.Model.Classes;
using GCSProgramacaoTV.Model.Interfaces;
using HtmlAgilityPack;
using Prism.Commands;
using Prism.Events;
using Prism.Navigation;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Unity;
using Xamarin.Forms;
using System.Linq;
using GCSEntities.Classes;

namespace GCSProgramacaoTV.ViewModels
{
    public class FavoritosPageViewModel : ViewModelBase
	{
        private CanalFavorito _canalSelecionado;

        private string MensagemErro
        {
            set
            {
                DependencyService.Get<IMessage>().LongAlert(value);
            }
        }

        public DelegateCommand GravarCmd { get; set; }

        public ObservableCollection<CanalFavorito> ListaCanais { get; set; }
        public CanalFavorito CanalSelecionado { get { return _canalSelecionado; } set { SetProperty(ref _canalSelecionado, value); SelecionouCanal(); } }

        private Task Inicializacao { get; }

        private void SelecionouCanal()
        {
            this.CanalSelecionado.Checado = !this.CanalSelecionado.Checado;
        }

        public FavoritosPageViewModel(INavigationService navigationService,
            IUnityContainer unityContainer,
            IEventAggregator eventAggregator) : base(navigationService, unityContainer, eventAggregator)
        {
            //Gambi pra funcionar o async
            this.Inicializacao = IniciaTela();
        }

        private async Task IniciaTela()
        {
            IniciaObjetos();
            await CarregaCanais();
            await PegaFavoritos();
        }

        private void IniciaObjetos()
        {
            this.ListaCanais = new ObservableCollection<CanalFavorito>();            
        }

        private async Task CarregaCanais()
        {
            string html = Constantes.BASEURL + $@"/programacao/categoria/Todos";

            HtmlWeb web = new HtmlWeb();

            var htmlDoc = await web.LoadFromWebAsync(html);

            var nodes = htmlDoc.DocumentNode.SelectNodes("//ul/li/a");

            if (nodes != null)
            {
                short i = 0;

                foreach (HtmlNode node in nodes)
                {
                    if (node != null && node.Attributes["title"] != null)
                    {
                        var c = node.Descendants("div").Where(x => x.Attributes["class"].Value == "licontent").FirstOrDefault();

                        if (c != null)
                        {
                            //Pega o programa atual que está na tag h2
                            var d = c.ChildNodes.Where(x => x.Name == "h2").FirstOrDefault();

                            CanalFavorito cnl = new CanalFavorito()
                            {
                                Id = i++.ToString(),
                                Nome = d?.InnerHtml.Replace("&amp;", "&"),
                            };

                            this.ListaCanais.Add(cnl);
                        }
                    }
                }
            }
        }

        private async Task PegaFavoritos()
        {
            string lf = await FavoritosService.ListaFavoritos(DevolveIdUsuario());

            if (!String.IsNullOrWhiteSpace(lf))
            {
                string[] af = lf.Split(',');

                foreach(string s in af)
                {
                    if (int.TryParse(s, out int idx))
                    {
                        CanalFavorito cf = this.ListaCanais[idx];
                        cf.Checado = true;
                    }
                }
            }
        }

        protected override void IniciaCommands()
        {
            this.GravarCmd = new DelegateCommand(async () => await DoGravar());
        }

        private async Task DoGravar()
        {
            string canaisFavoritos = String.Empty;

            var l = ListaCanais.Where(x => x.Checado);

            foreach (var c in l)
                canaisFavoritos = canaisFavoritos
                    + c.Id
                    + ",";

            string result = await FavoritosService.GravarFavorito(DevolveIdUsuario(), canaisFavoritos);

            if (!String.IsNullOrWhiteSpace(result))
                this.MensagemErro = result;
        }
    }
}
