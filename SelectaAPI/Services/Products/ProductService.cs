using Microsoft.EntityFrameworkCore;
using SelectaAPI.Database;
using SelectaAPI.DTOs;
using SelectaAPI.Models;
using SelectaAPI.Repositories.Interfaces.ProductsInterface;
using SelectaAPI.Repository.Interfaces;
using SelectaAPI.Services.Interfaces;
using SelectaAPI.Services.Interfaces.ProductsInterface;

namespace SelectaAPI.Services.Products
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IFilesUploadAWSService _aws;
        private readonly IHomeRepository _homeRepository;

        public ProductService(IProductRepository productRepository, IFilesUploadAWSService aws, ApplicationDbContext context, IHomeRepository homeRepository)
        {
            _productRepository = productRepository;
            _aws = aws;
            _homeRepository = homeRepository;
        }

        public async Task<AddImageOfProductDTO> AdicionarImagemNoProduto(AddImageOfProductDTO addImageDTO)
        {
            var addImage = await _productRepository.AdicionarImagemNoProduto(addImageDTO);

            return addImage;
        }

        public async Task<EditProductDTO> EditarProduto(int idProduto, EditProductDTO editProductDTO)
        {
            var verifyIdProduct = await _homeRepository.VerificarSeProdutoExiste(idProduto);

            if (!verifyIdProduct) throw new Exception("ID do produto não existente");

            var callMethodEdit = await _productRepository.EditarProduto(idProduto, editProductDTO);

            return callMethodEdit;
        }

        public async Task<EditPromotionResponseDTO> EditarPromocao(EditPromotionRequestDTO editPromotionRequest, int idPromocao)
        {
            var editPromotion = await _productRepository.EditarPromocao(editPromotionRequest, idPromocao);
            return editPromotion;
        }

        public async Task<IEnumerable<string>> BuscarTodasAsImagensDoProduto(int idProduto)
        {
            var s3Keys = await _productRepository.BuscarTodasAsImagensDoProduto(idProduto);

            if (s3Keys == null || !s3Keys.Any())
                return Enumerable.Empty<string>();

            return await _aws.GetAllImages(s3Keys);
        }

        public async Task<string?> BuscarImagemPrincipalDoProduto(int idProduto)
        {
            var s3Key = await _productRepository.BuscarImagemPrincipalDoProduto(idProduto);
            if (s3Key == null) return null;

            return await _aws.GetImage(s3Key);
        }

        public async Task<IEnumerable<tbProdutoModel>> PesquisarProdutos(string query)
        {
            var pesquisa = await _productRepository.PesquisarProdutos(query);

            if (pesquisa == null) throw new ArgumentException("Nenhum produto encontrado!");

            return pesquisa;    
        }

        public async Task<AddProductDTO> CadastrarProduto(AddProductDTO addProductDTO)
        {
            var addProduct = await _productRepository.CadastrarProduto(addProductDTO);

            return addProduct;
        }


        public async Task<AddPromotionResponseDTO> CadastrarPromocao(AddPromotionRequestDTO addPromotionRequest)
        {
            var addPromotion = await _productRepository.CadastrarPromocao(addPromotionRequest);

            return new AddPromotionResponseDTO
            {
                IdProduto = addPromotion.IdProduto,
                ValorAnterior = addPromotion.ValorAnterior,
                ValorDesconto = addPromotion.ValorDesconto,
                NovoValor = addPromotion.ValorDesconto * addPromotion.ValorAnterior / 100,
                Status = addPromotion.Status,
                Validade = addPromotion.Validade,
            };
        }

        public async Task RemoverProduto(int idProduto)
        {
            var product = await _productRepository.BuscarProdutosPorId(idProduto);

            if (product == null)
                throw new ArgumentException("Produto não encontrado.");

            await _productRepository.RemoverProduto(product);
        }

        public async Task RemoverPromocao(int idPromocao)
        {
            var promotion = await _productRepository.BuscarPromocaoPorId(idPromocao);

            if(promotion == null)
                throw new ArgumentException("Promoção não encontrada");
            await _productRepository.RemoverPromocao(promotion);
        }
    }
}
