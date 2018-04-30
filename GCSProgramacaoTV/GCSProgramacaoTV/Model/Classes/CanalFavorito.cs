using Prism.Mvvm;

namespace GCSProgramacaoTV.Model.Classes
{
    public class CanalFavorito : BindableBase
    {
        private string _name;
        private bool _checado;

        public string Id { get; set; }
        public string Nome { get { return _name; } set { SetProperty(ref _name, value); } }
        public bool Checado { get { return _checado; } set { SetProperty(ref _checado, value); } }
    }
}
