using IntegracoesML.DAL;
using IntegracoesML.Entity;
using IntegracoesML.Util;
using Newtonsoft.Json;
using RestSharp;
using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;
using System.Xml;
using System.Xml.Linq;

namespace IntegracoesML.Business
{
    public class IntegracaoService
    {
        private Log log;

        public IntegracaoService() {
            this.log = new Log();
        }

        public void IniciarIntegracaoEstoque(SAPbobsCOM.Company oCompany)
        {
            try
            {
                Repositorio repositorio = new Repositorio();

                this.log.WriteLogEstoque("Inicio do Processo de Integração de Estoque");

                WarehouseDAL whsDAL = new WarehouseDAL();

                SAPbobsCOM.Recordset recordset = (SAPbobsCOM.Recordset)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);

                recordset = whsDAL.RecuperarSaldoEstoqueSAP(oCompany);

                if (recordset != null && recordset.RecordCount > 0)
                {
                    for (int i = 0; i < recordset.RecordCount; i++)
                    {

                        try
                        {
                            string _itemCode = recordset.Fields.Item("ItemCode").Value.ToString();
                            Int16 _onHand = System.Convert.ToInt16(recordset.Fields.Item("OnHand").Value.ToString());

                            Task<HttpResponseMessage> itemResp = repositorio.RecuperarItemPorSKUSAP(_itemCode);

                            if (itemResp.Result.IsSuccessStatusCode)
                            {
                                //desseralializar item

                                var jsonItemML = itemResp.Result.Content.ReadAsStringAsync().Result;

                                var itemML = JsonConvert.DeserializeObject<ItemML>(jsonItemML);

                                foreach (String itemIdML in itemML.results) {

                                    Task<HttpResponseMessage> responseAttEstoque = repositorio.AtualizarQuantidadeEstoque(itemIdML, _onHand);

                                    if (responseAttEstoque.Result.IsSuccessStatusCode)
                                    {
                                        this.log.WriteLogTable(oCompany, EnumTipoIntegracao.Estoque, _itemCode, _itemCode, EnumStatusIntegracao.Sucesso, "Estoque atualizado com sucesso.");
                                        this.log.WriteLogEstoque("Quantidade de estoque do Produto " + _itemCode + " atualizada com sucesso.");
                                    }
                                }
                            }


                        }
                        catch (Exception e)
                        {
                            this.log.WriteLogEstoque("Exception IniciarProcessoEstoque " + e.Message);
                            throw;
                        }


                        recordset.MoveNext();

                    }

                }

                if (recordset != null)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(recordset);
                }

