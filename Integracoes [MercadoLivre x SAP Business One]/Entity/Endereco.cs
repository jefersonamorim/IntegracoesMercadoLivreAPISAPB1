using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegracoesML.Entity
{
    public class Endereco
    {
        /*
        //ID do endereço
        public string id { get; set; } //em branco

        //ID do cliente
        public string userId { get; set; }

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

        */

            public string addressName { get; set; }
            public string addressType { get; set; }
            public string city { get; set; }
            public string complement { get; set; }
            public string country { get; set; }
            public object countryfake { get; set; }
            public float[] geoCoordinate { get; set; }
            public string neighborhood { get; set; }
            public string number { get; set; }
            public string postalCode { get; set; }
            public string receiverName { get; set; }
            public object reference { get; set; }
            public string state { get; set; }
            public string street { get; set; }
            public string userId { get; set; }
            public string id { get; set; }
            public string accountId { get; set; }
            public string accountName { get; set; }
            public string dataEntityId { get; set; }
            public string createdBy { get; set; }
            public DateTime createdIn { get; set; }
            public object updatedBy { get; set; }
            public object updatedIn { get; set; }
            public string lastInteractionBy { get; set; }
            public DateTime lastInteractionIn { get; set; }
            public object[] followers { get; set; }
            public object[] tags { get; set; }
            public object auto_filter { get; set; }
        
    }
}
