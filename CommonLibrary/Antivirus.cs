using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace CommonLibrary
{

    public static class Antivirus
    {
        public static bool ScanFile(string filePath, string antivirusExePath, bool IsAllowToAntivirusScan)
        {
            bool response = false;
            if (IsAllowToAntivirusScan == false)
                return true;
            if (File.Exists(filePath))
            {
                Process myProcess;
                myProcess = new Process();
                myProcess.StartInfo.FileName = antivirusExePath;
                string myprocarg = '"'+antivirusExePath +'"'+" /ScanFile "+'"'+filePath + '"';
                myProcess.StartInfo.Arguments = myprocarg;
                myProcess.Start();
                myProcess.WaitForExit();
                Thread.Sleep(2000);
                if (File.Exists(filePath))
                {
                    response = true;
                }

            }
            return response;
        }

    }
}
