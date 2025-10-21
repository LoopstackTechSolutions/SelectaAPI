using Microsoft.EntityFrameworkCore;
using SelectaAPI.Database;
using SelectaAPI.DTOs;
using SelectaAPI.Models;
using SelectaAPI.Repositories.Interfaces.ProductsInterface;
using SelectaAPI.Services;
using SelectaAPI.Services.Interfaces;

namespace SelectaAPI.Repositories.Products
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IFilesUploadAWSService _aws;
        public ProductRepository(ApplicationDbContext context, IFilesUploadAWSService aws)
        {
            _context = context;
            _aws = aws;
        }

        public async Task<tbProdutoModel> ProductRegister(IFormFile file, string? prefix, AddProductDTO addProductDTO)
        {
            var image = await _aws.UploadFiles(file, prefix);

            _context.produtos.Add(new tbProdutoModel
            {
                Nome = addProductDTO.Nome,
                Nota = addProductDTO.Nota,
                Condicao = addProductDTO.Condicao,
                Descricao = addProductDTO.Descricao,
                PrecoUnitario = addProductDTO.PrecoUnitario,
                Peso = addProductDTO.Peso,
                Quantidade = addProductDTO.Quantidade,
                Vendedor = addProductDTO.Vendedor,
                Status = addProductDTO.Status,
                IdVendedor = addProductDTO.IdVendedor,
            });

        }

        public async Task<AddPromotionResponseDTO> PromotionRegister(AddPromotionRequestDTO addPromotionRequest)
        {
            var getProduct = await _context.produtos.Where(p => p.IdProduto == addPromotionRequest.IdProduto)
                .Select(p => p.PrecoUnitario)
                .FirstOrDefaultAsync();
            var addPromotionEntity = new tbPromocaoModel()
            {
                Desconto = addPromotionRequest.Desconto,
                IdProduto = addPromotionRequest.IdProduto,
                Status = addPromotionRequest.Status,
                ValidaAte = addPromotionRequest.Validade
            };

            await _context.promocoes.AddAsync(addPromotionEntity);
            await _context.SaveChangesAsync();

            return new AddPromotionResponseDTO
            {
                ValorDesconto = addPromotionRequest.Desconto,
                IdProduto = addPromotionRequest.IdProduto,
                Status = addPromotionRequest.Status,
                Validade = addPromotionRequest.Validade,
                ValorAnterior = getProduct
            };
        }

    }
}
