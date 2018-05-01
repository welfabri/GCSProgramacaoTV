using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace GCSProgramacaoTV.Model.Classes
{
    public class MasterPageItem : BindableBase
    {
        private bool _visivel;
        private Color _cor;

        public string Title { get; set; }
        public string IconSource { get; set; }
        public String TargetTypeName { get; set; }
        public Type TargetType { get; set; }
        public bool Visivel { get { return this._visivel; } set { SetProperty(ref this._visivel, value); Cor = value ? Color.Green : Color.LightGray; } }

        public Color Cor { get { return _cor; } set { SetProperty(ref _cor, value); } }

        public MasterPageItem Preencher(string title, string iconSource, 
            String targetTypeName, Type targetType, bool visivel)
        {
            this.Title = title;
            this.IconSource = iconSource;
            this.TargetTypeName = targetTypeName;
            this.TargetType = targetType;
            this.Visivel = visivel;

            return this;
        }
    }
}
