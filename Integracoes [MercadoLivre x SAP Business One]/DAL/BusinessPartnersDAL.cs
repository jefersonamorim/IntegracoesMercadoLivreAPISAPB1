using IntegracoesML.Business;
using IntegracoesML.Entity;
using IntegracoesML.Util;
using Newtonsoft.Json;
using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace IntegracoesML.DAL
{
    public class BusinessPartnersDAL
    {

        private SAPbobsCOM.Company oCompany;

        private Log log;

        internal BusinessPartnersDAL()
         {
            this.log = new Log();
            //this.oCompany = company;
        }

        public void InserirBusinessPartner(SAPbobsCOM.Company company, Order pedido, out string messageError)
        {
            int addBPNumber = 0;

            string document = string.Empty;
            Boolean isCorporate = false;

            if (pedido.buyer.billing_info.doc_type != null && pedido.buyer.billing_info.doc_type.Equals("CNPJ"))
            {
                if (pedido.buyer.billing_info.doc_number != null)
                {
                    document = pedido.buyer.billing_info.doc_number;
                    isCorporate = true;
                }
            }
            else if (pedido.buyer.billing_info.doc_type != null && pedido.buyer.billing_info.doc_type.Equals("CPF"))
            {
                if (pedido.buyer.billing_info.doc_number != null)
                {
                    document = pedido.buyer.billing_info.doc_number;
                }
            }

            try
            {
                CountyDAL countyDAL = new CountyDAL();

                this.oCompany = company;

                int _groupCode = Convert.ToInt32(ConfigurationManager.AppSettings["GroupCode"]);
                int _splCode = Convert.ToInt32(ConfigurationManager.AppSettings["SlpCode"]);
                int _QoP = Convert.ToInt32(ConfigurationManager.AppSettings["QoP"]);
                int groupNum = Convert.ToInt32(ConfigurationManager.AppSettings["GroupNum"]);
                string indicadorIE = ConfigurationManager.AppSettings["IndicadorIE"];
                string indicadorOpConsumidor = ConfigurationManager.AppSettings["IndicadorOpConsumidor"];
                string gerente = ConfigurationManager.AppSettings["Gerente"];
                int priceList = Convert.ToInt32(ConfigurationManager.AppSettings["PriceList"]);
                string cardCodePrefix = ConfigurationManager.AppSettings["CardCodePrefix"];
                int categoriaCliente = Convert.ToInt32(ConfigurationManager.AppSettings["CategoriaCliente"]);
                
                this.log.WriteLogPedido("Inserindo Cliente " + cardCodePrefix + document);

                BusinessPartners oBusinessPartner = null;
                oBusinessPartner = (SAPbobsCOM.BusinessPartners)oCompany.GetBusinessObject(BoObjectTypes.oBusinessPartners);

                BusinessPartners oBusinessPartnerUpdateTest = null;
                oBusinessPartnerUpdateTest = (SAPbobsCOM.BusinessPartners)oCompany.GetBusinessObject(BoObjectTypes.oBusinessPartners);

                if (oBusinessPartnerUpdateTest.GetByKey(cardCodePrefix + document))
                {
                    oBusinessPartner = oBusinessPartnerUpdateTest;
                }

                //Setando campos padrões
                oBusinessPartner.CardCode = cardCodePrefix + document;

                oBusinessPartner.CardName = pedido.buyer.first_name + " "+pedido.buyer.last_name;
                oBusinessPartner.EmailAddress = pedido.buyer.email;
                
                oBusinessPartner.CardType = BoCardTypes.cCustomer;
                oBusinessPartner.GroupCode = _groupCode;
                oBusinessPartner.SalesPersonCode = _splCode;
                oBusinessPartner.PayTermsGrpCode = groupNum;
                oBusinessPartner.PriceListNum = priceList;
                //oBusinessPartner.CardForeignName = "Teste";

                //Setando campos de usuário
                oBusinessPartner.UserFields.Fields.Item("U_TX_IndIEDest").Value = indicadorIE;
                oBusinessPartner.UserFields.Fields.Item("U_TX_IndFinal").Value = indicadorOpConsumidor;
                oBusinessPartner.UserFields.Fields.Item("U_Gerente").Value = gerente;
                oBusinessPartner.UserFields.Fields.Item("U_CategoriaCliente").Value = gerente;


                //removendo o +55
                if (pedido.buyer.phone != null)
                {
                    if (pedido.buyer.phone.number != null)
                    {
                        oBusinessPartner.Phone1 = pedido.buyer.phone.number;
                    }
                }
                string codMunicipio = string.Empty;

                Repositorio repositorio = new Repositorio();

                Shipments shipment = null;

                Task<HttpResponseMessage> responseShipment = repositorio.BuscarShipmentById(pedido.shipping.id);

                if (responseShipment.Result.IsSuccessStatusCode)
                {
                    var jsonShipment = responseShipment.Result.Content.ReadAsStringAsync().Result;

                    shipment = JsonConvert.DeserializeObject<Shipments>(jsonShipment);

                    if (shipment.receiver_address != null)
                    {
                        codMunicipio = countyDAL.RecuperarCodigoMunicipio(shipment.receiver_address.city.name, this.oCompany);
                    }
                }

                //Inserindo endereços
                //COBRANÇA
                oBusinessPartner.Addresses.SetCurrentLine(0);
                oBusinessPartner.Addresses.AddressType = BoAddressType.bo_BillTo;
                oBusinessPartner.Addresses.AddressName = "COBRANCA";

                if (shipment != null)
                {
                    oBusinessPartner.Addresses.City = shipment.receiver_address.city.name;

                    if (shipment.receiver_address.comment != null && shipment.receiver_address.comment.Length <= 100)
                    {
                        oBusinessPartner.Addresses.BuildingFloorRoom = shipment.receiver_address.comment;
                    }

                    //oBusinessPartner.Addresses.Country = "1058";
                    if (shipment.receiver_address.neighborhood.name != null)
                    {
                        oBusinessPartner.Addresses.Block = shipment.receiver_address.neighborhood.name;
                    }

                    oBusinessPartner.Addresses.StreetNo = shipment.receiver_address.street_number;
                    oBusinessPartner.Addresses.ZipCode = shipment.receiver_address.zip_code;
                    oBusinessPartner.Addresses.State = shipment.receiver_address.state.id.Substring(3);
                    oBusinessPartner.Addresses.Street = shipment.receiver_address.street_name;
                    oBusinessPartner.Addresses.County = codMunicipio;
                    //oBusinessPartner.Addresses.Country = "br";
                }

                //FATURAMENTO
                oBusinessPartner.Addresses.SetCurrentLine(1);
                oBusinessPartner.Addresses.AddressType = BoAddressType.bo_ShipTo;
                oBusinessPartner.Addresses.AddressName = "FATURAMENTO";

                if (shipment != null)
                {
                    oBusinessPartner.Addresses.City = shipment.receiver_address.city.name;

                    if (shipment.receiver_address.comment != null && shipment.receiver_address.comment.Length <= 100)
                    {
                        oBusinessPartner.Addresses.BuildingFloorRoom = shipment.receiver_address.comment;
                    }

                    //oBusinessPartner.Addresses.Country = "1058";
                    if (shipment.receiver_address.neighborhood.name != null)
                    {
                        oBusinessPartner.Addresses.Block = shipment.receiver_address.neighborhood.name;
                    }

                    oBusinessPartner.Addresses.StreetNo = shipment.receiver_address.street_number;
                    oBusinessPartner.Addresses.ZipCode = shipment.receiver_address.zip_code;
                    oBusinessPartner.Addresses.State = shipment.receiver_address.state.id.Substring(3);
                    oBusinessPartner.Addresses.Street = shipment.receiver_address.street_name;
                    oBusinessPartner.Addresses.County = codMunicipio;
                    //oBusinessPartner.Addresses.Country = "br";
                }

                #region ENDEREÇO FOR
                /*
                for (int i = 0; i < 2; i++)
                {
                    if (i > 0)
                    {
                        oBusinessPartner.Addresses.SetCurrentLine(i);
                        oBusinessPartner.Addresses.AddressType = BoAddressType.bo_ShipTo;
                        oBusinessPartner.Addresses.AddressName = "FATURAMENTO";
                    }
                    else
                    {
                        oBusinessPartner.Addresses.SetCurrentLine(i);
                        oBusinessPartner.Addresses.AddressType = BoAddressType.bo_BillTo;
                        oBusinessPartner.Addresses.AddressName = "COBRANCA";

                        if (!oBusinessPartnerUpdateTest.GetByKey(cardCodePrefix + document))
                        {
                            oBusinessPartner.Addresses.Add();
                        }
                    }

                    if (shipment != null)
                    {
                        oBusinessPartner.Addresses.City = shipment.receiver_address.city.name;

                        if (shipment.receiver_address.comment != null && shipment.receiver_address.comment.Length <= 100)
                        {
                            oBusinessPartner.Addresses.BuildingFloorRoom = shipment.receiver_address.comment;
                        }

                        //oBusinessPartner.Addresses.Country = "1058";
                        if (shipment.receiver_address.neighborhood.name != null)
                        {
                            oBusinessPartner.Addresses.Block = shipment.receiver_address.neighborhood.name;
                        }

                        oBusinessPartner.Addresses.StreetNo = shipment.receiver_address.street_number;
                        oBusinessPartner.Addresses.ZipCode = shipment.receiver_address.zip_code;
                        oBusinessPartner.Addresses.State = shipment.receiver_address.state.id.Substring(3);
                        oBusinessPartner.Addresses.Street = shipment.receiver_address.street_name;
                        oBusinessPartner.Addresses.County = codMunicipio;
                        //oBusinessPartner.Addresses.Country = "br";
                    }

                }*/
                #endregion

                #region código de endereço antigo
                /*oBusinessPartner.Addresses.SetCurrentLine(0);
                oBusinessPartner.Addresses.AddressName = "COBRANCA";
                oBusinessPartner.Addresses.AddressType = SAPbobsCOM.BoAddressType.bo_BillTo;

                oBusinessPartner.Addresses.Street = endereco.street;
                oBusinessPartner.Addresses.Block = endereco.neighborhood;
                oBusinessPartner.Addresses.ZipCode = endereco.postalCode;
                oBusinessPartner.Addresses.City = endereco.city;
                oBusinessPartner.Addresses.Country = "BR";
                oBusinessPartner.Addresses.State = endereco.state;
                oBusinessPartner.Addresses.BuildingFloorRoom = endereco.complement;
                oBusinessPartner.Addresses.StreetNo = endereco.number;
                
                oBusinessPartner.Addresses.Add();

                oBusinessPartner.Addresses.SetCurrentLine(1);
                oBusinessPartner.Addresses.AddressName = "FATURAMENTO";
                oBusinessPartner.Addresses.AddressType = SAPbobsCOM.BoAddressType.bo_ShipTo;

                oBusinessPartner.Addresses.Street = endereco.street;
                oBusinessPartner.Addresses.Block = endereco.neighborhood;
                oBusinessPartner.Addresses.ZipCode = endereco.postalCode;
                oBusinessPartner.Addresses.City = endereco.city;
                oBusinessPartner.Addresses.Country = "BR";
                oBusinessPartner.Addresses.State = endereco.state;
                oBusinessPartner.Addresses.BuildingFloorRoom = endereco.complement;
                oBusinessPartner.Addresses.StreetNo = endereco.number;*/
                //oBusinessPartner.Addresses.Add();
                #endregion

                oBusinessPartner.BilltoDefault = "COBRANCA";
                oBusinessPartner.ShipToDefault = "FATURAMENTO";

                BusinessPartners oBusinessPartnerUpdate = null;
                oBusinessPartnerUpdate = (SAPbobsCOM.BusinessPartners)oCompany.GetBusinessObject(BoObjectTypes.oBusinessPartners);

                if (oBusinessPartnerUpdate.GetByKey(cardCodePrefix + document))
                {
                    addBPNumber = oBusinessPartner.Update();

                    if (addBPNumber != 0)
                    {
                        messageError = oCompany.GetLastErrorDescription();
                        this.log.WriteLogTable(oCompany, EnumTipoIntegracao.Cliente, document, cardCodePrefix+document, EnumStatusIntegracao.Erro, messageError);
                        //this.log.WriteLogCliente("InserirBusinessPartner error SAP: " + messageError);
                    }
                    else
                    {
                        messageError = "";
                        this.log.WriteLogTable(oCompany,EnumTipoIntegracao.Cliente,document, cardCodePrefix + document,EnumStatusIntegracao.Sucesso,"Cliente atualizado com sucesso.");
                        //this.Log.WriteLogCliente("BusinessPartner " + cardCodePrefix + document + " atualizado com sucesso.");

                        System.Runtime.InteropServices.Marshal.ReleaseComObject(oBusinessPartner);
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(oBusinessPartnerUpdate);
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(oBusinessPartnerUpdateTest);
                    }

                }
                else
                {
                    //Setando informações Fiscais
                    //oBusinessPartner.FiscalTaxID.SetCurrentLine(0);
                    if (isCorporate)
                    {
                        oBusinessPartner.FiscalTaxID.TaxId0 = document;
                    }
                    else {

                        oBusinessPartner.FiscalTaxID.TaxId4 = document;
                        oBusinessPartner.FiscalTaxID.TaxId1 = "Isento";
                    }
                    //oBusinessPartner.FiscalTaxID.Address = "FATURAMENTO";
                    //oBusinessPartner.FiscalTaxID.Add();

                    addBPNumber = oBusinessPartner.Add();

                    if (addBPNumber != 0)
                    {
                        messageError = oCompany.GetLastErrorDescription();
                        this.log.WriteLogTable(oCompany, EnumTipoIntegracao.Cliente, document, "", EnumStatusIntegracao.Erro, messageError);
                        //Log.WriteLogCliente("InserirBusinessPartner error SAP: " + messageError);
                    }
                    else
                    {
                        string CardCode = oCompany.GetNewObjectKey();
                        this.log.WriteLogTable(oCompany, EnumTipoIntegracao.Cliente, document, CardCode, EnumStatusIntegracao.Sucesso, "Cliente inserido com sucesso.");
                        //Log.WriteLogCliente("BusinessPartner " + cardCodePrefix +CardCode + " inserido com sucesso.");
                        messageError = "";
                    }
                }
                System.Runtime.InteropServices.Marshal.ReleaseComObject(oBusinessPartner);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(oBusinessPartnerUpdateTest);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(oBusinessPartnerUpdate);
            }
            catch (Exception e)
            {
                this.log.WriteLogTable(oCompany, EnumTipoIntegracao.Cliente, document, "", EnumStatusIntegracao.Erro, e.Message);
                this.log.WriteLogPedido("InserirBusinessPartner Exception: " + e.Message);
                throw;
            }

        }

    }
}
