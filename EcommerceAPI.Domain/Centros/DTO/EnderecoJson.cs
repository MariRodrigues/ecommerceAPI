﻿namespace EcommerceAPI.Domain.Centros.DTO
{
    public class EnderecoJson
    {
        public string CEP { get; set; }
        public string Logradouro { get; set; }
        public string Bairro { get; set; }
        public string Localidade { get; set; }
        public string UF { get; set; }
        public string Erro { get; set; }
    }
}
