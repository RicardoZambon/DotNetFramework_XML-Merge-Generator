using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace XmlMergeGenerator.XML
{
    [Serializable]
    public class LocalizationGroup
    {
        #region Properties

        [XmlAttribute]
        public string Name { get; set; }

        [XmlElement("LocalizationGroup")]
        public LocalizationGroup[] ChildGroups { get; set; }

        [XmlElement("LocalizationItem")]
        public LocalizationItem[] ChildItems { get; set; }

        #endregion

        #region Constructors

        public LocalizationGroup()
        {

        }

        public LocalizationGroup(string Name) : this()
        {
            this.Name = Name;
        }

        #endregion

        #region Functions And Methods

        public void AddItems(List<LocalizationGroup> list)
        {
            if (ChildGroups == null || ChildGroups.Count() == 0)
                ChildGroups = list.ToArray();
            else
                foreach(var group in list)
                    if (ChildGroups.FirstOrDefault(x => x.Name == group.Name) == null)
                        AddGroup(group);
                    else
                    {
                        if (group.ChildGroups != null && group.ChildGroups.Count() > 0)
                            ChildGroups.FirstOrDefault(x => x.Name == group.Name).AddItems(group.ChildGroups.ToList());
                        if (group.ChildItems != null && group.ChildItems.Count() > 0)
                            ChildGroups.FirstOrDefault(x => x.Name == group.Name).AddItems(group.ChildItems.ToList());
                    }
        }

        public void AddItems(List<LocalizationItem> list)
        {
            if (ChildItems == null || ChildItems.Count() == 0)
                ChildItems = list.ToArray();
            else
                foreach (var item in list)
                    if (ChildItems.FirstOrDefault(x => x.Name == item.Name) == null)
                        AddItem(item);
                    else
                        ChildItems.FirstOrDefault(x => x.Name == item.Name).Value = item.Value;
        }

        public void AddGroup(LocalizationGroup group)
        {
            var list = ChildGroups.ToList();
            list.Add(group);
            ChildGroups = list.ToArray();
        }

        public void AddItem(LocalizationItem item)
        {
            var list = ChildItems.ToList();
            list.Add(item);
            ChildItems = list.ToArray();
        }

        #endregion
    }
}