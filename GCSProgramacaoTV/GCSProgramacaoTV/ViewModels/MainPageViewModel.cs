using GCSProgramacaoTV.Model;
using GCSProgramacaoTV.Model.Classes;
using HtmlAgilityPack;
using Prism.Commands;
using Prism.Events;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Unity;
using Xamarin.Forms;

namespace GCSProgramacaoTV.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        #region Campos
        private Programa _programaSelecionado;
        private Canal _canalSelecionado;
        private string _dataAtual;
        private bool _estaCarregando = false;
        private string _textoBuscar;

        private List<Canal> _todosCanais;
        private KeyValuePair<string, string> _generoSelecionado;
        #endregion

        #region Propriedades
        public String DataAtual
        {
            get => _dataAtual;
            set => SetProperty(ref _dataAtual, value);
        }

        public bool EstaCarregando { get => _estaCarregando; set => SetProperty(ref _estaCarregando, value); }

        public ObservableCollection<Canal> ListaCanais { get; set; }
        public ObservableCollection<Programa> ListaProgramas { get; set; }
        public ObservableCollection<KeyValuePair<string, string>> ListaGeneros { get; set; }

        public Canal CanalSelecionado { get => _canalSelecionado; set { SetProperty(ref _canalSelecionado, value); SelecionouCanal(); } }

        public Programa ProgramaSelecionado { get => _programaSelecionado; set { SetProperty(ref _programaSelecionado, value); SelecionouPrograma(); } }

        public KeyValuePair<string, string> GeneroSelecionado { get => _generoSelecionado; set { SetProperty(ref _generoSelecionado, value); SelecionouGenero(); } }        

        public string TextoBuscarCanais
        {
            get => _textoBuscar;
            set
            {
                SetProperty(ref _textoBuscar, value);
                BuscaCanaisPeloTexto();
            }
        }

        public DelegateCommand CmdAtualizar { get; set; }

        /// <summary>
        /// http://blog.stephencleary.com/2013/01/async-oop-2-constructors.html
        /// </summary>
        public Task Initialization { get; private set; }
        #endregion

        public MainPageViewModel(INavigationService navigationService,
            IUnityContainer unityContainer,
            IEventAggregator eventAggregator)
            : base(navigationService, unityContainer, eventAggregator)
        {
            Title = "Programação da TV";

            Initialization = IniciaSistemaAsync();
        }

        private async Task IniciaSistemaAsync()
        {
            await IniciaSistema();
        }

        private async Task IniciaSistema()
        {
            IniciaCommands();
            IniciaListas();            
            IniciaTimer();

            this._generoSelecionado = this.ListaGeneros.FirstOrDefault();
        }

        private void IniciaListas()
        {

            KeyValuePair<string, string> CriaChave(string chave, string valor)
            {
                return new KeyValuePair<string, string>(chave, valor);
            }

            this._todosCanais = new List<Canal>();
            this.ListaCanais = new ObservableCollection<Canal>();
            this.ListaProgramas = new ObservableCollection<Programa>();
            this.ListaGeneros = new ObservableCollection<KeyValuePair<String, String>>()
            {
                { CriaChave("Todos", "Todos") }, 
                { CriaChave("Filmes", "Filmes") },
                { CriaChave("Séries", "Series") },
                { CriaChave("Esportes", "Esportes") },
                { CriaChave("Infantil", "Infantil") },
                { CriaChave("Variedades", "Variedades") },
                { CriaChave("Documentários", "Documentarios") },
                { CriaChave("Notícias", "Noticias") },
                { CriaChave("Aberta", "Aberta") },
                { CriaChave("Htv", "Todos") }
            };            
        }

        private void IniciaCommands()
        {
            this.CmdAtualizar = new DelegateCommand(DoAtualizar);
        }

        /// <summary>
        /// A cada tempo, atualzia a lista de canais
        /// </summary>
        private void IniciaTimer()
        {
            AtualizaDataAtual();

            Device.StartTimer(TimeSpan.FromMinutes(15), () =>
            {
                DoAtualizar();

                return true; // True = Repeat again, False = Stop the timer
            });
        }

        private async Task IniciaCanais()
        {
            /*
             * Exemplo de como devolve o nó de canais
             * 
             <li>
                <a title="Segurança Máxima" class="devicepadding" href="/programacao/canal/MDO">
	                <div class="lileft logo canal_mdo"></div>
	                <div class="licontent">
		                <h2>A&amp;E</h2>
		                <h3 class="dark"><strong>15:00</strong>&nbsp;&nbsp;<div class="progressbar"><div class="progress" style="width: 2px;"></div></div>&nbsp;&nbsp;Segurança Máxima</h3>
		                <h3><strong>16:00</strong>&nbsp;&nbsp;Escravos da Cientologia</h3>
		                <h3><strong>17:00</strong>&nbsp;&nbsp;Tratamento de Choque</h3>
	                </div>
                </a>
             </li>
             */

            if (String.IsNullOrEmpty(GeneroSelecionado.Value))
                return;

            this.EstaCarregando = true;

            try
            {
                this._todosCanais.Clear();
                this.ListaCanais.Clear();
                this.ListaProgramas.Clear();
                
                string html = Constantes.BASEURL + $@"/programacao/categoria/{GeneroSelecionado.Value}";

                HtmlWeb web = new HtmlWeb();

                var htmlDoc = await web.LoadFromWebAsync(html);

                var nodes = htmlDoc.DocumentNode.SelectNodes("//ul/li/a");

                if (nodes != null)
                {
                    foreach (HtmlNode node in nodes)
                    {
                        if (node != null && node.Attributes["title"] != null)
                        {
                            var c = node.Descendants("div").Where(x => x.Attributes["class"].Value == "licontent").FirstOrDefault();

                            if (c != null)
                            {
                                //Pega o programa atual que está na tag h2
                                var d = c.ChildNodes.Where(x => x.Name == "h2").FirstOrDefault();

                                Canal cnl = new Canal()
                                {
                                    Id = node.Attributes["href"]?.Value,
                                    Nome = d?.InnerHtml.Replace("&amp;", "&"),
                                    Numero = 0,
                                    ProgramaAtual = node.Attributes["title"]?.Value
                                };

                                if ((GeneroSelecionado.Key.ToLower() != "htv") || Htv.ECanalHtv(cnl.Nome))
                                {
                                    this.ListaCanais.Add(cnl);
                                    this._todosCanais.Add(cnl);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                this.DataAtual = ex.Message;
            }
            finally
            {
                this.EstaCarregando = false;
            }
        }

        private async Task SelecionouCanal()
        {
            await DevolveListaProgramas();
        }

        private async Task DevolveListaProgramas()
        {
            /*
             * 
             <li class="subheader devicepadding">Domingo, 18/03</li>

             <li>
				<a title="Segurança Máxima" class="devicepadding" href="/programacao/programa/475607-seguranca-maxima">
					<div class='lileft time'>15:00</div>
					<div class="licontent">
						<h2>Segurança Máxima</h2>
						<h3>Séries/Reality Show</h3>
					</div>
					<div class="liright"><div class="noar">NO AR</div></div>
                </a>			
             </li>
             */


            this.EstaCarregando = true;

            try
            {
                this.ListaProgramas.Clear();

                string html = Constantes.BASEURL + this.CanalSelecionado.Id;

                HtmlWeb web = new HtmlWeb();

                var htmlDoc = await web.LoadFromWebAsync(html);

                //var nodeData = htmlDoc.DocumentNode.SelectSingleNode("//ul/li");

                var nodesA = htmlDoc.DocumentNode.SelectNodes("//ul/li/a");

                if (nodesA != null)
                {
                    foreach (HtmlNode node in nodesA)
                    {
                        if (node != null && node.Attributes["title"] != null)
                        {
                            Programa prg = new Programa()
                            {
                                Id = node.Attributes["href"].Value,
                                Nome = node.Attributes["title"].Value
                            };

                            //var c = node.Descendants("div").Where(x => x.Attributes["class"].Value == "time").FirstOrDefault();
                            var c = node.Descendants("div").FirstOrDefault();

                            prg.Horario = c?.InnerHtml; 

                            this.ListaProgramas.Add(prg);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                this.DataAtual = ex.Message;
            }
            finally
            {
                this.EstaCarregando = false;
            }
        }

        private async Task SelecionouPrograma()
        {
            //Abrir tela com a sinopse
            if (ProgramaSelecionado != null)
            {
                NavigationParameters n = new NavigationParameters("id=" + ProgramaSelecionado.Id)
                {
                    { "nomePrograma", ProgramaSelecionado.Nome },
                    { "horario", ProgramaSelecionado.Horario },
                    { "canal", CanalSelecionado.Nome }
                };
                await NavigationService.NavigateAsync("NavigationPage/DetalhePrograma", n);
            }
        }

        private async void DoAtualizar()
        {            
            AtualizaDataAtual();
            await IniciaCanais();
        }

        private void BuscaCanaisPeloTexto()
        {
            this.ListaCanais.Clear();

            if (string.IsNullOrWhiteSpace(this.TextoBuscarCanais))
                this._todosCanais.ForEach(this.ListaCanais.Add);
            else
                this._todosCanais.Where(x => x.Nome.ToLower().Contains(this.TextoBuscarCanais.ToLower())).ToList().ForEach(this.ListaCanais.Add);
        }

        private void AtualizaDataAtual()
        {
            this.DataAtual = "Última Atualização: " + DateTime.Now.ToString("dd/MM/yyyy hh:mm");
        }

        private async Task SelecionouGenero()
        {
            await IniciaCanais();
        }
    }
}