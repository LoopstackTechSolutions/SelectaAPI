using Microsoft.EntityFrameworkCore;
using SelectaAPI.Database;
using SelectaAPI.DTOs;
using SelectaAPI.Models;
using SelectaAPI.Repository.Interfaces;

namespace SelectaAPI.Repository
{
    public class RegistrationRepository : IRegistrationRepository
    {
        private readonly ApplicationDbContext _context;

        public RegistrationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<AddCategory_ClientDTO> CategoryClientRegister(AddCategory_ClientDTO addCategoryClientDTO)
        {
                var cliente = await _context.clientes.FindAsync(addCategoryClientDTO.IdCliente);

                var categoria = await _context.categorias.FindAsync(addCategoryClientDTO.IdCategoria);

                var exists = await _context.categoriaClientes
                    .AnyAsync(cc => cc.IdCliente == addCategoryClientDTO.IdCliente && cc.IdCategoria == addCategoryClientDTO.IdCategoria);

                var categoriaCliente = new tbCategoria_Cliente
                {
                    IdCategoria = addCategoryClientDTO.IdCategoria,
                    IdCliente = addCategoryClientDTO.IdCliente
                };

                _context.categoriaClientes.Add(categoriaCliente);
                await _context.SaveChangesAsync();

                return (new AddCategory_ClientDTO
                {
                  IdCategoria =  categoriaCliente.IdCategoria,
                    IdCliente = categoriaCliente.IdCliente
                });
            }

        public async Task<AddClientDTO> ClientRegister(AddClientDTO addClientDTO)
        {
            var entityClient = new tbClienteModel()
            {
                Nome = addClientDTO.Nome.Trim(),
                Email = addClientDTO.Email.Trim(),
                Senha = addClientDTO.Senha.Trim()
            };

            var verification = await _context.clientes
                .FirstOrDefaultAsync(c => c.Email == entityClient.Email);
            await _context.clientes.AddAsync(entityClient);
            await _context.SaveChangesAsync();

            return addClientDTO;
        }

        public async Task<AddEmployeeDTO> EmployeeRegister(AddEmployeeDTO addEmployeeDTO)
        {
            var entityEmployee = new tbFuncionarioModel()
            {
                Nome = addEmployeeDTO.Nome.Trim(),
                Senha = addEmployeeDTO.Senha.Trim(),
                Email = addEmployeeDTO.Email.Trim(),
                Cpf = addEmployeeDTO.Cpf.Trim(),
                NivelAcesso = addEmployeeDTO.NivelAcesso.Trim()
            };
                await _context.funcionarios.AddAsync(entityEmployee);
                await _context.SaveChangesAsync();

                 return addEmployeeDTO;
            }

        /*
        public async Task<IEnumerable<GetClientDTO>> GetClient()
        {
            var getClient = await _context.clientes.Select(c => new GetClientDTO{
                Email = c.Email 
            }).ToListAsync();
            return  getClient;
        }

        public async Task<IEnumerable<GetEmployeeDTO>> GetEmployee()
        {
            var getEmployee = await _context.funcionarios.Select(f => new GetEmployeeDTO
            {
                Email = f.Email,
                Cpf = f.Cpf,
            }).ToListAsync();
            return (IEnumerable<GetEmployeeDTO>)getEmployee;
        }
        */
    }
    }

