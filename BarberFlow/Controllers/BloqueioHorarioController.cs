using BarberFlow.API.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BarberFlow.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BloqueioHorarioController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;

        public BloqueioHorarioController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
    }
}
