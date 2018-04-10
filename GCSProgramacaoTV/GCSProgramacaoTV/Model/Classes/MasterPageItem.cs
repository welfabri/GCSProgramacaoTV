using System;
using System.Collections.Generic;
using System.Text;

namespace GCSProgramacaoTV.Model.Classes
{
    public class MasterPageItem
    {
        public string Title { get; set; }
        public string IconSource { get; set; }
        public String TargetTypeName { get; set; }
        public Type TargetType { get; set; }
        public bool Visivel { get; set; }

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
