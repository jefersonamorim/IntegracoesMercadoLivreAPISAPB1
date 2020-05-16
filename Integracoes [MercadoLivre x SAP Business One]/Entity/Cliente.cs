using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegracoesML.Entity
{
    public class Cliente
    {
        public string isCorporate { get; set; }
        public string tradeName { get; set; }
        public string rclastcart { get; set; }
        public string rclastcartvalue { get; set; }
        public string rclastsession { get; set; }
        public string rclastsessiondate { get; set; }
        public string homePhone { get; set; }
        public string phone { get; set; }
        //public string brandPurchasedTag { get; set; }
        //public string brandVisitedTag { get; set; }
        //public string categoryPurchasedTag { get; set; }
        //public string categoryVisitedTag { get; set; }
        //public string departmentVisitedTag { get; set; }
        //public string productPurchasedTag { get; set; }
        //public string productVisitedTag { get; set; }
        public string stateRegistration { get; set; }
        public string email { get; set; }
        public string userId { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string document { get; set; }
        public bool isNewsletterOptIn { get; set; }
        public string localeDefault { get; set; }
        public string attach { get; set; }
        public string approved { get; set; }
        public string birthDate { get; set; }
        public string businessPhone { get; set; }
       // public string carttag { get; set; }
        //public string checkouttag { get; set; }
        public string corporateDocument { get; set; }
        public string corporateName { get; set; }
        public string documentType { get; set; }
        public string gender { get; set; }
        //public string visitedProductWithStockOutSkusTag { get; set; }
        public string customerClass { get; set; }
        public string priceTables { get; set; }
        public string id { get; set; }
        public string accountId { get; set; }
        public string accountName { get; set; }
        public string dataEntityId { get; set; }
        public string createdBy { get; set; }
        public DateTime createdIn { get; set; }
        public string updatedBy { get; set; }
        public string updatedIn { get; set; }
        public string lastInteractionBy { get; set; }
        public DateTime lastInteractionIn { get; set; }
        public string[] followers { get; set; }
        //public string[] tags { get; set; }
        public string auto_filter { get; set; }


        /*public string approved { get; set; }

        //Atualizado em
        public string updatedIn { get; set; }

        //Atualizado por
        public string updatedBy { get; set; }

        //criado em
        public DateTime createdIn { get; set; }

        //criado por
        public string createdBy { get; set; }

        //ID do registro
        public string id { get; set; }

        //ID do cliente
        public string userId { get; set; }

        //nome
        public string firstName { get; set; }

        //sobrenome
        public string lastName { get; set; }

        //e-mail do cliente
        public string email { get; set; }

        //Tipo do documento
        public string documentType { get; set; } //Brasil = "cpf"

        //Documento (CPF)
        public string document { get; set; }

        //Telefone
        public string homePhone { get; set; }


        // TODO Se o cliente for pessoa jurídica, além dos dados acima, preencher:

        //define se o cliente é pessoa jurídica
        public string isCorporate { get; set; } //"true"

        //razão social
        public string corporateName { get; set; }

        //nome fantasia
        public string tradeName { get; set; }

        //inscrição estadual
        public string stateRegistration { get; set; }

        //CNPJ
        public string corporateDocument { get; set; }

        public Endereco endereco { get; set; }*/
    }
}
