using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamoDbMigrator.DynamoDbCli.Serializer
{
    class FormatCommand
    {
        public bool Indent { get; set; }
        public int IndentLevel { get; set; }

        public FormatCommand()
        {
            IndentLevel = 0;
        }

        public string GetIndent()
        {
            return string.Join("", Enumerable.Range(0, IndentLevel).Select(t => "  "));
        }

        public void ResetIndent()
        {
            IndentLevel = 0;
        }

        public void NextLevel()
        {
            IndentLevel++;
        }

        public string NextLine()
        {
            if (!Indent) return " ";
            StringBuilder sb = new StringBuilder();
            sb.Append(" `").Append("\n").Append(GetIndent());
            return sb.ToString();
        }
    }
}
