using GCSEntities.Repositorios;
using System;
using System.Collections.Generic;
using System.Text;

namespace GCSEntities.Classes
{
    public static class Factory
    {
        public static UsuarioRepositorio UsuarioRepositorio()
        {
            return new UsuarioRepositorio();
        }
    }
}
