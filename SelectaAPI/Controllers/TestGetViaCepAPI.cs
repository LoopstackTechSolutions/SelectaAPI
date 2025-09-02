using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SelectaAPI.DTOs;
using SelectaAPI.Integracao.Interfaces;

namespace SelectaAPI.Controllers
{
    [Route("selectaAPI/[controller]")]
    [ApiController]
    public class TestGetViaCepAPI : ControllerBase
    {
        private readonly IViaCepIntegracao _viaCepIntegracao;
        public TestGetViaCepAPI(IViaCepIntegracao viaCepIntegracao)
        {
            _viaCepIntegracao = viaCepIntegracao;
        }

        [HttpGet("{cep}")]
        public async Task<ActionResult<AddAdressWithAPI>> DataList([FromRoute]string cep)
        {
            var responseData = await _viaCepIntegracao.GetDataViaCep(cep);

            if (responseData == null)
            {
                return BadRequest("Cep não encontrado");
            }
            return Ok(responseData);
        }
    }
}
