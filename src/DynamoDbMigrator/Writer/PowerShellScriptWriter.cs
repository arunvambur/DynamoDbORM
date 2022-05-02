using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamoDbMigrator.Writer
{
    class PowerShellScriptWriter : ScriptWriter
    {
        public override void Write(string outputPath, string fileName, string content)
        {
            this.WriteToFile(outputPath, fileName, "ps1", content);
        }
    }
}
