using Microsoft.EntityFrameworkCore;
using SelectaAPI.Database;
using SelectaAPI.DTOs;
using SelectaAPI.Models;
using SelectaAPI.Repositories.Interfaces.UsersInterface;

namespace SelectaAPI.Repositories.Users
{
    public class SalesPersonRepository : ISalesPersonRepository
    {
        private readonly ApplicationDbContext _context;
        public SalesPersonRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<ResponseMyProductsDTO>> MeusProdutos(int idVendedor, int pageNumber = 1, int pageSize = 20)
        {
            var lista = await _context.produtos.Where(p => p.IdVendedor == idVendedor)
                .Select(p => new ResponseMyProductsDTO
                {
                    IdVendedor = p.IdVendedor,
                    Condicao = p.Condicao,
                    Nome = p.Nome,
                    Descricao = p.Descricao,
                    Nota = p.Nota,
                    Peso = p.Peso,
                    PrecoUnitario = p.PrecoUnitario,
                    Quantidade = p.Quantidade,
                    Status = p.Status
                }).AsNoTracking()
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            return lista;
        }

        public async Task<bool> RetornoVazio(int idVendedor)
        {
            return await _context.produtos.AnyAsync(p => p.IdVendedor == idVendedor);
        }
    }
}
