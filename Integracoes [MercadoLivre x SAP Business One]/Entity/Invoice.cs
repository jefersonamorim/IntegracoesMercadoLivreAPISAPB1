using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegracoesML.Entity
{
    class Invoice
    {

        internal Invoice()
        {
            this.type = "Output";
        }

        public string type { get; set; }
        public string issuanceDate { get; set; }
        public string invoiceNumber { get; set; }
        public string invoiceValue { get; set; }
        public string invoiceKey { get; set; }
        public string invoiceUrl { get; set; }
        public string courier { get; set; }
        public string trackingNumber { get; set; }
        public string trackingUrl { get; set; }
        public List<ItemNF> items { get; set; }
    }
    public class ItemNF
    {
        public string id { get; set; }
        public int price { get; set; }
        public int quantity { get; set; }
    }


}
