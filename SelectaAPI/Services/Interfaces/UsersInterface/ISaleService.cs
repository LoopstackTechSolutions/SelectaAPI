using SelectaAPI.DTOs;
using SelectaAPI.Models;

namespace SelectaAPI.Services.Interfaces.UsersInterface
{
    public interface ISaleService
    {
        Task<PedidoResponseDTO> ComprarProduto(PedidoDTO pedidoDTO);

    }
}
