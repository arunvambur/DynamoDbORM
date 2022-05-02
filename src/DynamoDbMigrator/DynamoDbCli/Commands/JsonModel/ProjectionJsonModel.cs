using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamoDbMigrator.DynamoDbCli.Commands.JsonModel
{
    class ProjectionJsonModel
    {
        public List<string> NonKeyAttributes { get; set; }
        public string ProjectionType { get; set; }

        public ProjectionJsonModel()
        {

        }
    }
}
