using GCSProgramacaoTV.Model.Eventos;
using Prism.Events;
using System;
using Xamarin.Forms;

namespace GCSProgramacaoTV.Views
{
    public partial class MasterDetailMainPage : MasterDetailPage
    {
        public MasterDetailMainPage(IEventAggregator eventAggregator)
        {
            InitializeComponent();
            
            eventAggregator.GetEvent<DetailClickEvent>().Subscribe(OnClickDetail);            
        }

        private void OnClickDetail(Type page)
        {
            if (page != null)
            {
                Detail = new NavigationPage((Page)Activator.CreateInstance(page));
                IsPresented = false;
            }
        }
    }
}