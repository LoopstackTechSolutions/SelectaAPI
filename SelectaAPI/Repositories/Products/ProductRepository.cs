using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
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

        public async Task<AddImageOfProductDTO> AddImageOfProduct(/*IFormFile file, string? prefix,*/ AddImageOfProductDTO addImageDTO)
        {
            var awsCommunication = await _aws.UploadFiles(addImageDTO.File, addImageDTO.Prefix);

            var addImageEntity = new tbImagem_ProdutoModel()
            {
                IdProduto = addImageDTO.IdProduto,
                IsPrincipal = addImageDTO.Principal,
                S3Key = awsCommunication.S3Key,
            };
            await _context.imagensProdutos.AddAsync(addImageEntity);
            await _context.SaveChangesAsync();
            return addImageDTO;
        }

        public async Task<IEnumerable<string>> GetAllImagesOfProduct(int idProduto)
        {
            var image = await _context.imagensProdutos.Where(i => i.IdProduto == idProduto)
                .Select(i => i.S3Key)
                .ToListAsync(); 
            return image;
        }

        public async Task<string?> GetPrincipalImage(int idProduto)
        {
            var image = await _context.imagensProdutos.Where(i => i.IdProduto == idProduto && i.IsPrincipal == true)
                .Select(i => i.S3Key)
                .FirstOrDefaultAsync();

            return image;
        }

        public async Task<AddProductDTO> ProductRegister(AddProductDTO addProductDTO)
        {
            var addProductEntity =(new tbProdutoModel()
            {
                Nome = addProductDTO.Nome,
                Condicao = addProductDTO.Condicao,
                Descricao = addProductDTO.Descricao,
                PrecoUnitario = addProductDTO.PrecoUnitario,
                Peso = addProductDTO.Peso,
                Quantidade = addProductDTO.Quantidade,
                Status = addProductDTO.Status,
                IdVendedor = addProductDTO.IdVendedor
            });
            await _context.produtos.AddAsync(addProductEntity);
            await _context.SaveChangesAsync();

            return addProductDTO;
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
