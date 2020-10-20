using System;
using System.Xml.Serialization;

namespace XmlMergeGenerator.XML
{
    [Serializable]
    public class LocalizationItem
    {
        #region Properties

        [XmlAttribute]
        public string Name { get; set; }

        [XmlAttribute]
        public string Value { get; set; }

        #endregion
    }
}