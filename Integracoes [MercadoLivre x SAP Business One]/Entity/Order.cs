using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegracoesML.Entity
{
    public class Order
    {
        public long id { get; set; }
        public DateTime date_created { get; set; }
        public DateTime date_closed { get; set; }
        public DateTime last_updated { get; set; }
        //public object manufacturing_ending_date { get; set; }
        public Feedback feedback { get; set; }
        //public object[] mediations { get; set; }
        //public object comments { get; set; }
        //public object pack_id { get; set; }
        //public object pickup_id { get; set; }
        public Order_Request order_request { get; set; }
        //public bool fulfilled { get; set; }
        public float total_amount { get; set; }
        public float total_amount_with_shipping { get; set; }
        public float paid_amount { get; set; }
        public Coupon coupon { get; set; }
        public DateTime expiration_date { get; set; }
        public Order_Items[] order_items { get; set; }
        public string currency_id { get; set; }
        public Payment[] payments { get; set; }
        public Shipping shipping { get; set; }
        public string status { get; set; }
        //public object status_detail { get; set; }
        public string[] tags { get; set; }
        public Buyer buyer { get; set; }
        public Seller seller { get; set; }
        public Taxes taxes { get; set; }
    }

    public class Feedback
    {
        public Purchase purchase { get; set; }
        //public object sale { get; set; }
    }

    public class Purchase
    {
        public long id { get; set; }
        public DateTime date_created { get; set; }
        public bool fulfilled { get; set; }
        public string rating { get; set; }
        public string status { get; set; }
    }

    public class Order_Request
    {
        //public object _return { get; set; }
        //public object change { get; set; }
    }

    public class Coupon
    {
        //public object id { get; set; }
        public int amount { get; set; }
    }


    public class Estimated_Delivery_Final
    {
        public DateTime date { get; set; }
        //public int offset { get; set; }
    }

    public class Estimated_Delivery_Extended
    {
        public DateTime date { get; set; }
        public int offset { get; set; }
    }

    public class Estimated_Handling_Limit
    {
        public DateTime date { get; set; }
    }



    public class Dimensions_Source
    {
        public string id { get; set; }
        public string origin { get; set; }
    }

    public class Buyer
    {
        public int id { get; set; }
        public string nickname { get; set; }
        public string email { get; set; }
        public Phone phone { get; set; }
        public Alternative_Phone alternative_phone { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public Billing_Info billing_info { get; set; }
    }

    public class Phone
    {
        //public object area_code { get; set; }
        public string extension { get; set; }
        public string number { get; set; }
        public bool verified { get; set; }
    }

    public class Alternative_Phone
    {
        public string area_code { get; set; }
        public string extension { get; set; }
        public string number { get; set; }
    }

    public class Billing_Info
    {
        public string doc_type { get; set; }
        public string doc_number { get; set; }
    }

    public class Seller
    {
        public int id { get; set; }
        public string nickname { get; set; }
        public string email { get; set; }
        public Phone1 phone { get; set; }
        public Alternative_Phone1 alternative_phone { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
    }

    public class Phone1
    {
        public string area_code { get; set; }
        public string extension { get; set; }
        public string number { get; set; }
        public bool verified { get; set; }
    }

    public class Alternative_Phone1
    {
        public string area_code { get; set; }
        public string extension { get; set; }
        public string number { get; set; }
    }

    public class Taxes
    {
        //public object amount { get; set; }
        //public object currency_id { get; set; }
    }

    public class Order_Items
    {
        public Item item { get; set; }
        public int quantity { get; set; }
        public float unit_price { get; set; }
        public float full_unit_price { get; set; }
        public string currency_id { get; set; }
        //public object manufacturing_days { get; set; }
    }

    public class Item
    {
        public string id { get; set; }
        public string title { get; set; }
        public string category_id { get; set; }
        //public long variation_id { get; set; }
        public string seller_custom_field { get; set; }
        public Variation_Attributes[] variation_attributes { get; set; }
        public string warranty { get; set; }
        public string condition { get; set; }
        //public object seller_sku { get; set; }
    }

    public class Variation_Attributes
    {
        //public object id { get; set; }
        public string name { get; set; }
        //public object value_id { get; set; }
        public string value_name { get; set; }
    }

    public class Payment
    {
        public long id { get; set; }
        public long order_id { get; set; }
        public int payer_id { get; set; }
        public Collector collector { get; set; }
        //public long card_id { get; set; }
        public string site_id { get; set; }
        public string reason { get; set; }
        public string payment_method_id { get; set; }
        public string currency_id { get; set; }
        public int installments { get; set; }
        public string issuer_id { get; set; }
        public Atm_Transfer_Reference atm_transfer_reference { get; set; }
        //public object coupon_id { get; set; }
        //public object activation_uri { get; set; }
        public string operation_type { get; set; }
        public string payment_type { get; set; }
        public string[] available_actions { get; set; }
        public string status { get; set; }
        //public object status_code { get; set; }
        public string status_detail { get; set; }
        public float transaction_amount { get; set; }
        public int taxes_amount { get; set; }
        //public int shipping_cost { get; set; }
        public int coupon_amount { get; set; }
        public int overpaid_amount { get; set; }
        public float total_paid_amount { get; set; }
        //public float installment_amount { get; set; }
        //public object deferred_period { get; set; }
        ///public DateTime date_approved { get; set; }
        public string authorization_code { get; set; }
        //public object transaction_order_id { get; set; }
        public DateTime date_created { get; set; }
        public DateTime date_last_modified { get; set; }
    }

    public class Collector
    {
        public int id { get; set; }
    }

    public class Atm_Transfer_Reference
    {
        //public object company_id { get; set; }
        public string transaction_id { get; set; }
    }


    public class Shipping
    {
        public float cost { get; set; }
        public string substatus { get; set; }
        public DateTime date_created { get; set; }
        public int receiver_id { get; set; }
        ////public object date_first_printed { get; set; }
        public int sender_id { get; set; }
        //public Shipping_Option shipping_option { get; set; }
        public string mode { get; set; }
        public Sender_Address sender_address { get; set; }
        public string shipping_mode { get; set; }
        //public int service_id { get; set; }
        public string site_id { get; set; }
        public Shipping_Items[] shipping_items { get; set; }
        //public Receiver_Address receiver_address { get; set; }
        public Cost_Components cost_components { get; set; }
        public long id { get; set; }
        /////public object picking_type { get; set; }
        public string currency_id { get; set; }
        public string shipment_type { get; set; }
        public string status { get; set; }
        public string logistic_type { get; set; }
    }


    public class Sender_Address
    {
        public Country country { get; set; }
        public string address_line { get; set; }
        public string[] types { get; set; }
        ////public object agency { get; set; }
        //public City city { get; set; }
        public string geolocation_type { get; set; }
        public float latitude { get; set; }
        //public Municipality municipality { get; set; }
        public string street_name { get; set; }
        public string zip_code { get; set; }
        ////public object geolocation_source { get; set; }
        public string street_number { get; set; }
        public string comment { get; set; }
        public int id { get; set; }
        public State state { get; set; }
        public Neighborhood neighborhood { get; set; }
        ////public object geolocation_last_updated { get; set; }
        //public float longitude { get; set; }
    }

    public class Cost_Components
    {
        public int loyal_discount { get; set; }
        public int special_discount { get; set; }
        public int compensation { get; set; }
        //public int gap_discount { get; set; }
        public float ratio { get; set; }
    }

    public class Shipping_Items
    {
        public int quantity { get; set; }
        public Dimensions_Source dimensions_source { get; set; }
        public string description { get; set; }
        public string id { get; set; }
        public string dimensions { get; set; }
    }



}
