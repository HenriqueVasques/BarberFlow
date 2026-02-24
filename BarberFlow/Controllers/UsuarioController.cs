using BarberFlow.API.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BarberFlow.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;
        public UsuarioController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
    }
}
