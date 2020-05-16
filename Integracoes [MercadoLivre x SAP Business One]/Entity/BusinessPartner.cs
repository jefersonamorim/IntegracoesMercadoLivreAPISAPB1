using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegracoesML.Entity
{
    public class BusinessPartner
    {

        public string CardCode { get; set; }

        public string CardName { get; set; }

        public int GroupCode { get; set; }

        public string SplCode { get; set; }

        public string OperacaoConsumidor { get; set; }

        public string Phone1 { get; set; }

        public string EmailAddress { get; set; }

        public string cpf { get; set; }

        public EnderecoCliente endereco;
    }
}
