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

            if (string.IsNullOrWhiteSpace(addClientDTO.Nome) ||
                string.IsNullOrWhiteSpace(addClientDTO.Email) ||
                string.IsNullOrWhiteSpace(addClientDTO.Senha))
            {
                return BadRequest("Preencha todos os campos obrigatórios: Nome, Email e Senha.");
            }

            var entityClient = new tbClienteModel()
            {
                Nome = addClientDTO.Nome.Trim(),
                Email = addClientDTO.Email.Trim(),
                Senha = addClientDTO.Senha.Trim()
            };

            try
            {
                var verification = await _context.clientes
                    .FirstOrDefaultAsync(c => c.Email == entityClient.Email);

                if (verification != null)
                    return BadRequest("Este e-mail já está cadastrado.");

                await _context.clientes.AddAsync(entityClient);
                await _context.SaveChangesAsync();

                return Ok("Cliente cadastrado com sucesso!");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }
        [HttpPost("employee-register")]
        public async Task<IActionResult> EmployeeRegister(AddEmployeeDTO addEmployeeDTO)
        {
            if (addEmployeeDTO == null) return StatusCode(400, "preencha os campos");

            if (string.IsNullOrEmpty(addEmployeeDTO.Nome) || string.IsNullOrEmpty(addEmployeeDTO.Email)
                || string.IsNullOrEmpty(addEmployeeDTO.Cpf) || string.IsNullOrEmpty(addEmployeeDTO.Senha)
                || string.IsNullOrEmpty(addEmployeeDTO.NivelAcesso)
                )
            {
                return StatusCode(400, "Necessário preencher todas as informações");
            }

            if (!addEmployeeDTO.Cpf.All(char.IsDigit) || addEmployeeDTO.Cpf.Length < 11 || addEmployeeDTO.Cpf.Length > 11) return StatusCode(400, "CPF inválido");

            var entityEmployee = new tbFuncionarioModel()
            {
                Nome = addEmployeeDTO.Nome.Trim(),
                Senha = addEmployeeDTO.Senha.Trim(),
                Email = addEmployeeDTO.Email.Trim(),
                Cpf = addEmployeeDTO.Cpf.Trim(),
                NivelAcesso = addEmployeeDTO.NivelAcesso.Trim()
            };

            try
            {
                var verification = await _context.funcionarios.FirstOrDefaultAsync(f => f.Cpf == entityEmployee.Cpf || f.Email == entityEmployee.Email);
                if (verification != null) return StatusCode(400, "Cpf ou Email ja cadastrados no sistema");

                await _context.funcionarios.AddAsync(entityEmployee);
                await _context.SaveChangesAsync();
                return StatusCode(200, "sucesso ao cadastrar funcionário");
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, $"Erro de banco: {ex.InnerException?.Message ?? ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"erro no servidor {ex.Message}");
            }
        }
        /*
        [HttpPost("address-register")]
        public async Task<IActionResult> AddressRegister()
        {

        }
        */

        [HttpPost("category-client-register")]
        public async Task<IActionResult> CategoryClientRegister(AddCategory_ClientDTO addCategoryClient)
        {
            try
            {
                var cliente = await _context.clientes.FindAsync(addCategoryClient.IdCliente);
                if (cliente == null)
                    return NotFound("Cliente não encontrado.");

                var categoria = await _context.categorias.FindAsync(addCategoryClient.IdCategoria);
                if (categoria == null)
                    return NotFound("Categoria não encontrada.");

                var exists = await _context.categoriaClientes
                    .AnyAsync(cc => cc.IdCliente == addCategoryClient.IdCliente && cc.IdCategoria == addCategoryClient.IdCategoria);

                if (exists)
                    return BadRequest("Essa categoria já foi vinculada ao cliente.");

                var categoriaCliente = new tbCategoria_Cliente
                {
                    IdCategoria = addCategoryClient.IdCategoria,
                    IdCliente = addCategoryClient.IdCliente
                };

                _context.categoriaClientes.Add(categoriaCliente);
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    Message = "Categoria vinculada ao cliente com sucesso!",
                    categoriaCliente.IdCategoria,
                    categoriaCliente.IdCliente
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        }
    }
