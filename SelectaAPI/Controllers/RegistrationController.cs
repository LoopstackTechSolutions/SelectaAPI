using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SelectaAPI.Database;
using SelectaAPI.Models;
using SelectaAPI.DTOs;
using Microsoft.EntityFrameworkCore;
using MySqlX.XDevAPI;


namespace SelectaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegistrationController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public RegistrationController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("client-register")]
        public async Task<IActionResult> ClientRegister(AddClientDTO addClientDTO)
        {
                if (addClientDTO == null)
                    return BadRequest("Os dados do cliente não foram enviados.");

                // validação de campos obrigatórios
                if (string.IsNullOrWhiteSpace(addClientDTO.Nome) ||
                    string.IsNullOrWhiteSpace(addClientDTO.Email) ||
                    string.IsNullOrWhiteSpace(addClientDTO.Senha))
                {
                    return BadRequest("Preencha todos os campos obrigatórios: Nome, Email e Senha.");
                }

                var entity = new tbClienteModel()
                {
                    Nome = addClientDTO.Nome.Trim(),
                    Email = addClientDTO.Email.Trim(),
                    Senha = addClientDTO.Senha.Trim()
                };

                try
                {
                    var verification = await _context.clientes
                        .FirstOrDefaultAsync(c => c.Email == entity.Email);

                    if (verification != null)
                        return BadRequest("Este e-mail já está cadastrado.");

                    await _context.clientes.AddAsync(entity);
                    await _context.SaveChangesAsync();

                    return Ok("Cliente cadastrado com sucesso!");
                }
                catch (Exception ex)
                {
                    return StatusCode(500, $"Erro interno: {ex.Message}");
                }
            }

        }
    }
