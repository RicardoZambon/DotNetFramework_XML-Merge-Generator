using System.Linq;

namespace XmlMergeGenerator
{
    public class Languages
    {
        #region Properties

        public string FileName { get; set; }

        private string _Name;
        public string Name
        {
            get { return (_Name == null || _Name == string.Empty ? FileName.Replace("-","").ToLower() : _Name); }
            set { _Name = value; }
        }

        #endregion

        #region Constructors

        public Languages(string Language)
        {
            var lang = Language.Split('#');
            if (lang.Count() > 0) FileName = lang[0];
            if (lang.Count() > 1) Name = lang[1];
        }

        #endregion
    }
}