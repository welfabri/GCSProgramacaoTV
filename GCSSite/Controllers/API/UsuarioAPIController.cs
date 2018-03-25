using GCSEntities.Classes;
using GCSSite.Models.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace GCSSite.Controllers.API
{
    [Produces("application/json")]
    [Route("api/Usuario")]
    public class UsuarioAPIController : Controller
    {
        // GET: api/Usuario
        [HttpGet]
        public IEnumerable<Usuario> Get()
        {
            return UsuarioService.GetActives();
        }

        // GET: api/Usuario/5
        [HttpGet("{id}", Name = "Get")]
        public Usuario Get(int id)
        {
            return UsuarioService.Get(id);
        }
        
        // POST: api/Usuario
        [HttpPost]
        public void Post([FromBody]Usuario value)
        {
            UsuarioService.Insert(value);
        }
        
        // PUT: api/Usuario/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]Usuario value)
        {
            UsuarioService.Update(id, value);
        }
        
        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            UsuarioService.Delete(id);
        }
    }
}
