﻿using System;
using Newtonsoft.Json;

namespace GOC.Inventory.Domain.Events
{
    [JsonObject]
    public class ItemDeleted : IDomainEvent
    {
        public ItemDeleted(Guid id, DateTime dateDeleted, Guid userId)
        {
            ItemDeletedId = id;
            DateOccurredUtc = dateDeleted;
            UserId = userId;
        }

        public Guid ItemDeletedId { get; private set; }

        public DateTime DateOccurredUtc { get; private set; }

        public Guid UserId { get; private set; }

    }
}
