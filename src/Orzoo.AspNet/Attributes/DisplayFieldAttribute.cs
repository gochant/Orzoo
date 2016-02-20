using System;
using System.Web.Mvc;

namespace Orzoo.AspNet.Attributes
{
    public class DisplayFieldAttribute : Attribute, IMetadataAware
    {
        #region Constructors

        public DisplayFieldAttribute(string fieldName)
        {
            DisplayField = fieldName;
        }

        #endregion

        #region Properties, Indexers

        public string DisplayField { get; private set; }

        #endregion

        #region IMetadataAware Members

        public void OnMetadataCreated(ModelMetadata metadata)
        {
            metadata.AdditionalValues["DisplayField"] = DisplayField;
        }

        #endregion
    }
}