using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IntegradorVETX.DAL
{
    public class BaseUDO
    {
        private CompanyService oCompService;
        private Company oCompany;
        private GeneralData oGeneralData;
        private GeneralService oGeneralService;
        private GeneralDataParams oGeneralParams;

        public BaseUDO()
        {

        }

        public BaseUDO(Company company)
        {
            oCompany = company;
        }

        //Consulta em tabela customizada por objeto, passar nome da tabela e coleção de parâmetros(Nome do campo e valor)
        protected GeneralData Consultar(string nomeObjeto, Dictionary<string, string> parametros)
        {
            oGeneralData = null;

            try
            {
                if (!String.IsNullOrEmpty(nomeObjeto) && parametros != null)
                {
                    oCompService = oCompany.GetCompanyService();
                    oGeneralService = oCompService.GetGeneralService(nomeObjeto);
                    oGeneralParams = ((SAPbobsCOM.GeneralDataParams)(oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralDataParams)));

                    if (parametros != null && parametros.Count > 0)
                    {
                        foreach (var item in parametros)
                        {
                            oGeneralParams.SetProperty(item.Key.ToString(), item.Value);
                        }

                        oGeneralData = oGeneralService.GetByParams(oGeneralParams);
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            return oGeneralData;
        }

        //Exclusão em tabela customizada por objeto, passar nome da tabela e parâmetros(Nome do campo e valor)
        protected bool Excluir(string nomeObjeto, KeyValuePair<string, string> parametro)
        {
            bool retorno = false;

            try
            {
                oCompService = oCompany.GetCompanyService();
                oGeneralService = oCompService.GetGeneralService(nomeObjeto);
                oGeneralParams = ((SAPbobsCOM.GeneralDataParams)(oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralDataParams)));

                if (parametro.Key != null && parametro.Value != null)
                {
                    oGeneralParams.SetProperty(parametro.Key, parametro.Value);
                    oGeneralService.Delete(oGeneralParams);
                    retorno = true;
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            return retorno;
        }

        //Alteração em tabela customizada por objeto, passar nome da tabela e coleção de parâmetros(Nome do campo e valor)
        protected bool Alterar(string nomeObjeto, string codigo, Dictionary<string, string> parametros)
        {
            bool retorno = false;

            try
            {
                oCompService = oCompany.GetCompanyService();
                oGeneralService = oCompService.GetGeneralService(nomeObjeto);
                oGeneralParams = ((SAPbobsCOM.GeneralDataParams)(oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralDataParams)));

                //Recupera Objeto para alteração
                oGeneralParams.SetProperty("Code", codigo);
                oGeneralData = oGeneralService.GetByParams(oGeneralParams);

                if (parametros != null && parametros.Count > 0)
                {
                    foreach (var item in parametros)
                    {
                        oGeneralData.SetProperty(item.Key.ToString(), item.Value.ToString());
                    }

                    oGeneralService.Update(oGeneralData);
                }

                retorno = true;
            }
            catch (Exception e)
            {
                throw e;
            }

            return retorno;
        }

        //Inclusão em tabela customizada por objeto, passar nome da tabela e coleção de parâmetros(Nome do campo e valor)
        protected void Incluir(string nomeObjeto, Dictionary<string, string> parametros)
        {
            try
            {
                oCompService = oCompany.GetCompanyService();
                oCompany.StartTransaction();
                oGeneralService = oCompService.GetGeneralService(nomeObjeto);

                oGeneralData = (SAPbobsCOM.GeneralData)oGeneralService.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralData);

                if (parametros != null && parametros.Count > 0)
                {
                    foreach (var item in parametros)
                    {
                        oGeneralData.SetProperty(item.Key.ToString(), item.Value.ToString());
                    }

                    oGeneralService.Add(oGeneralData);
                }

                if (oCompany.InTransaction)
                {
                    oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_Commit);
                }
            }
            catch (Exception e)
            {
                if (oCompany.InTransaction)
                {
                    oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack);
                }

                throw e;
            }
        }

        //Consulta o o último Id por objeto na tabela customizada
        protected int ConsultarUltimoId(string nomeObjeto)
        {
            int ultimo = 0;

            try
            {
                oCompService = oCompany.GetCompanyService();
                oGeneralService = oCompService.GetGeneralService(nomeObjeto);

                try
                {
                    oGeneralData = (SAPbobsCOM.GeneralData)oGeneralService.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralData);
                    GeneralCollectionParams oGeneralCol = oGeneralService.GetList();

                    List<int> lista = new List<int>();

                    foreach (GeneralDataParams item in oGeneralCol)
                    {
                        string aux = item.GetProperty("Code").ToString();
                        if (aux != "*")
                        {
                            lista.Add(Convert.ToInt32(aux));
                        }
                    }

                    lista.Sort();
                    ultimo = lista.Last();
                }
                catch (Exception e)
                {
                    //Não existe registro no BD
                    if (e.HResult == -2028)
                    {
                        return ultimo;
                    }
                    //Registro Duplicado
                    else if (e.HResult == -2035)
                    {
                        throw e;
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            return ultimo;
        }
    }
}
