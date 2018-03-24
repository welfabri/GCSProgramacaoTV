using System;
using System.Collections.Generic;
using System.Text;

namespace GCSProgramacaoTV.Model.Classes
{
    public static class ProgramasCtrl
    {
        public static string[] CanaisHTV()
        {
            string[] canais = new string[]
            {
                "a&e", "amc", "animal planet", "axn", 
                "baby", "band", "bbc", "bis", "bloomberg"
            };

            return canais;
        }
    }
}
