using IntegracoesML.Util;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegracoesML.DAL
{
    public class WarehouseDAL
    {
        private SAPbobsCOM.Company oCompany;

        private Log log;
        internal WarehouseDAL() {
            this.log = new Log();
        }

        public SAPbobsCOM.Recordset RecuperarSaldoEstoqueSAP(SAPbobsCOM.Company company)
        {
            string _query = string.Empty;

            string whsCode = ConfigurationManager.AppSettings["WhsCode"];

            this.oCompany = company;

            SAPbobsCOM.Recordset recordSet = (SAPbobsCOM.Recordset)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);

            try
            {
                

                if (company.Connected)
                {
                    
                    _query = string.Format("Select a.ItemCode, b.OnHand From OITM a " +
                                          "inner join OITW b on a.ItemCode = b.ItemCode " +
                                          "where invntItem = 'Y' and a.SellItem = 'Y' and a.frozenFor = 'N' and WhsCode = '{0}'", whsCode);
                    recordSet.DoQuery(_query);

                    //Log.WriteLog("Query: "+_query);

                    if (recordSet.RecordCount > 0)
                    {
                        this.log.WriteLogEstoque("Foram encontrados " + recordSet.RecordCount + " Items no SAP.");
                        return recordSet;
                    }
                }

                //CommonConn.FinalizeCompany();
                
            }
            catch (Exception e)
            {
                this.log.WriteLogEstoque("Exception recuperarSaldoEstoqueSAP "+e.Message);
                throw;
            }
            return recordSet;
        }

    }
}
