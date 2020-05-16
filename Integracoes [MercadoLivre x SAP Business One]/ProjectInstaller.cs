using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.Threading.Tasks;

namespace IntegracoesML
{
    [RunInstaller(true)]
    public partial class InstaladorService : System.Configuration.Install.Installer
    {
        public InstaladorService()
        {
            InitializeComponent();
        }

        private void ServiceInstaller1_AfterInstall(object sender, InstallEventArgs e)
        {

        }
    }
}
