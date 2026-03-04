using BarberFlow.API.Data.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BarberFlow.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AgendamentoController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;
        public AgendamentoController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
    }
}
