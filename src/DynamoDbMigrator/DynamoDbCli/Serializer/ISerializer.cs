using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamoDbMigrator.DynamoDbCli.Serializer
{
    interface ISerializer
    {
        string Serialize(object obj, FormatCommand formatCommand = null);
    }
}
