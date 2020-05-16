namespace IntegracoesML.Entity
{
    public class EnderecoCliente
    {
        //ID do endereço
        public int id { get; set; } //em branco

        //ID do cliente
        public int userId { get; set; }

        //nome do endereço
        public string addressName { get; set; }

        //tipo do endereço
        public string addressType { get; set; } // "residential" ou "commercial"

        //país
        public string country { get; set; } //"BRA" para Brasil 

        //estado (UF)
        public string state { get; set; }

        //cidade
        public string city { get; set; }

        //bairro
        public string neighborhood { get; set; }

        //CEP
        public string postalCode { get; set; }

        //endereço
        public string street { get; set; }

        //número
        public string number { get; set; }

        //Complemento 
        public string complement { get; set; }

        //campo não usado
        public string reference { get; set; } //em branco

        //destinatário
        public string receiverName { get; set; }
    }
}