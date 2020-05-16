using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegracoesML.Entity
{
    public class Shipments
    {
        public long id { get; set; }
        public string mode { get; set; }
        public string created_by { get; set; }
        public long order_id { get; set; }
        public float order_cost { get; set; }
        public float base_cost { get; set; }
        public string site_id { get; set; }
        public string status { get; set; }
        public string substatus { get; set; }

        public Shipping_Option shipping_option { get; set; }

        public Receiver_Address receiver_address { get; set; }
    }

    public class Receiver_Address
    {
        //public int id { get; set; }
        public string address_line { get; set; }
        public string street_name { get; set; }
        public string street_number { get; set; }
        public string comment { get; set; }
        public string zip_code { get; set; }
        public City city { get; set; }
        public State state { get; set; }
        public Country country { get; set; }
        public Neighborhood neighborhood { get; set; }
        public Municipality municipality { get; set; }
        public object agency { get; set; }
        public string[] types { get; set; }
        public float latitude { get; set; }
        public float longitude { get; set; }
        public string geolocation_type { get; set; }
        //public DateTime geolocation_last_updated { get; set; }
        public string geolocation_source { get; set; }
        public string delivery_preference { get; set; }
        public string receiver_name { get; set; }
        public string receiver_phone { get; set; }
    }

    public class City
    {
        public string id { get; set; }
        public string name { get; set; }
    }

    public class State
    {
        public string id { get; set; }
        public string name { get; set; }
    }

    public class Country
    {
        public string id { get; set; }
        public string name { get; set; }
    }

    public class Neighborhood
    {
        public object id { get; set; }
        public string name { get; set; }
    }

    public class Municipality
    {
        public object id { get; set; }
        public object name { get; set; }
    }

    public class Shipping_Option
    {
        public long id { get; set; }
        public int shipping_method_id { get; set; }
        public string name { get; set; }
        public string currency_id { get; set; }
        public float list_cost { get; set; }
        public double cost { get; set; }
        public string delivery_type { get; set; }
        public Estimated_Schedule_Limit estimated_schedule_limit { get; set; }
        public Estimated_Delivery_Time estimated_delivery_time { get; set; }
        public Estimated_Delivery_Limit estimated_delivery_limit { get; set; }
        public Estimated_Delivery_Final estimated_delivery_final { get; set; }
        public Estimated_Delivery_Extended estimated_delivery_extended { get; set; }
        public Estimated_Handling_Limit estimated_handling_limit { get; set; }
    }

    public class Estimated_Schedule_Limit
    {
        public object date { get; set; }
    }

    public class Estimated_Delivery_Time
    {
        public string type { get; set; }
        public DateTime date { get; set; }
        public string unit { get; set; }
        //public Offset offset { get; set; }
        public Time_Frame time_frame { get; set; }
        public object pay_before { get; set; }
        public int shipping { get; set; }
        public int handling { get; set; }
        public object schedule { get; set; }
    }

    public class Offset
    {
        public DateTime date { get; set; }
        public int shipping { get; set; }
    }

    public class Time_Frame
    {
        public object from { get; set; }
        public object to { get; set; }
    }

    public class Estimated_Delivery_Limit
    {
        public DateTime date { get; set; }
        public int offset { get; set; }
    }

   



}
