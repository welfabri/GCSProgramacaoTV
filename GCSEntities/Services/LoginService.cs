using System;
using System.Collections.Generic;
using System.Text;

namespace GCSEntities.Services
{
    public static class LoginService
    {
        public static bool Login(string usuario, string senha)
        {
            return usuario.Equals("admin", StringComparison.OrdinalIgnoreCase) && senha.Equals("admin", StringComparison.OrdinalIgnoreCase);
        }

        public static bool Registrar(string email, string senha, string nome)
        {
            return true;
        }
    }
}
