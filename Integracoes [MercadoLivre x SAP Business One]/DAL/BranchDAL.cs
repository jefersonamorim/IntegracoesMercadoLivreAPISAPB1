using System;

namespace IntegradorAticOs.DAL
{
    public class BranchDAL : BaseUDO
    {
        private SAPbobsCOM.Company oCompany;
        private SAPbobsCOM.Recordset oRecordSet;

        internal BranchDAL(SAPbobsCOM.Company company)
            : base(company)
        {
            this.oCompany = company;
        }

        public string ValidateBranchId(string branchId)
        {
            try
            {
                oRecordSet = (SAPbobsCOM.Recordset)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);

                oRecordSet.DoQuery(string.Format("SELECT [U_PrefixoFilial] FROM [dbo].[OBPL] WHERE BPLId = {0}", branchId));

                if (oRecordSet.RecordCount == 1)
                {
                    return oRecordSet.Fields.Item("U_PrefixoFilial").Value.ToString();
                }
            }
            catch (Exception)
            {
                return "false";
            }

            return "false";
        }
    }
}
