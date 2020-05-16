
using IntegracoesML.Entity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;

namespace IntegracoesML.Business
{
    public class FileService
    {
        public List<string> FileExists(string filePath)
        {
            //Filtrar apenas xml's ("*.xml")
            string[] files = System.IO.Directory.GetFiles(filePath, "*.xml", System.IO.SearchOption.TopDirectoryOnly);

            return files.ToList();
        }

        public void ProcessFile(List<string> fileList)
        {
            foreach (string _file in fileList)
            {
                string _draftCode = DateTime.Now.ToString("yyyyMMddHHmmssffff");
                string _draftName = Path.GetFileNameWithoutExtension(_file);

                //NFXml _NFXml = GetXML(_file);

            }
        }

    }
}
