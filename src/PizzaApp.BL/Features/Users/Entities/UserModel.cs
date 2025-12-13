namespace PizzaApp.BL.Features.Users.Entities;

public class UserModel
{
    public int Id { get; set; }
    public Guid ExternalId { get; set; }
    public DateTime CreationTime { get; set; }
    public DateTime ModificationTime { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public List<RoleModel>? Roles { get; set; }
    
    public BlockInformationModel? BlockInformation { get; set; }
}