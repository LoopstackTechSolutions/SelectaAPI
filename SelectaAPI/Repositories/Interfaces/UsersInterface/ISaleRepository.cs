using SelectaAPI.DTOs;
using SelectaAPI.Models;

namespace SelectaAPI.Repositories.Interfaces.UsersInterface
{
    public interface ISaleRepository
    {
        Task<PedidoResponseDTO> ComprarProduto(PedidoDTO pedidoDTO);
    }
}
