using SelectaAPI.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace SelectaAPI.DTOs
{
    public class ResponseMyProductsDTO
    {
        public string Nome { get; set; }
        public int? Quantidade { get; set; }
        public decimal PrecoUnitario { get; set; }
        public bool Condicao { get; set; }
        public int? Peso { get; set; }
        public string? Status { get; set; }
        public int? Nota { get; set; }
        public string? Descricao { get; set; }
        public int? IdVendedor { get; set; }

    }
}
