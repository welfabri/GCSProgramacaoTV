using GCSEntities.Classes;
using GCSEntities.Repositorios;
using System.Collections.Generic;
using System.Linq;

namespace GCSSite.Models.Services
{
    public static class UsuarioService
    {
        internal static void Delete(int id)
        {
            Factory.UsuarioRepositorio().Delete(id);
        }

        internal static void Update(int id, Usuario value)
        {
            Factory.UsuarioRepositorio().Update(id, value);
        }

        internal static void Insert(Usuario value)
        {
            Factory.UsuarioRepositorio().Insert(value);
        }

        internal static IEnumerable<Usuario> GetActives()
        {
            return Factory.UsuarioRepositorio().GetAll().Where(x => x.Ativo);
        }

        internal static Usuario Get(int id)
        {
            return Factory.UsuarioRepositorio().Get(id);
        }
    }
}
