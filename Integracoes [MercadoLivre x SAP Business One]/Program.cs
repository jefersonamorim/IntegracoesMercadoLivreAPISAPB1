using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace IntegracoesML
{
    static class Program
    {
        /// <summary>
        /// Ponto de entrada principal para o aplicativo.
        /// </summary>
        static void Main()
        {
#if(!DEBUG)
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new ScheduleIntegracoesML()
            };
            ServiceBase.Run(ServicesToRun);
#else
            //CHAMAR CLASSE DE TESTE

            ScheduleIntegracoesML schedule = new ScheduleIntegracoesML();

            schedule.TesteAsync();
#endif
        }
    }
}
