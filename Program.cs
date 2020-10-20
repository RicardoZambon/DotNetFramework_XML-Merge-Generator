using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace XmlMergeGenerator
{
    public class Program
    {
        #region Properties

        private static List<Languages> _Languages;
        public static List<Languages> Languages
        {
            get
            {
                if (_Languages == null)
                {
                    _Languages = new List<Languages>();
                    try
                    {
                        (ConfigurationManager.AppSettings.GetValues("Languages")[0]).ToString().Split(',').Distinct().Where(x => x != string.Empty).ToList().ForEach(x => _Languages.Add(new Languages(x))); ;
                    }
                    catch { }
                }
                return _Languages;
            }
        }

        private static string[] _SourceFiles;
        public static string[] SourceFiles
        {
            get
            {
                if (_SourceFiles == null)
                    try
                    {
                        _SourceFiles = (ConfigurationManager.AppSettings.GetValues("SourceFiles")[0]).ToString().Split(',');
                    }
                    catch
                    {
                        _SourceFiles = new string[0];
                    }
                return _SourceFiles;
            }
        }

        private static string[] _SourceFilesLanguageSeparator;
        public static string[] SourceFilesLanguageSeparator
        {
            get
            {
                if (_SourceFilesLanguageSeparator == null)
                    try
                    {
                        _SourceFilesLanguageSeparator = (ConfigurationManager.AppSettings.GetValues("SourceFilesLanguageSeparator")[0]).ToString().Split(',');
                    }
                    catch
                    {
                        _SourceFilesLanguageSeparator = new string[0];
                    }
                return _SourceFilesLanguageSeparator;
            }
        }

        private static string _DestPath;
        public static string DestPath
        {
            get
            {
                if (_DestPath == null)
                    try
                    {
                        _DestPath = (ConfigurationManager.AppSettings.GetValues("TargetFile")[0]).ToString();
                    }
                    catch
                    {
                        _DestPath = string.Empty;
                    }
                return _DestPath;
            }
        }

        #endregion

        public static void Main(string[] args)
        {
            try
            {
                if (Languages.Count() == 0 || SourceFiles.Count() == 0 || DestPath == string.Empty) return;

                var mergedTranslations = new List<XML.LocalizationGroup>();
                foreach (var language in Languages)
                {
                    var group = new XML.LocalizationGroup(language.Name);
                    foreach (string file in SourceFiles)
                    {
                        var originalFile = GetTranslations(file);
                        if (originalFile != null) group.AddItems(originalFile);

                        var translatedFile = GetTranslations(Path.GetDirectoryName(file) + @"\" + Path.GetFileNameWithoutExtension(file) + GetSourceFileSeparator(file) + language.FileName + Path.GetExtension(file));
                        if (translatedFile != null) group.AddItems(translatedFile);
                    }
                    mergedTranslations.Add(group);
                }

                var staticTexts = new XML.StaticTexts();
                staticTexts.Localization = mergedTranslations.ToArray();
                SaveTranslations(staticTexts, DestPath);
                Log.GravarLog(string.Format("Processamento XML concluído com sucesso. Arquivo de destino: '{0}'.", System.IO.Path.GetFullPath(DestPath)));
            }
            catch (Exception ex)
            {
                Log.GravarLog(string.Format("Ocorreu um erro no processamento do arquivo. Erro: {0}. StackTrace: {1}", ex.Message, ex.StackTrace));
            }
        }

        #region Functions And Methods

        private static List<XML.LocalizationGroup> GetTranslations(string file)
        {
            var serializer = new XmlSerializer(typeof(XML.Application));
            try
            {
                using (var reader = new StreamReader(file))
                {
                    var app = (XML.Application)serializer.Deserialize(reader);
                    return (app.Localization.FirstOrDefault(x => x.Name == "StaticTexts") != null ? app.Localization.FirstOrDefault(x => x.Name == "StaticTexts").ChildGroups.ToList() : null);
                }
            }
            catch (Exception ex)
            {
                Log.GravarLog(string.Format("Falha ao recuperar o arquivo de origem '{0}'. Erro: {1}. StackTrace: {2}", file, ex.Message, ex.StackTrace));
                return null;
            }
        }

        private static void SaveTranslations(XML.StaticTexts fileContent, string fileDestination)
        {
            var serializer = new XmlSerializer(typeof(XML.StaticTexts));
            try
            {
                using (var writer = new StreamWriter(fileDestination))
                    serializer.Serialize(writer, fileContent);
            }
            catch (Exception ex)
            {
                Log.GravarLog(string.Format("Falha ao salvar o arquivo '{0}'. Erro: {1}. StackTrace: {2}", fileDestination, ex.Message, ex.StackTrace));
            }
        }

        private static string GetSourceFileSeparator(string source)
        {
            try
            {
                return SourceFilesLanguageSeparator[SourceFiles.ToList().IndexOf(source)];
            }
            catch { return string.Empty; }
        }

        #endregion
    }
}