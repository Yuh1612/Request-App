using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Request.Domain.Base
{
    public interface IBaseEntity
    {
        public Guid Id { get; set; }
    }

    public interface IDeleteEntity
    {
        public bool IsDelete { get; set; }
    }

    public abstract class DeleteEntity : BaseEntity, IDeleteEntity
    {
        public virtual bool IsDelete { get; set; }
    }

    public abstract class BaseEntity : IBaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual Guid Id { get; set; }
    }

    public abstract class Entity : DeleteEntity
    {
        [NotMapped]
        private List<BaseDomainEvent> _events;

        [NotMapped]
        public IReadOnlyList<BaseDomainEvent> Events => _events?.AsReadOnly();

        protected void AddEvent(BaseDomainEvent @event)
        {
            _events = _events ?? new List<BaseDomainEvent>();
            _events.Add(@event);
        }

        protected void RemoveEvent(BaseDomainEvent @event)
        {
            _events.Remove(@event);
        }

        public void ClearEvents()
        {
            _events?.Clear();
        }
    }
}