using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace EfCo.UowRepository.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DepartamentoController : ControllerBase
    {
        
        private readonly ILogger<DepartamentoController> _logger;
        private readonly IDepartamentoRepository _departamentoRepository;
        public DepartamentoController(ILogger<DepartamentoController> logger,
        IDepartamentoRepository repository)
        {
            _logger = logger;
            _departamentoRepository = repository;
        }
        //departamento/1
        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id /*[FromServices]IDepartamentoRepository repository*/)
        {
           var departamento = await _departamentoRepository.GetByIdAsync(id);

           return Ok(departamento);
        }
    }
}
