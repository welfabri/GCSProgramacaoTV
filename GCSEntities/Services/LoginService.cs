using GCSEntities.Classes;
using System;

namespace GCSEntities.Services
{
    public static class LoginService
    {
        public static Usuario Login(string usuario, string senha)
        {
            if (!PreValidaUsuario(usuario, senha))
                return null;

            if (usuario.Equals("admin", StringComparison.OrdinalIgnoreCase) && senha.Equals("admin", StringComparison.OrdinalIgnoreCase))
            {
                return CriaUsuarioAdministrador();
            }
            else
            {
                return RealizaLoginInterno(usuario, senha);
            }
        }

        private static Usuario RealizaLoginInterno(string usuario, string senha)
        {
            return new Usuario()
            {
                Ativo = true,
                Email = "usuario",
                Id = 1,
                Nome = "Pegar do banco",
                Senha = String.Empty
            };
        }

        private static bool PreValidaUsuario(string usuario, string senha)
        {
            return !String.IsNullOrWhiteSpace(usuario) && !String.IsNullOrWhiteSpace(senha);
        }

        private static Usuario CriaUsuarioAdministrador()
        {
            return new Usuario()
            {
                Ativo = true,
                Email = "admin",
                Id = -1,
                Nome = "Administrador",
                Senha = String.Empty
            };
        }

        public static Usuario Registrar(string email, string senha, string nome)
        {
            //Valida se pode registar o usuário
            if (!PodeRegistrarUsuario())
                return null;

            //
            Usuario result = new Usuario()
            {
                Nome = nome,
                Senha = senha,
                Ativo = true,
                Email = email,
                Id = 1
            };

            return result;
        }

        private static bool PodeRegistrarUsuario()
        {
            return false;
        }
    }
}
