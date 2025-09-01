using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SelectaAPI.Models
{
    [Table("tbEndereco")]
    public class tbEnderecoModel
    {
        [Key]
        public int IdEndereco { get; set; }
        public int Cep { get; set; }
        public string Logradouro { get; set; }
        public bool isPrincipal { get; set; } = true;
        public int IdCliente { get; set; }
        public tbClienteModel Cliente { get; set; }
        public string Complemento { get; set; }
        public string Unidade { get; set; }
        public string Bairro { get; set; }
        public string Localidade { get; set; }
        public string Uf { get; set; }
        public string Estado { get; set; }
        public string Regiao { get; set; }
        public string Ibge { get; set; }
        public string Gia { get; set; }
        public string Ddd { get; set; }
        public string Siafi { get; set; }
    }
}
