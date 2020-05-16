using IntegracoesML.Entity;
using IntegracoesML.Service;
using IntegracoesML.Util;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace IntegracoesML.Business
{
    class Repositorio : BaseService
    {
        private Item item;

        //Método responsável por consultar o Feed de Eventos de Pedidos
        public async Task<HttpResponseMessage> ConsultarFeed()
        {
            Log log = new Log();
            try
            {
                string seller_id = "seller=" + ConfigurationManager.AppSettings["user_id"];

                string _paramToken = "&access_token=" + ConfigurationManager.AppSettings["access_token"]; 

                string uriFeed = "/orders/search/recent?" + seller_id + _paramToken;

                HttpResponseMessage responseFeed = await BuildClientMeli().GetAsync(uriFeed);

                return responseFeed;
            }
            catch (HttpRequestException e)
            {
                //log.WriteLogRetNF("Exception ConsultarFilaDeEventos " + e.InnerException.Message);
                throw e;
            }
        }

        public async Task<HttpResponseMessage> ConsultarMyFeed()
        {
            Log log = new Log();
            try
            {
                string seller_id = "app_id=" + ConfigurationManager.AppSettings["client_id"];

                string _paramToken = "&access_token=" + ConfigurationManager.AppSettings["access_token"];

                string uriFeed = "/myfeeds?" + seller_id + _paramToken;

                HttpResponseMessage responseFeed = await BuildClientMeli().GetAsync(uriFeed);

                return responseFeed;
            }
            catch (HttpRequestException e)
            {
                //log.WriteLogRetNF("Exception ConsultarFilaDeEventos " + e.InnerException.Message);
                throw e;
            }
        }

        public async Task<HttpResponseMessage> BuscarPedidoMeli(long idPedido)
        {
            Log log = new Log();
            try
            {
                log.WriteLogPedido("Buscando Pedido " + idPedido);

                string _paramToken = "?access_token=" + ConfigurationManager.AppSettings["access_token"];

                string uriOrder = "/orders/"+ idPedido + _paramToken;

                HttpResponseMessage responseOrder = await BuildClientMeli().GetAsync(uriOrder);

                return responseOrder;
            }
            catch (HttpRequestException e)
            {
                log.WriteLogPedido("Exception BuscarPedidoMeli " + e.InnerException.Message);
                throw;
            }
        }
        public async Task<HttpResponseMessage> BuscarShipmentById(long idShipment)
        {
            Log log = new Log();
            try
            {
                log.WriteLogPedido("Buscando Shipment " + idShipment);

                string _paramToken = "?access_token=" + ConfigurationManager.AppSettings["access_token"];

                string uriOrder = "/shipments/" + idShipment + _paramToken;

                HttpResponseMessage responseOrder = await BuildClientMeli().GetAsync(uriOrder);

                return responseOrder;
            }
            catch (HttpRequestException e)
            {
                log.WriteLogPedido("Exception BuscarShipmentById " + e.InnerException.Message);
                throw;
            }
        }
        public async Task<HttpResponseMessage> InserirNote(long resource)
        {
            Log log = new Log();
            try
            {
                log.WriteLogPedido("Setando Note: " + resource);

                string _paramToken = "?access_token=" + ConfigurationManager.AppSettings["access_token"];

                string uriOrder = "/orders/"+ resource + "/notes" + _paramToken;

                string postContent = "{\"note\":\"Integrado\"}";

                HttpResponseMessage response = await BuildClientMeli().PostAsync(uriOrder, new StringContent(postContent, UnicodeEncoding.UTF8, "application/json"));

                return response;
            }
            catch (HttpRequestException e)
            {
                log.WriteLogPedido("Exception InserirNote " + e.InnerException.Message);
                throw;
            }
        }
        public async Task<HttpResponseMessage> BuscarNote(long resource)
        {
            Log log = new Log();
            try
            {
                log.WriteLogPedido("Buscando Note: " + resource);

                string _paramToken = "?access_token=" + ConfigurationManager.AppSettings["access_token"];

                string uriOrder = "/orders/" +resource+ "/notes" + _paramToken;

                HttpResponseMessage response = await BuildClientMeli().GetAsync(uriOrder);

                return response;
            }
            catch (HttpRequestException e)
            {
                log.WriteLogPedido("Exception BuscarNote " + e.InnerException.Message);
                throw;
            }
        }

        public async Task<HttpResponseMessage> RecuperarShipmentId(string resource)
        {
            Log log = new Log();
            try
            {
                //log.WriteLogRetNF("Buscando Pedido " + resource);

                string _paramToken = "?access_token=" + ConfigurationManager.AppSettings["access_token"];

                string uriOrder = resource + _paramToken;

                HttpResponseMessage responseOrder = await BuildClientMeli().GetAsync(uriOrder);

                return responseOrder;
            }
            catch (HttpRequestException e)
            {
                log.WriteLogRetNF("Exception BuscarShipmentId " + e.InnerException.Message);
                throw;
            }
        }

        public async Task<HttpResponseMessage> RecuperarItemPorSKUSAP(string _sku)
        {
            Log log = new Log();
            try
            {
                string _paramToken = "?sku="+_sku+"&access_token=" + ConfigurationManager.AppSettings["access_token"];

                string uriItemSku = "/users/"+ ConfigurationManager.AppSettings["user_id"] + "/items/search" + _paramToken;

                HttpResponseMessage responseOrder = await BuildClientMeli().GetAsync(uriItemSku);

                return responseOrder;
            }
            catch (HttpRequestException e)
            {
                log.WriteLogRetNF("Exception RecuperarItemPorSKUSAP " + e.InnerException.Message);
                throw;
            }
        }

        public async Task<HttpResponseMessage> EnvioXmlNF(string resource, string postContent)
        {
            Log log = new Log();


            try
            {
                log.WriteLogRetNF("Enviando dados NF: " + resource);

                HttpResponseMessage response;

                string _paramToken = "?access_token=" + ConfigurationManager.AppSettings["access_token"] + "&" + "siteId=" + ConfigurationManager.AppSettings["siteId"];

                string uriOrder = resource + "/invoice_data" + _paramToken;

                using (var httpClient = new HttpClient())
                {
                    using (var request = new HttpRequestMessage(new HttpMethod("POST"), "https://api.mercadolibre.com" + uriOrder))
                    {
                        request.Content = new StringContent(postContent);
                        request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/xml");

                        return await httpClient.SendAsync(request);
                    }
                }

                /*HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://api.mercadolibre.com"+uriOrder);
                request.ContentType = "text/xml";
                request.Method = "POST";

                byte[] bytes;
                bytes = System.Text.Encoding.ASCII.GetBytes(postContent);
                request.ContentLength = bytes.Length;
                Stream requestStream = request.GetRequestStream();
                requestStream.Write(bytes, 0, bytes.Length);
                requestStream.Close();


                HttpRequestMessage req = new HttpRequestMessage();
                req.RequestUri = new Uri("https://api.mercadolibre.com"+uriOrder);
                req.Method = HttpMethod.Post;
                //req.Content.Headers.ContentType = new MediaTypeHeaderValue("application/xml");
                req.Headers.Remove("Content-Type");
                req.Headers.Add("Content-Type", "application/json");

                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/xml; charset=utf-8");
                //HttpResponseMessage response = await BuildClientMeliEnvioNF().PostAsync(uriOrder, new StringContent(postContent, Encoding.UTF8, "application/xml"));

                HttpResponseMessage response = await client.SendAsync(req);
                */
                //HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                //HttpResponseMessage response = await BuildClientMeliEnvioNF().PostAsync(uriOrder, new StringContent(postContent, Encoding.UTF8, "application/xml"));

                /*string text = "Escape characters ： < > & \" \'";
                string xmlText = SecurityElement.Escape(text);

                XmlConvert.VerifyXmlChars(postContent);
         
                return response;*/
            }
            catch (HttpRequestException e)
            {
                log.WriteLogRetNF("Exception EnvioXmlNF " + e.InnerException.Message);
                throw;
            }
        }


        //Método responsável por buscar item por SKU
        public async Task<HttpResponseMessage> BuscarItemPorSKU(string _itemCode, int _onHand, SAPbobsCOM.Company oCompany)
        {
            Log log = new Log();
            try
            {
                //log.WriteLogEstoque("Buscando Item VTEX por ManufacturerCode - Código Item SAP: "+_itemCode);

                string uri = "api/catalog_system/pvt/sku/stockkeepingunitbyalternateId/" + _itemCode;

                //Log.WriteLog("URI: " + uri);

                HttpResponseMessage response = await BuildClient().GetAsync(uri);
                
               /* if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = response.Content.ReadAsStringAsync();

                    this.item =  JsonConvert.DeserializeObject<Item>(jsonResponse.Result);

                    if (this.item != null)
                    {
                        //Log.WriteLog("Item " + item.ManufacturerCode + " localizado.");

                        int _skuId = this.item.Id;

                        if (this.item.IsActive)
                        {
                            string warehouseId = ConfigurationManager.AppSettings["warehouseId"];

                            Task<HttpResponseMessage> responseAtualizacaoEstoque = this.AtualizarQuantidadeEstoque(_skuId, _onHand);

                            return await responseAtualizacaoEstoque;
                        }
                    }
                }
                else {
                    //Log.WriteLogTable(oCompany, EnumTipoIntegracao.Estoque, _itemCode, "", EnumStatusIntegracao.Erro, "Item não localizado na VTEX");
                    //this.log.WriteLogEstoque("Item "+_itemCode+" não localizado na Vtex. ");
                    return response;
                }*/
                
            }
            catch (HttpRequestException e)
            {
                log.WriteLogEstoque("Exception BuscarItemVtexPorSKU "+e.InnerException.Message);
                //throw;
            }
            return null;
        }

        //Método responsável por Atualizar quantidade em estoque por produto
        public async Task<HttpResponseMessage> AtualizarQuantidadeEstoque(string _skuId, int _onHand) {
            Log log = new Log();
            try
            {
                string _paramToken = "?access_token=" + ConfigurationManager.AppSettings["access_token"];

                string uriAttEstoque = "items/" + _skuId + _paramToken;

                var jsonUpdateInventory = "{\"available_quantity\":"+_onHand+"}";

                HttpResponseMessage response = await BuildClientMeli().PutAsync(uriAttEstoque, new StringContent(jsonUpdateInventory, UnicodeEncoding.UTF8, "application/json"));

                return response;
            }
            catch (HttpRequestException e)
            {
                log.WriteLogEstoque("Exception AtualizarQuantidadeEstoque " + e.InnerException.Message);
                throw;
            }
        }

        //Método responsável por consultar o Feed de Eventos de Pedidos
        public async Task<HttpResponseMessage> ConsultarFilaDeEventos() {
            Log log = new Log();
            try
            {
                log.WriteLogPedido("Consultando Fila de Eventos de Pedidos");

                string maxLot = ConfigurationManager.AppSettings["maxLot"];

                string _paramFeed = "?maxlot=" + maxLot;

                string uriFeed = "api/orders/feed"+ _paramFeed;

                HttpResponseMessage responseFeed = await BuildClientMeli().GetAsync(uriFeed);

                log.WriteLogPedido("Consulta realizada.");

                return responseFeed;
            }
            catch (HttpRequestException e)
            {
                log.WriteLogPedido("Exception ConsultarFilaDeEventos "+e.InnerException.Message);
                throw;
            }
        }

        //Método responsável por recuperar Pedido Vtex
        public async Task<HttpResponseMessage> BuscarPedido(string orderId){
            Log log = new Log();
            try
            {
                log.WriteLogPedido("Buscando Pedido "+orderId);

                string uriOrder = "api/oms/pvt/orders/"+orderId;

                HttpResponseMessage responseOrder = await BuildClientMeli().GetAsync(uriOrder);

                return responseOrder;
            }
            catch (HttpRequestException e)
            {
                log.WriteLogPedido("Exception BuscarPedido " +e.InnerException.Message);
                throw;
            }
        }

        public async Task<HttpResponseMessage> CancelarPedido(string orderId)
        {
            Log log = new Log();
            try
            {
                //log.WriteLogPedido("Buscando Pedido " + orderId);

                string uriOrder = "api/oms/pvt/orders/" + orderId + "/cancel";

                HttpResponseMessage responseOrder = await BuildClientMeli().GetAsync(uriOrder);

                return responseOrder;
            }
            catch (HttpRequestException e)
            {
                log.WriteLogPedido("Exception dido " + e.InnerException.Message);
                throw;
            }
        }

        public async Task<HttpResponseMessage> RetornoNotaFiscal(Invoice invoice, string idOrderVtex) {
            Log log = new Log();
            try
            {
                log.WriteLogPedido("Fazendo Post número NF");

                string uri = "api/oms/pvt/orders/"+ idOrderVtex + "/invoice";

                var jsonInvoice = JsonConvert.SerializeObject(invoice);

                HttpResponseMessage response = await BuildClient().PostAsync(uri, new StringContent(jsonInvoice, UnicodeEncoding.UTF8, "application/json"));

                return response;
            }
            catch (HttpRequestException e)
            {
                log.WriteLogPedido("Exception RetornoNotaFiscal "+e.InnerException.Message);
                throw;
            }
        }

     
    }
}
