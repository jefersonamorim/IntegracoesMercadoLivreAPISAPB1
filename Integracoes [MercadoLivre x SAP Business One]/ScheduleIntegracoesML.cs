using HttpParamsUtility;
using IntegracoesML.Business;
using IntegracoesML.DAL;
using IntegracoesML.Entity;
using IntegracoesML.Util;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Web;

namespace IntegracoesML
{
    public partial class ScheduleIntegracoesML : ServiceBase
    {
        private Timer timerEstoque = null;

        private Timer timerPedidos = null;

        private Timer timerRetNF = null;

        private string _path = ConfigurationManager.AppSettings["Path"];

        private Boolean jobIntegracaoPedido = Convert.ToBoolean(ConfigurationManager.AppSettings["jobIntegracaoPedido"]);

        private Boolean jobIntegracaoRetornoNF = Convert.ToBoolean(ConfigurationManager.AppSettings["jobIntegracaoRetornoNF"]);

        private Boolean jobIntegracaoEstoque = Convert.ToBoolean(ConfigurationManager.AppSettings["jobIntegracaoEstoque"]);

        private Log log;

        private SAPbobsCOM.Company oCompany;

        public static MercadoLibre.SDK.MeliCredentials credencial;

        private IntegracaoService integracaoService;

        public ScheduleIntegracoesML()
        {
            this.log = new Log();
            this.oCompany = CommonConn.InitializeCompany();

            IntegracaoService integracaoService = new IntegracaoService();

            integracaoService.GetNewAccessToken();

            InitializeComponent();
        }

        public async Task TesteAsync()
        {
            IntegracaoService integracaoService = new IntegracaoService();

            //integracaoService.GetNewAccessToken();

            //integracaoService.IniciarIntegracaoPedido(this.oCompany);
            integracaoService.RetornoNotaFiscal(this.oCompany);
            //integracaoService.IniciarIntegracaoEstoque(oCompany);
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                if (jobIntegracaoPedido)
                {
                    this.timerPedidos = new Timer();

                    string intervaloExecucaoPedido = ConfigurationManager.AppSettings["intervaloExecucaoPedido"];

                    this.timerPedidos.Interval = Convert.ToInt32(intervaloExecucaoPedido);

                    timerPedidos.Enabled = true;

                    this.timerPedidos.Elapsed += new System.Timers.ElapsedEventHandler(this.IntegracaoPedido);
                }

                if (jobIntegracaoRetornoNF)
                {
                    this.timerRetNF = new Timer();

                    string intervaloExecucaoRetNF = ConfigurationManager.AppSettings["intervaloExecucaoRetNF"];

                    this.timerRetNF.Interval = Convert.ToInt32(intervaloExecucaoRetNF);

                    timerRetNF.Enabled = true;

                    this.timerRetNF.Elapsed += new System.Timers.ElapsedEventHandler(this.IntegracaoRetornoNF);
                }

                if (jobIntegracaoEstoque)
                {
                    this.timerEstoque = new Timer();

                    string intervaloExecucaoEstoque = ConfigurationManager.AppSettings["intervaloExecucaoEstoque"] + ",01";

                    this.timerEstoque.Interval = TimeSpan.FromHours(Convert.ToDouble(intervaloExecucaoEstoque)).TotalMilliseconds;

                    timerEstoque.Enabled = true;

                    this.timerEstoque.Elapsed += new System.Timers.ElapsedEventHandler(this.IntegracaoEstoque);
                }
            }
            catch (Exception)
            {
                throw;
            }

        }

        private void IntegracaoPedido(object sender, ElapsedEventArgs e)
        {
            try
            {
                timerPedidos.Enabled = false;
                timerPedidos.AutoReset = false;

                this.log.WriteLogPedido("#### INTEGRAÇÃO DE PEDIDOS INICIALIZADA");

                IntegracaoService integracaoService = new IntegracaoService();

                integracaoService.IniciarIntegracaoPedido(this.oCompany);

                timerPedidos.Enabled = true;

                //System.Runtime.InteropServices.Marshal.ReleaseComObject(oCompany);

                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
            }
            catch (Exception ex)
            {
                this.log.WriteLogPedido("Exception IntegracaoPedido " + ex.Message);
                throw;
            }
        }

        private void IntegracaoRetornoNF(object sender, ElapsedEventArgs e)
        {
            try
            {
                timerRetNF.Enabled = false;
                timerRetNF.AutoReset = false;

                this.log.WriteLogRetNF("#### INTEGRAÇÃO DE RERTORNO DE nf INICIALIZADA");

                IntegracaoService integracaoService = new IntegracaoService();

                integracaoService.RetornoNotaFiscal(this.oCompany);

                timerRetNF.Enabled = true;

                //System.Runtime.InteropServices.Marshal.ReleaseComObject(oCompany);

                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
            }
            catch (Exception ex)
            {
                this.log.WriteLogRetNF("Exception IntegracaoRetornoNF " + ex.Message);
                throw;
            }
        }

        private void IntegracaoEstoque(object sender, ElapsedEventArgs e)
        {
            try
            {
                timerEstoque.Enabled = false;
                timerEstoque.AutoReset = false;

                this.log.WriteLogEstoque("#### INTEGRAÇÃO DE ESTOQUE INICIALIZADA");

                IntegracaoService integracaoService = new IntegracaoService();

                integracaoService.IniciarIntegracaoEstoque(this.oCompany);

                timerEstoque.Enabled = true;

                //System.Runtime.InteropServices.Marshal.ReleaseComObject(oCompany);

                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
            }
            catch (Exception ex)
            {
                this.log.WriteLogPedido("Exception IntegracaoEstoque " + ex.Message);
                throw;
            }
        }

        protected override void OnStop()
        {
            timerEstoque.Stop();
            timerPedidos.Stop();
            timerRetNF.Stop();
        }
    }
}
