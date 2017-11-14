using System;

namespace SparqLinq.ObjectModel
{
    [AttributeUsage(AttributeTargets.Class)]
    public class TypeAttribute : Attribute
    {
        public TypeAttribute(string typeUri)
        {
            this.Uri = new Uri(typeUri);
        }

        public Uri Uri { get; private set; }
    }
}
