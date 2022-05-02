using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamoDbMigrator.Writer
{
    abstract class ScriptWriter
    {
        protected bool WriteToFile(string outputPath, string fileName, string fileExtension, string content)
        {
            if (!Directory.Exists(outputPath)) Directory.CreateDirectory(outputPath);

            var fullPath = Path.Combine(outputPath, $"{fileName}.{fileExtension}");

            if (File.Exists(fullPath)) File.Delete(fullPath);

            using (StreamWriter outputFile = new StreamWriter(fullPath))
            {
                outputFile.WriteLine(content);
            }

            return true;
        }

        public abstract void Write(string outputPath, string fileName, string content);
    }
}
