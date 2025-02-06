namespace Forja.Domain.Entities.Common;

public abstract class SoftDeletableEntity
{
    public bool IsDeleted { get; set; } = false;
}