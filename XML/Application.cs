using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace XmlMergeGenerator.XML
{
    [Serializable, XmlRoot("Application")]
    public class Application
    {
        #region Properties

        [XmlArray("Localization"), XmlArrayItem("LocalizationGroup", typeof(LocalizationGroup))]
        public LocalizationGroup[] Localization { get; set; }

        #endregion

        #region Constructors

        public Application()
        {
            Localization = new List<LocalizationGroup>().ToArray();
        }

        #endregion
    }
}