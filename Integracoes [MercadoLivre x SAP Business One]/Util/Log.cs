using IntegracoesML.DAL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegracoesML.Util
{
    public class Log
    {
        
        private static string _filePathRetNF = ConfigurationManager.AppSettings["pathLogRetNF"];
        private static string _filePathEstoque = ConfigurationManager.AppSettings["pathLogEstoque"];
        private static string _filePathPedido = ConfigurationManager.AppSettings["pathLogPedido"];

        public void WriteLogEstoque(string message)
        {
            try
            {
                using (StreamWriter sw = File.AppendText(_filePathEstoque))
                {
                    sw.WriteLine(DateTime.Now.ToString() + ": " + message);
                    sw.Flush();
                    sw.Close();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void WriteLogRetNF(string message)
        {
            try
            {
                using (StreamWriter sw = File.AppendText(_filePathRetNF))
                {
                    sw.WriteLine(DateTime.Now.ToString() + ": " + message);
                    sw.Flush();
                    sw.Close();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void WriteLogPedido(string message)
        {
            try
            {
                using (StreamWriter sw = File.AppendText(_filePathPedido))
                {
                    sw.WriteLine(DateTime.Now.ToString() + ": " + message);
                    sw.Flush();
                    sw.Close();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void WriteLogTable(SAPbobsCOM.Company company, string tipoDocumento, string numVtex, string numSAP, string status, string mensagem)
        {
            try
            {
                if (company.Connected)
                {
                    SAPbobsCOM.UserTable userTableLog = (SAPbobsCOM.UserTable)company.UserTables.Item("LOG_INTEGRACOESML");
                    
                    userTableLog.UserFields.Fields.Item("U_data").Value = DateTime.Now.ToString();
                    userTableLog.UserFields.Fields.Item("U_tipoDocumento").Value = tipoDocumento;
                    userTableLog.UserFields.Fields.Item("U_numML").Value = numVtex;
                    userTableLog.UserFields.Fields.Item("U_numSAP").Value = numSAP;
                    userTableLog.UserFields.Fields.Item("U_status").Value = status;
                    userTableLog.UserFields.Fields.Item("U_mensagem").Value = mensagem;

                    userTableLog.Add();

                    System.Runtime.InteropServices.Marshal.ReleaseComObject(userTableLog);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }


    }
}
