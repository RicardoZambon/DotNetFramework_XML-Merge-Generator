using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XmlMergeGenerator
{
    public static class Log
    {
        private static string _LogPath;
        private static string LogPath
        {
            get
            {
                if (_LogPath == null)
                    try
                    {
                        _LogPath = (ConfigurationManager.AppSettings.GetValues("LogPath")[0]).ToString();
                    }
                    catch
                    {
                        _LogPath = string.Empty;
                    }

                if (_LogPath == string.Empty)
                    _LogPath = Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
                return _LogPath;
            }
        }

        private static string LogFileName { get { return string.Format("{0}.log", Path.GetFileNameWithoutExtension(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName)); } }

        private static string FullLogPath { get { return Path.Combine(LogPath, LogFileName); } }

        public static void GravarLog(string Mensagem)
        {
            if (!File.Exists(FullLogPath))
                File.Create(FullLogPath).Close();
            File.AppendAllText(FullLogPath, string.Format("{0} - {1}{2}", DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"), Mensagem, Environment.NewLine));
        }
    }
}
