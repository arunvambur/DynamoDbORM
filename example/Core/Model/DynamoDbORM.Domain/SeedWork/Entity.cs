using MediatR;
using System;
using System.Collections.Generic;

namespace DynamoDbORM.Domain.SeedWork
{

    public class BaseEntity
    {
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }

        public BaseEntity()
        {
            var date = DateTime.Now;
            CreatedOn = date;
            UpdatedOn = date;
        }

        public void SetUpdatedOn()
        {
            UpdatedOn = DateTime.Now;
        }
    }
    public class EntityWithId : BaseEntity
    {
        public string Id { get; set; }

        public EntityWithId()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}