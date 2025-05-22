namespace Repository.Entities;

public abstract class BaseEntity<TEntityId> where TEntityId : notnull
{
    public TEntityId Id { get; set; } = default!;
}
