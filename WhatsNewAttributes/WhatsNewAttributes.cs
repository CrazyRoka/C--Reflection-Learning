using System;

namespace WhatsNewAttributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Constructor, AllowMultiple=true, Inherited=false)]
    public class LastModifiedAttribute : Attribute
    {
        public LastModifiedAttribute(string dateModified, string changes)
        {
            DateModified = DateTime.Parse(dateModified); ;
            Changes = changes;
        }

        public DateTime DateModified { get; }

        public string Changes { get; }

        public string Issues { get; set; }
    }

    [AttributeUsage(AttributeTargets.Assembly)]
    public class SupportsWhatsNewAttribute : Attribute
    {
    }
}
