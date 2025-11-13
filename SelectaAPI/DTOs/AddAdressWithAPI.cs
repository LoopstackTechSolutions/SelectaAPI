namespace SelectaAPI.DTOs
{
    public class AddAdressWithAPI
    {
        public string Cep { get; set; }
        public string Logradouro { get; set; }
        public string Complemento { get; set; }
        public string Bairro { get; set; }
        public string Localidade { get; set; }
        public string Uf { get; set; }
        public int IdCliente { get; set; }
        public bool IsPrincipal { get; set; } = true;
    }
}
