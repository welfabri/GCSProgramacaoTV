using GCSProgramacaoTV.Model;
using GCSProgramacaoTV.Model.Interfaces;
using HtmlAgilityPack;
using Plugin.LocalNotifications;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Globalization;
using Xamarin.Forms;

namespace GCSProgramacaoTV.ViewModels
{
    public class DetalheProgramaViewModel : BindableBase, INavigationAware
	{
        private string _nomePrograma;
        private string _sinopse;
        private string _title;
        private string _horario;

        public string NomePrograma { get => _nomePrograma; set { SetProperty(ref _nomePrograma, value); Title = value; } }

        public string Title { get => _title; set => SetProperty(ref _title, value); }

        public string Sinopse { get => _sinopse; set => SetProperty(ref _sinopse, value); }

        public string Horario { get => _horario; set => SetProperty(ref _horario, value); }

        public DelegateCommand CmdLembrar { get; set; }

        public DetalheProgramaViewModel()
        {
            IniciaComandos();
        }

        private void IniciaComandos()
        {
            this.CmdLembrar = new DelegateCommand(DoLembrar, CanLembrar);
        }

        private bool CanLembrar()
        {
            return true;
        }

        private void DoLembrar()
        {
            try
            {
                //Passar a data também
                DateTime dateTime = DateTime.ParseExact(this.Horario, "HH:mm",
                                            CultureInfo.InvariantCulture);
                CrossLocalNotifications.Current.Show("Lembrar de Programação", "vai começar o programa " + this.NomePrograma,
                    1, dateTime);
                DependencyService.Get<IMessage>().LongAlert("Lembrete adicionado");
            }
            catch(Exception ex)
            {
                this.Sinopse = ex.Message;
            }
        }

        public void OnNavigatedFrom(NavigationParameters parameters)
        {

        }

        public void OnNavigatedTo(NavigationParameters parameters)
        {
            if (parameters.ContainsKey("id"))
            {
                MostraDetalhes((string)parameters["id"]);

                if (parameters.ContainsKey("horario"))
                    this.Horario = (string)parameters["horario"];
            }
        }

        public void OnNavigatingTo(NavigationParameters parameters)
        {
            //if (parameters.ContainsKey("id"))
            //    MostraDetalhes((string)parameters["id"]);
        }

        private void MostraDetalhes(string id)
        {
            /*
             * 
             <h1 class="devicepadding">Segurança Máxima</h1>
            <ul>
	            <li>
                    <div class="body1 devicepadding detalhes">
                        <p class="body2">
		    	            Behind Bars: Rookie Year: Season 1 - Ep. 01<br />Reality Show| EUA (2015)
                        </p>

                        <p>
                            <script type="text/javascript">
			                    var str="Vamos acompanhar os melhores da classe no início de sua carreira na Penitenciária do Novo México onde todos os novos agentes enfrentam uma questão de vida ou morte: será que vale a pena?";
			                    document.write(str.replace(".tvp.", ""));
		                    </script>
                        </p>
                        <p><strong>Dire&ccedil;&atilde;o:</strong><br /><a class='nome' href='/programacao/busca/Campbell Scott'>Campbell Scott</a><a class='nome' href='/programacao/busca/Stanley Tucci'>Stanley Tucci</a></p>
                        <p><strong>Elenco:</strong><br /><a class='nome' href='/programacao/busca/Marc Anthony'>Marc Anthony</a><a class='nome' href='/programacao/busca/Tony Shalhoub'>Tony Shalhoub</a><a class='nome' href='/programacao/busca/Stanley Tucci'>Stanley Tucci</a><a class='nome' href='/programacao/busca/Minnie Driver'>Minnie Driver</a><a class='nome' href='/programacao/busca/Isabella Rossellini'>Isabella Rossellini</a><a class='nome' href='/programacao/busca/Liev Schreiber'>Liev Schreiber</a><a class='nome' href='/programacao/busca/Ian Holm'>Ian Holm</a><a class='nome' href='/programacao/busca/Allison Janney'>Allison Janney</a><a class='nome' href='/programacao/busca/Peter Appel'>Peter Appel</a><a class='nome' href='/programacao/busca/Susan Floyd'>Susan Floyd</a><a class='nome' href='/programacao/busca/Seth Jones'>Seth Jones</a></p>
                    </div>
                </li>

                        ....

            <li class="subheader devicepadding">Pr&oacute;ximas exibi&ccedil;&otilde;es</li>



		<li>
		    <a class="devicepadding">
		      <div class="lileft logo canal_mgm"></div>
		      <div class="licontent">
			      <h2>Sáb, 17/03 às 02h15</h2>
			      <h3>AMC</h3>
		      </div>
		    </a>
		</li>


		<li>
		    <a class="devicepadding">
		      <div class="lileft logo canal_mgm"></div>
		      <div class="licontent">
			      <h2>Qui, 22/03 às 05h25</h2>
			      <h3>AMC</h3>
		      </div>
		    </a>
		</li>



</ul>
             */

            string html = id.Contains("http") ? id : Constantes.BASEURL + id;

            HtmlWeb web = new HtmlWeb();

            try
            {
                var htmlDoc = web.Load(html);

                var noTitulo = htmlDoc.DocumentNode.SelectSingleNode("//div/div/h1");
                NomePrograma = noTitulo?.InnerHtml;

                var noScript = htmlDoc.DocumentNode.SelectSingleNode("//div/div/ul/li/div/p/script"); ;
                int? idxIni = noScript?.InnerHtml.IndexOf("\"");
                int? idxFim = noScript?.InnerHtml.IndexOf(";");

                if (idxIni != null && idxIni > 0)
                    this.Sinopse = noScript?.InnerHtml.Substring(idxIni.Value + 1, idxFim.Value - idxIni.Value - 2);
            }
            catch(Exception ex)
            {
                this.Sinopse = ex.Message;
            }
        }
    }
}
