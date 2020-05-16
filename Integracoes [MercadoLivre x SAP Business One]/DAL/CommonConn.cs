using IntegracoesML.Util;
using System.Configuration;

namespace IntegracoesML.DAL
{
    public class CommonConn
    {
        public static SAPbobsCOM.Company oCompany;

        public static SAPbobsCOM.Company InitializeCompany()
        {
            try
            {
                //Log.WriteLog("Inicializando conexão com o BD e SAP.");

                oCompany = new SAPbobsCOM.Company
                {
                    Server = ConfigurationManager.AppSettings["Server"],
                    language = SAPbobsCOM.BoSuppLangs.ln_English,
                    UseTrusted = false,
                    DbServerType = SAPbobsCOM.BoDataServerTypes.dst_MSSQL2016,
                    CompanyDB = ConfigurationManager.AppSettings["DataBase"],
                    UserName = ConfigurationManager.AppSettings["SapUser"],
                    Password = ConfigurationManager.AppSettings["SapPassword"],
                    DbUserName = ConfigurationManager.AppSettings["DbUser"],
                    DbPassword = ConfigurationManager.AppSettings["DbPassword"],
                    //LicenseServer = "sapsrv:30000"
                };

                if (oCompany.Connected == true)
                {
                    oCompany.Disconnect();
                }

                long lRetCode = oCompany.Connect();

                if (lRetCode != 0)
                {
                    int temp_int = 0;
                    string temp_string = "";
                    oCompany.GetLastError(out temp_int, out temp_string);

                    //Log.WriteLog("InitializeCompany Error: " + temp_string);
                    return oCompany;
                }
                else
                {
                    //Log.WriteLog("Conexão realizada com sucesso.");
                    return oCompany;
                    //Inserir na tabela de log
                }
            }
            catch (System.Exception e)
            {
                //Log.WriteLog("InitializeCompany Exception: "+e.Message);
                throw e;
            }
        }

        public static void FinalizeCompany()
        {
            if (oCompany.Connected == true)
            {
                oCompany.Disconnect();
            }
        }
    }
}
