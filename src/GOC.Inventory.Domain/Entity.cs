using System;
namespace GOC.Inventory.Domain
{
    public abstract class Entity<TId> : IEquatable<Entity<TId>>
    {
        
        public TId Id { get; protected set; }

        protected Entity()
        {
        }

        protected Entity(TId id)
        {
            if(object.Equals(id, default(TId)))
            {
                throw new ArgumentException("The ID cannot be the type's default value", nameof(id));
            }
            Id = id;
        }

        public override bool Equals(object obj)
        {
            var entity = obj as Entity<TId>;
            if(entity != null)
            {
                return this.Equals(entity);
            }
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public bool Equals(Entity<TId> other)
        {
            if(other == null){
                return false;
            }
            return Id.Equals(other.Id);
        }
    }
}
