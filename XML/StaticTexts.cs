using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace XmlMergeGenerator.XML
{
    [Serializable, XmlRoot("StaticTexts")]
    public class StaticTexts
    {
        #region Properties

        [XmlElement("LocalizationGroup")]
        public LocalizationGroup[] Localization { get; set; }

        #endregion

        #region Constructors

        public StaticTexts()
        {
            Localization = new List<LocalizationGroup>().ToArray();
        }

        #endregion
    }
}