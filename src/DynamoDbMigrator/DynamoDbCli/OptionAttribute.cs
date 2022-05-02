using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamoDbMigrator.DynamoDbCli
{
    public class OptionAttribute : Attribute
    {
        public string Name { get; set; }
        public bool IsRequired { get; set; }
        public OptionType OptionType { get; set; }

        public OptionAttribute()
        {
            
        }

        public OptionAttribute(string name)
        {
            Name = name;
        }
    }

    public enum OptionType
    {
        KeyValue,
        Json,
        List
    }
}
