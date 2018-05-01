using GCSProgramacaoTV.Model;
using GCSProgramacaoTV.Model.Interfaces;
using HtmlAgilityPack;
using Plugin.LocalNotifications;
using Plugin.Notifications;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Globalization;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace GCSProgramacaoTV.ViewModels
{
    public class DetalheProgramaViewModel : BindableBase, INavigationAware
	{
        private string _nomePrograma, _sinopse, _title, _horario, _canal, _data;
        private HtmlDocument _htmlRetornado;

        public string NomePrograma { get => _nomePrograma; set { SetProperty(ref _nomePrograma, value); Title = value; } }

        public string Title { get => _title; set => SetProperty(ref _title, value); }

        public string Sinopse { get => _sinopse; set => SetProperty(ref _sinopse, value); }

        public string Horario { get => _horario; set { SetProperty(ref _horario, value); this.CmdLembrar.RaiseCanExecuteChanged(); } }

        public DateTime HorarioDateTime => DateTime.ParseExact(this.Horario, "HH:mm",
             CultureInfo.InvariantCulture);

        public string Data { get => _data; set { SetProperty(ref _data, value); this.CmdLembrar.RaiseCanExecuteChanged(); } }

        public DateTime DataDateTime
        {
            get
            {
                //Seg, 19/03 às 15h25
                //MELHORAR
                string dia = this.Data.Substring(5, 2);
                string mes = this.Data.Substring(8, 2);
                string hora = this.Data.Substring(14, 2);
                string minuto = this.Data.Substring(17, 2);

                return new DateTime(DateTime.Now.Year, int.Parse(mes), int.Parse(dia), int.Parse(hora), int.Parse(minuto), 0);
            }
        }

        public DelegateCommand CmdLembrar { get; set; }

        public DetalheProgramaViewModel()
        {
            IniciaComandos();
        }

        private void IniciaComandos()
        {
            this.CmdLembrar = new DelegateCommand(async () => await DoLembrar(), CanLembrar);
        }

        private bool CanLembrar()
        {
            return (this.Data != null) && (this.DataDateTime > DateTime.Now);
        }

        private async Task DoLembrar()
        {
            try
            {
                Notification n = new Notification()
                {
                    //Date = this.DataDateTime,
                    Id = DateTime.Now.Minute + DateTime.Now.Second,
                    Message = $"{this.NomePrograma} no canal { this._canal}",
                    Title = "Lembrete de Programação",
                    Vibrate = true,
                    When = this.DataDateTime.TimeOfDay
                };
                //VERIFICAR SE FOI CORRIGIDO POIS ESTÁ MANDANDO A NOTIFICAÇÃO IMEDIATAMENTE
                //await CrossNotifications.Current.Send(n);

                CrossLocalNotifications.Current.Show("Lembrete de Programação", 
                    $"{this.NomePrograma} no canal { this._canal}",
                    DateTime.Now.Minute + DateTime.Now.Second,
                    this.DataDateTime);
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

                if (parameters.ContainsKey("canal"))
                    this._canal = (string)parameters["canal"];
            }
        }

        public void OnNavigatingTo(NavigationParameters parameters)
        {
            //if (parameters.ContainsKey("id"))
            //    MostraDetalhes((string)parameters["id"]);
        }

        private async void MostraDetalhes(string id)
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
                _htmlRetornado = await web.LoadFromWebAsync(html);

                var noTitulo = _htmlRetornado.DocumentNode.SelectSingleNode("//div/div/h1");
                NomePrograma = noTitulo?.InnerHtml;

                var noScript = _htmlRetornado.DocumentNode.SelectSingleNode("//div/div/ul/li/div/p/script");
                int? idxIni = noScript?.InnerHtml.IndexOf("\"");
                int? idxFim = noScript?.InnerHtml.IndexOf(";");

                if (idxIni != null && idxIni > 0)
                    this.Sinopse = noScript?.InnerHtml.Substring(idxIni.Value + 1, idxFim.Value - idxIni.Value - 2);

                var noData = _htmlRetornado.DocumentNode.SelectSingleNode("//div/div/ul/li/a/div/h2");

                if (noData != null)
                    this.Data = noData.InnerHtml;
            }
            catch(Exception ex)
            {
                this.Sinopse = ex.Message;
            }
        }
    }
}