                //Log.WriteLog("Atualização controle executação.");
                // Environment.SetEnvironmentVariable("controleExecucao", DateTime.Now.ToUniversalTime().ToString("s") + "Z");

            }
            catch (Exception e)
            {
                this.log.WriteLogTable(oCompany, EnumTipoIntegracao.Estoque, "", "", EnumStatusIntegracao.Erro, e.Message);
                this.log.WriteLogEstoque("Exception IniciarProcessoEstoque " + e.Message);
                throw;
            }
        }

        private void InserirClientes(SAPbobsCOM.Company company, Order pedidoMl) {
            try
            {
                BusinessPartnersDAL bpDAL = new BusinessPartnersDAL();

                string errorMessage;

                bpDAL.InserirBusinessPartner(company, pedidoMl, out errorMessage);
            }
            catch (Exception e)
            {
                this.log.WriteLogPedido("Exception inserirClientes " + e.Message);
                throw;
            }
        }

        public void IniciarIntegracaoPedido(SAPbobsCOM.Company oCompany) {
            try
            {
                this.log.WriteLogPedido("Inicio do Processo de Integração de Pedido.");

                Repositorio repositorioPedido = new Repositorio();

                OrderRecent ordersRecent = new OrderRecent();

                List<Order> results = new List<Order>();

                Task<HttpResponseMessage> responseFeed = repositorioPedido.ConsultarFeed();

                if (responseFeed.Result.IsSuccessStatusCode)
                {
                    var jsonOrdersRecentString = responseFeed.Result.Content.ReadAsStringAsync().Result;

                    var jsonSerializeconfig = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    };
                    //JavaScriptSerializer jsSerializer = new JavaScriptSerializer();

                    ordersRecent = JsonConvert.DeserializeObject<OrderRecent>(jsonOrdersRecentString, jsonSerializeconfig);

                    //ordersRecent = jsSerializer.Deserialize<OrderRecent>(jsonOrdersRecentString);

                    results = ordersRecent.results;

                    if (results.Count > 0)
                    {
                        foreach (Order pedidoMl in results) {

                            Task<HttpResponseMessage> responseOrder = repositorioPedido.BuscarPedidoMeli(pedidoMl.id);

                            if (responseOrder.Result.IsSuccessStatusCode)
                            {
                                var jsonOrderString = responseOrder.Result.Content.ReadAsStringAsync().Result;

                                Order pedido = JsonConvert.DeserializeObject<Order>(jsonOrderString, jsonSerializeconfig);

                                bool integrado = false;

                                //Validar se tem a nota de "Integrado"
                                Task<HttpResponseMessage> responseGetNote = repositorioPedido.BuscarNote(pedido.id);

                                if (responseGetNote.Result.IsSuccessStatusCode)
                                {
                                    var jsonNote = responseGetNote.Result.Content.ReadAsStringAsync().Result;

                                    var notes = JsonConvert.DeserializeObject<List<Notes>>(jsonNote, jsonSerializeconfig);

                                    foreach (Notes note in notes)
                                    {
                                        foreach (Result result in note.results)
                                        {
                                            if (result.note.Equals("Integrado"))
                                            {
                                                integrado = true;
                                            }
                                        }
                                    }

                                }
                                //recuperar pedido
                                if (!integrado)
                                {
                                    //validar se pedido está confirmado
                                    if (pedido.status.Equals("paid"))
                                    {
                                        //integrar cliente
                                        this.InserirClientes(oCompany, pedido);

                                        //Integrar Pedido
                                        this.InserirPedidoVenda(oCompany, pedido);

                                        //atualizar campo notas 
                                        Task<HttpResponseMessage> responseNote = repositorioPedido.InserirNote(pedido.id);

                                        if (!responseNote.Result.IsSuccessStatusCode)
                                        {
                                            //logar não foi possível atualizar nota do pedido
                                            this.log.WriteLogPedido("Não foi possível atualizar Note pedido " + pedido.id + " Erro:" + responseNote.Result.ReasonPhrase);
                                        }

                                    }
                                }
                            }

                            
                        }
                    }
                }
                else if (responseFeed.Result.StatusCode.Equals("401")) {
                    GetNewAccessToken();
                }
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
            }
            catch (Exception e)
            {
                this.log.WriteLogPedido("Exception IniciarIntegracaoPedido " + e.Message);
                throw;
            }
        }

        private int InserirPedidoVenda(SAPbobsCOM.Company oCompany, Order pedidoMl) {
            try
            {
                if (oCompany.Connected)
                {
                    OrdersDAL orderDAL = new OrdersDAL(oCompany);
                    string messageError = "";
                    int oOrderNum = 0;
                    Boolean inserir = true;

                    foreach (Order_Items item in pedidoMl.order_items)
                    {
                        if (item.item.seller_custom_field == null && inserir)
                        {
                            this.log.WriteLogTable(oCompany, EnumTipoIntegracao.PedidoVenda, pedidoMl.id.ToString(), "", EnumStatusIntegracao.Erro, "Um ou mais item(s) do pedido está com o código de referência inválido.");
                            //throw new ArgumentException("Não foi possível criar o Pedido de Venda para o pedido "+pedidoVtex.orderId+" pois um ou mais item(s) do pedido está com o código de referência inválido.");
                            inserir = false;
                        }
                    }

                    if (inserir)
                    {
                        oOrderNum = orderDAL.InsertOrder(pedidoMl, out messageError);

                    }
                }

                return 0;
            }
            catch (Exception e)
            {
                this.log.WriteLogPedido("Exception InserirPedidoVenda " + e.Message);
                throw;
            }
        }

        public void RetornoNotaFiscal(SAPbobsCOM.Company oCompany)
        {
            try
            {
                this.log.WriteLogRetNF("Inicio do Processo de Integração de Retorno de NF");

                Repositorio repositorio = new Repositorio();

                string resultado = "Não há nenhuma NF a ser retornada.";
                int contador = 0;

                Task<HttpResponseMessage> responseFeed = repositorio.ConsultarMyFeed();

                if (responseFeed.Result.IsSuccessStatusCode)
                {
                    var jsonMyFeed = responseFeed.Result.Content.ReadAsStringAsync().Result;

                    var myFeeds = JsonConvert.DeserializeObject<MyFeedResponse>(jsonMyFeed);

                    if (myFeeds.messages.Length > 0)
                    {
                        foreach (Entity.Message feed in myFeeds.messages)
                        {
                            if (feed.topic.Equals("shipments"))
                            {
                                Task<HttpResponseMessage> responseShipment = repositorio.RecuperarShipmentId(feed.resource);

                                if (responseShipment.Result.IsSuccessStatusCode)
                                {
                                    var jsonShipments = responseShipment.Result.Content.ReadAsStringAsync().Result;

                                    var shipment = JsonConvert.DeserializeObject<Shipments>(jsonShipments);

                                    if (shipment.status.Equals("ready_to_ship"))
                                    {
                                        resultado = "Foi encontrado "+contador+1+"NF a ser enviado para o ML. (status_ready_to_ship)";

                                        //retornar os dados da NF -> xml
                                        this.log.WriteLogRetNF("Entrou no ready_to_ship e invoice_pending");

                                        List<string> fileList = new List<string>();
                                        FileService _fileService = new FileService();

                                        string _filePath = ConfigurationManager.AppSettings["FilePath"];

                                        //Verificar se existe arquivo
                                        fileList = _fileService.FileExists(_filePath);

                                        if (fileList.Count > 0)
                                        {
                                            if (oCompany.Connected)
                                            {
                                                OrdersDAL orders = new OrdersDAL(oCompany);

                                                SAPbobsCOM.Recordset recordSet = (Recordset)oCompany.GetBusinessObject(BoObjectTypes.BoRecordset);

                                                recordSet = orders.RecuperarNumeroNF();

                                                if (recordSet != null && recordSet.RecordCount > 0)
                                                {
                                                    for (int i = 0; i < recordSet.RecordCount; i++)
                                                    {
                                                        int updatePedidoNum = 0;

                                                        string nfKey = recordSet.Fields.Item("nfeKey").Value.ToString();
                                                        string docSAP = recordSet.Fields.Item("docSAP").Value.ToString();
                                                        string externalId = recordSet.Fields.Item("externalId").Value.ToString();

                                                        string idOrderML = recordSet.Fields.Item("idOrderML").Value.ToString();

                                                        string invoiceNumber = recordSet.Fields.Item("invoiceNumber").Value.ToString();

                                                        DateTime invoiceDate = DateTime.Parse(recordSet.Fields.Item("invoiceDate").Value.ToString());

                                                        if (invoiceDate.Date.Year >= DateTime.Now.Date.Year)
                                                        {
                                                            foreach (string _file in fileList)
                                                            {
                                                                if (Path.GetFileNameWithoutExtension(_file).Length == 51)
                                                                {
                                                                    string _fileName = Path.GetFileNameWithoutExtension(_file).Substring(7);

                                                                    if (nfKey.Equals(_fileName))
                                                                    {
                                                                        XmlDocument _xml = new XmlDocument { XmlResolver = null };
                                                                        _xml.Load(_file);
                                                                        string xmlToPOST = _xml.InnerXml.Replace("\\", "").ToString();


                                                                        //Fazer POST xml
                                                                        Task<HttpResponseMessage> responseEnvioXml = repositorio.EnvioXmlNF(feed.resource, xmlToPOST);

                                                                        if (responseEnvioXml.Result.IsSuccessStatusCode)
                                                                        {
                                                                            //Atualizando campo de usuário U_EnvioNFVTEX
                                                                            updatePedidoNum = orders.AtualizarPedidoVenda(oCompany, Convert.ToInt32(externalId));

                                                                            if (updatePedidoNum == 0)
                                                                            {
                                                                                this.log.WriteLogTable(oCompany, EnumTipoIntegracao.NF, idOrderML, docSAP, EnumStatusIntegracao.Sucesso, "Dados NF " + invoiceNumber + " enviado para a ML com sucesso.");
                                                                                this.log.WriteLogRetNF("Dados da NF do Pedido de Venda " + docSAP + " enviado ao Mercado Livre com sucesso.");
                                                                            }
                                                                            else
                                                                            {
                                                                                this.log.WriteLogTable(oCompany, EnumTipoIntegracao.NF, idOrderML, docSAP, EnumStatusIntegracao.Erro, "Dados NF " + invoiceNumber + " retornado porém não foi possivél atualizar campo de usuário (U_EnvioNFML) do Pedido de Venda");
                                                                                this.log.WriteLogRetNF("Falha ao atualizar Pedido de Venda " + docSAP);
                                                                            }
                                                                        }
                                                                        else if (responseEnvioXml.Result.StatusCode.Equals("401"))
                                                                        {
                                                                            GetNewAccessToken();
                                                                        }
                                                                        else if (!responseEnvioXml.Result.IsSuccessStatusCode)
                                                                        {
                                                                            var jsonResponseRetNF = responseEnvioXml.Result.Content.ReadAsStringAsync().Result;

                                                                            var _retNfresponse = JsonConvert.DeserializeObject<RetNFResponse>(jsonResponseRetNF);

                                                                            string mensagemErro = string.Empty;

                                                                            mensagemErro += "Status: " + _retNfresponse.status;

                                                                            if (!string.IsNullOrEmpty(_retNfresponse.error))
                                                                            {
                                                                                mensagemErro += " - Erro:" + _retNfresponse.error;
                                                                            }
                                                                            if (!string.IsNullOrEmpty(_retNfresponse.message))
                                                                            {
                                                                                mensagemErro += " - Detalhe do erro: " + _retNfresponse.message;
                                                                            }

                                                                            this.log.WriteLogTable(oCompany, EnumTipoIntegracao.NF, idOrderML, docSAP, EnumStatusIntegracao.Erro, mensagemErro);
                                                                            this.log.WriteLogRetNF("Não foi possível retornar o XML:" + responseEnvioXml.Result.Content.ReadAsStringAsync().Result);

                                                                            //Atualizando campo de usuário U_EnvioNFML
                                                                            updatePedidoNum = orders.AtualizarPedidoVenda(oCompany, Convert.ToInt32(externalId));

                                                                            if (updatePedidoNum != 0)
                                                                            {
                                                                               
                                                                                this.log.WriteLogRetNF("Não foi possível atualizar o Pedido Venda " + docSAP);
                                                                            }
                                                                        }
                                                                    }
                                                                }

                                                            }

                                                        }


                                                        recordSet.MoveNext();
                                                    }
                                                }
                                            }
                                            #region 
                                            /*
                                            if (oCompany.Connected)
                                            {
                                                OrdersDAL orders = new OrdersDAL(oCompany);

                                                SAPbobsCOM.Recordset recordSet = (Recordset)oCompany.GetBusinessObject(BoObjectTypes.BoRecordset);

                                                recordSet = orders.RecuperarNumeroNF();

                                                if (recordSet != null && recordSet.RecordCount > 0)
                                                {
                                                    for (int i = 0; i < recordSet.RecordCount; i++)
                                                    {
                                                        Repositorio repositorio = new Repositorio();
                                                        Invoice invoice = new Invoice();

                                                        //invoice.type = "Output";
                                                        //invoice.issuanceDate = recordSet.Fields.Item("invoiceDate").Value.ToString("yyyy-MM-dd");
                                                        invoice.invoiceNumber = recordSet.Fields.Item("invoiceNumber").Value.ToString();
                                                        invoice.invoiceKey = recordSet.Fields.Item("nfeKey").Value.ToString();
                                                        string externalId = recordSet.Fields.Item("externalId").Value.ToString();
                                                        string idOrderVtex = recordSet.Fields.Item("idOrderVtex").Value.ToString();
                                                        string idOrderVtex2 = recordSet.Fields.Item("idOrderVtex2").Value.ToString();
                                                        string docSAP = recordSet.Fields.Item("docSAP").Value.ToString();
                                                        string docNPV = recordSet.Fields.Item("docNPV").Value.ToString();

                                                        invoice.invoiceValue = recordSet.Fields.Item("totalNF").Value.ToString().Replace(",","");
                                                        invoice.courier = recordSet.Fields.Item("shippingMethod").Value.ToString();

                                                        int updatePedidoNum = 0;
                                                        string idPedidoVTEX = string.Empty;

                                                        string tempDocNPV = string.Empty;

                                                        List<ItemNF> listaItem = new List<ItemNF>();

                                                        for (int j = 0; j < recordSet.RecordCount ; j++)
                                                        {
                                                            tempDocNPV = recordSet.Fields.Item("docNPV").Value.ToString();

                                                            if (docNPV.Equals(tempDocNPV))
                                                            {
                                                                ItemNF item = new ItemNF();

                                                                item.id = recordSet.Fields.Item("codItem").Value.ToString();
                                                                item.price = System.Convert.ToInt32(recordSet.Fields.Item("precoItem").Value.ToString().Replace(",", ""));
                                                                item.quantity = System.Convert.ToInt32(recordSet.Fields.Item("qtdItem").Value.ToString());

                                                                listaItem.Add(item);
                                                            }

                                                            recordSet.MoveNext();
                                                        }

                                                        invoice.items = listaItem;

                                                        if (!string.IsNullOrEmpty(idOrderVtex))
                                                        {
                                                            idPedidoVTEX = idOrderVtex;
                                                        }
                                                        else if (!string.IsNullOrEmpty(idOrderVtex2))
                                                        {
                                                            idPedidoVTEX = idOrderVtex2;
                                                        }

                                                        if (!string.IsNullOrEmpty(idOrderVtex) && !string.IsNullOrEmpty(idOrderVtex2))
                                                        {

                                                            Task<HttpResponseMessage> response = repositorio.RetornoNotaFiscal(invoice, idPedidoVTEX);

                                                            if (response.Result.IsSuccessStatusCode)
                                                            {
                                                                //Atualizando campo de usuário U_EnvioNFVTEX
                                                                updatePedidoNum = orders.AtualizarPedidoVenda(oCompany, Convert.ToInt32(externalId));

                                                                if (updatePedidoNum == 0)
                                                                {
                                                                    this.log.WriteLogTable(oCompany, EnumTipoIntegracao.NF, idPedidoVTEX, docSAP, EnumStatusIntegracao.Sucesso, "Número NF " + invoice.invoiceNumber + " enviado para a Vtex com sucesso.");
                                                                    this.log.WriteLogPedido("Número NF para o Pedido de Venda " + docSAP + " enviado para a Vtex com sucesso.");
                                                                }
                                                                else
                                                                {
                                                                    this.log.WriteLogTable(oCompany, EnumTipoIntegracao.NF, idPedidoVTEX, docSAP, EnumStatusIntegracao.Erro, "Número NF " + invoice.invoiceNumber + " retornado porém não foi possivél atualizar campo de usuário (U_EnvioNFVTEX) do Pedido de Venda");
                                                                    this.log.WriteLogPedido("Falha ao atualizar Pedido de Venda " + docSAP);
                                                                }

                                                            }
                                                            else
                                                            {
                                                                this.log.WriteLogTable(oCompany, EnumTipoIntegracao.NF, idPedidoVTEX, externalId, EnumStatusIntegracao.Erro, response.Result.ReasonPhrase);
                                                                this.log.WriteLogPedido("Falha ao retornar número da Nota Fiscal " + externalId + " para a Vtex");
                                                            }
                                                        }
                                                        else
                                                        {
                                                            this.log.WriteLogTable(oCompany, EnumTipoIntegracao.NF, idPedidoVTEX, externalId, EnumStatusIntegracao.Erro, "Id do Pedido VTEX (NumAtCard e U_NumPedEXT) do Pedido de Venda " + docNPV + " em branco.");
                                                            this.log.WriteLogPedido("Falha ao retornar número da Nota Fiscal " + externalId + " para a Vtex - Id do Pedido VTEX (NumAtCard) do Pedido de Venda " + docNPV + " em branco.");

                                                            //Atualizando campo de usuário U_EnvioNFVTEX
                                                            updatePedidoNum = orders.AtualizarPedidoVenda(oCompany, Convert.ToInt32(externalId));

                                                            if (updatePedidoNum != 0)
                                                            {
                                                                this.log.WriteLogTable(oCompany, EnumTipoIntegracao.NF, idPedidoVTEX, docSAP, EnumStatusIntegracao.Erro, "Número NF " + invoice.invoiceNumber + " retornado porém não foi possivél atualizar campo de usuário (U_EnvioNFVTEX) do Pedido de Venda");
                                                                this.log.WriteLogPedido("Falha ao atualizar Pedido de Venda " + docSAP);
                                                            }
                                                        }
                                                        recordSet = orders.RecuperarNumeroNF();
                                                        //recordSet.MoveNext();
                                                    }
                                                }

                                                if (recordSet != null)
                                                {
                                                    System.Runtime.InteropServices.Marshal.ReleaseComObject(recordSet);
                                                }
                                            }*/
                                            #endregion //old
                                            
                                        }
                                    }
                                }
                                else if (responseShipment.Result.StatusCode.Equals("401"))
                                {
                                    GetNewAccessToken();
                                }
                            }
                        }

                        this.log.WriteLogRetNF(resultado);
                    }
                }
                else if (responseFeed.Result.StatusCode.Equals("401"))
                {
                    GetNewAccessToken();
                }

            }
            catch (Exception e)
            {
                this.log.WriteLogRetNF("Exception RetornoNotaFiscal Inner: "+e.InnerException+" - exception: "+e.Message+" - stack trace: "+e.StackTrace);
                //throw;
            }
        }

        public void GetNewAccessToken()
        {
            var client = new RestClient("https://api.mercadolibre.com/oauth/token");
            var request = new RestRequest(Method.POST);
            request.AddHeader("Cache-Control", "no-cache");
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            request.AddParameter("undefined", "client_id=2931710302492223&client_secret=FAC9pEl4U1lcQfx6T22R4l53rnpZP4GZ&grant_type=client_credentials", ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);

            var responseTokenObj = JsonConvert.DeserializeObject<AccessToken>(response.Content);

            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings["access_token"].Value = responseTokenObj.access_token;
            config.AppSettings.Settings["token_type"].Value = responseTokenObj.token_type;
            config.AppSettings.Settings["expires_in"].Value = responseTokenObj.expires_in.ToString();
            config.AppSettings.Settings["scope"].Value = responseTokenObj.scope;
            config.AppSettings.Settings["user_id"].Value = responseTokenObj.user_id.ToString();
            config.AppSettings.Settings["refresh_token"].Value = responseTokenObj.refresh_token;

            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");

        }
    }
}
