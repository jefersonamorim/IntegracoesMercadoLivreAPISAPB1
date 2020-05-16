using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegracoesML.Entity
{
    public class MyFeedResponse
    {
        public Message[] messages { get; set; }
    }

    public class Message
    {
        public string _id { get; set; }
        public string resource { get; set; }
        public int user_id { get; set; }
        public string topic { get; set; }
        public long application_id { get; set; }
        public int attempts { get; set; }
        public DateTime sent { get; set; }
        public DateTime received { get; set; }
        public Request request { get; set; }
        public Response response { get; set; }
    }

    public class Request
    {
        public string url { get; set; }
        public Headers headers { get; set; }
        public string data { get; set; }
    }

    public class Headers
    {
        public string host { get; set; }
        public string accept { get; set; }
        public string contenttype { get; set; }
        public int contentlength { get; set; }
    }

    public class Response
    {
        public int req_time { get; set; }
        public Error error { get; set; }
    }

    public class Error
    {
        public string code { get; set; }
        public string errno { get; set; }
        public string syscall { get; set; }
        public string hostname { get; set; }
        public string host { get; set; }
        public int port { get; set; }
    }

}
