using GCSProgramacaoTV.Model;
using GCSProgramacaoTV.Model.Classes;
using HtmlAgilityPack;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace GCSProgramacaoTV.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        private Programa _programaSelecionado;
        private Canal _canalSelecionado;
        private string _dataAtual;
        private bool _estaCarregando = false;
        private string _textoBuscar;

        private List<Canal> _todosCanais;

        public String DataAtual
        {
            get => _dataAtual;
            set => SetProperty(ref _dataAtual, value);
        }

        public bool EstaCarregando { get => _estaCarregando; set => SetProperty(ref _estaCarregando, value); }

        public ObservableCollection<Canal> ListaCanais { get; set; }
        public ObservableCollection<Programa> ListaProgramas { get; set; }

        public Canal CanalSelecionado { get => _canalSelecionado; set { SetProperty(ref _canalSelecionado, value); SelecionouCanal(); } }

        public Programa ProgramaSelecionado { get => _programaSelecionado; set { SetProperty(ref _programaSelecionado, value); SelecionouPrograma(); } }

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

        public MainPageViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            Title = "Programação da TV";

            this.EstaCarregando = true;

            try
            {
                IniciaSistema();
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

        private void IniciaSistema()
        {
            IniciaListas();
            IniciaCommands();
            IniciaTimer();
            IniciaCanais();
        }

        private void IniciaListas()
        {
            this._todosCanais = new List<Canal>();
            this.ListaCanais = new ObservableCollection<Canal>();
            this.ListaProgramas = new ObservableCollection<Programa>();
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

            this.ListaCanais.Clear();
            this._todosCanais.Clear();
            string html = Constantes.BASEURL + @"/programacao/categoria/Todos";

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

                            this.ListaCanais.Add(cnl);
                            this._todosCanais.Add(cnl);
                        }
                    }
                }
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

            this.ListaProgramas.Clear();

            string html = Constantes.BASEURL + this.CanalSelecionado.Id;

            HtmlWeb web = new HtmlWeb();

            var htmlDoc = await web.LoadFromWebAsync(html);

            var nodes = htmlDoc.DocumentNode.SelectNodes("//ul/li/a");

            if (nodes != null)
            {
                foreach (HtmlNode node in nodes)
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

        private async void SelecionouPrograma()
        {
            //Abrir tela com a sinopse
            if (ProgramaSelecionado != null)
            {
                NavigationParameters n = new NavigationParameters("id=" + ProgramaSelecionado.Id)
                {
                    { "nomePrograma", ProgramaSelecionado.Nome },
                    { "horario", ProgramaSelecionado.Horario }
                };
                await NavigationService.NavigateAsync("NavigationPage/DetalhePrograma", n);
            }
        }

        private async void DoAtualizar()
        {
            this.EstaCarregando = true;

            try
            {
                AtualizaDataAtual();
                await IniciaCanais();
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

        private void BuscaCanaisPeloTexto()
        {
            this.ListaCanais.Clear();

            if (string.IsNullOrWhiteSpace(this.TextoBuscarCanais))
                this._todosCanais.ForEach(this.ListaCanais.Add);
            else
                this._todosCanais.Where(x => x.Nome.ToLower().Contains(this.TextoBuscarCanais.ToLower())).ToList().ForEach(this.ListaCanais.Add);
        }

        void AtualizaDataAtual()
        {
            this.DataAtual = "Última Atualização: " + DateTime.Now.ToString("dd/MM/yyyy hh:mm");
        }
    }
}