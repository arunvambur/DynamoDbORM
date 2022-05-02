using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamoDbMigrator.DynamoDbCli.Commands.JsonModel
{
    class KeySchemaElementJsonModel
    {
        public string AttributeName { get; set; }
        public string KeyType { get; set; }
    }
}
