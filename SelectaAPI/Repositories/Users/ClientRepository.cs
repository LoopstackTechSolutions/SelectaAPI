using Microsoft.EntityFrameworkCore;
using SelectaAPI.Database;
using SelectaAPI.DTOs;
using SelectaAPI.Handlers;
using SelectaAPI.Models;
using SelectaAPI.Repositories.Interfaces.UsersInterface;

namespace SelectaAPI.Repositories.Users
{
    public class ClientRepository : IClientRepository
    {
        private readonly ApplicationDbContext _context;

        public ClientRepository(ApplicationDbContext context)
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
                IdCategoria = categoriaCliente.IdCategoria,
                IdCliente = categoriaCliente.IdCliente
            });
        }

        public async Task<AddClientDTO> ClientRegister(AddClientDTO addClientDTO)
        {
            var entityClient = new tbClienteModel()
            {
                Nome = addClientDTO.Nome.Trim(),
                Email = addClientDTO.Email.Trim(),
                Senha = addClientDTO.Senha.Trim(), 
            };

            var verification = await _context.clientes
                .FirstOrDefaultAsync(c => c.Email == entityClient.Email);
            await _context.clientes.AddAsync(entityClient);
            await _context.SaveChangesAsync();

            return addClientDTO;
        }

        public async Task<EditClientDTO> EditClient(int idCliente,EditClientDTO editClienteDTO)
        {
            var editClient = await _context.clientes.Where(c => c.IdCliente == idCliente)
                .FirstOrDefaultAsync();
            if (!string.IsNullOrWhiteSpace(editClienteDTO.Nome))
                editClient.Nome = editClienteDTO.Nome.Trim();

            if (!string.IsNullOrWhiteSpace(editClienteDTO.Email))
                editClient.Email = editClienteDTO.Email.Trim().ToLower();

            if (!string.IsNullOrWhiteSpace(editClienteDTO.Senha))
            {
                string hash = PasswordHashHandler.HashPassword(editClienteDTO.Senha.Trim());
                editClient.Senha = hash;
            }
            await _context.SaveChangesAsync();
            return editClienteDTO;
        }
    }
}
