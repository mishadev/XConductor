using System;

namespace XConductor.Infrastructure.CrossCutting.Shared.Utils
{
    [AttributeUsage(AttributeTargets.Field)]
    public class FieldDescriptionAttribute : Attribute
    {
        public string Description { get; private set; }

        public FieldDescriptionAttribute(string description)
        {
            this.Description = description;
        }

        public override string ToString()
        {
            return Description;
        }
    }
}
