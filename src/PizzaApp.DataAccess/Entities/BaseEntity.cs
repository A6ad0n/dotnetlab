using System.ComponentModel.DataAnnotations;

namespace PizzaApp.DataAccess.Entities;

public class BaseEntity : IBaseEntity
{
    public int Id { get; set; }
    public Guid ExternalId { get; set; }
    public DateTime ModificationTime { get; set; }
    public DateTime CreationTime { get; set; }
}