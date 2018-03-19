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
    }
}
