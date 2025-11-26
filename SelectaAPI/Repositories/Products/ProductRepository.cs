using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using SelectaAPI.Database;
using SelectaAPI.DTOs;
using SelectaAPI.Handlers;
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

        public async Task<AddImageOfProductDTO> AdicionarImagemNoProduto(/*IFormFile file, string? prefix,*/ AddImageOfProductDTO addImageDTO)
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

        public async Task<EditProductDTO> EditarProduto(int idProduto, EditProductDTO editProductDTO)
        {
            var editProduct = await _context.produtos.Where(p => p.IdProduto == idProduto)
                .FirstOrDefaultAsync();
            editProduct.Condicao = editProductDTO.Condicao;
            editProduct.Descricao = editProductDTO.Descricao ?? editProduct.Descricao;
            editProduct.Nome = editProductDTO.Nome ?? editProduct.Nome;
            editProduct.Peso = editProductDTO.Peso ?? editProduct.Peso;
            editProduct.PrecoUnitario = editProductDTO.PrecoUnitario;
            editProduct.Quantidade = editProductDTO.Quantidade ?? editProduct.Quantidade;
            editProduct.Status = editProductDTO.Status ?? editProduct.Status;
            await _context.SaveChangesAsync();

            return editProductDTO;
        }

        public async Task<IEnumerable<string>> BuscarTodasAsImagensDoProduto(int idProduto)
        {
            var image = await _context.imagensProdutos.Where(i => i.IdProduto == idProduto)
                .Select(i => i.S3Key).Take(5)
                .ToListAsync(); 
            return image;
        }

        public async Task<string?> BuscarImagemPrincipalDoProduto(int idProduto)
        {
            var image = await _context.imagensProdutos.Where(i => i.IdProduto == idProduto && i.IsPrincipal == true)
                .Select(i => i.S3Key)
                .FirstOrDefaultAsync();

            return image;
        }

        public async Task<AddProductDTO> CadastrarProduto(AddProductDTO addProductDTO)
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

        public async Task<AddPromotionResponseDTO> CadastrarPromocao(AddPromotionRequestDTO addPromotionRequest)
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

        public async Task<tbProdutoModel> BuscarProdutosPorId(int idProduto)
        {
            return await _context.produtos.FindAsync(idProduto);
        }

        public async Task RemoverProduto(tbProdutoModel produtoModel)
        {
            var removeProduct = _context.produtos.Remove(produtoModel);
            await _context.SaveChangesAsync();
        }

        public async Task<EditPromotionResponseDTO> EditarPromocao(EditPromotionRequestDTO editPromotionRequest, int idPromocao)
        {
            var editProduct = await _context.promocoes.Where(c => c.IdPromocao == idPromocao)
              .FirstOrDefaultAsync();
            if (!string.IsNullOrWhiteSpace(editPromotionRequest.Status))
                editProduct.Status = editPromotionRequest.Status.Trim();

            if ((editPromotionRequest.ValidaAte) != null)
                editProduct.ValidaAte = editPromotionRequest.ValidaAte;

            if ((editPromotionRequest.Desconto) != null)
                editProduct.Desconto = editPromotionRequest.Desconto;

            await _context.SaveChangesAsync();

            return new EditPromotionResponseDTO
            {
                ValorDesconto = editPromotionRequest.Desconto,
                Status = editPromotionRequest.Status,
                Validade = editPromotionRequest.ValidaAte,
            };
        }

        public async Task RemoverPromocao(tbPromocaoModel promocaoModel)
        {
            var removePromotion = _context.promocoes.Remove(promocaoModel);
            await _context.SaveChangesAsync();
        }

        public async Task<tbPromocaoModel> BuscarPromocaoPorId(int idPromocao)
        {
            return await _context.promocoes.FindAsync(idPromocao);
        }

        public async Task<IEnumerable<tbProdutoModel>> PesquisarProdutos(string query)
        {
            var pesquisa = await _context.produtos.Where(p => EF.Functions.Like(p.Nome, $"%{query}%"))
            .ToListAsync();

            return pesquisa;
        }

        public async Task<decimal> PrecoDoProduto(int idProduto)
        {
            var buscarPreco = await _context.produtos.Where(p => p.IdProduto == idProduto)
                .Select(p => p.PrecoUnitario).FirstOrDefaultAsync();
            return buscarPreco;
        }

        public async Task<bool> VerificarEstoque(int idProduto)
        {
            return await _context.produtos.AnyAsync(p => p.IdProduto == idProduto && p.Quantidade > 0);
        }

       public async Task<bool> QuantidadeSelecionada(int idProduto, int quantidade)
        {
            return await _context.produtos.AnyAsync(p => p.IdProduto == idProduto && p.Quantidade > quantidade);
        }

        public async Task<bool> VerificarStatusDoProduto(int idProduto)
        {
            return await _context.produtos.AnyAsync(p => p.IdProduto == idProduto && (p.Status == "disponivel" || p.Status == "ativo"));
        }
    }
}
