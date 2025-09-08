namespace SelectaAPI.DTOs
{
    public class TypeAccountOfClientDTO
    {
        public int IdCliente {get; set;}
        public bool? isVendedor {get; set;} = false; 
        public bool? isEntregador { get; set; } = false;
        public string Nome { get; set;  }
    }
}
