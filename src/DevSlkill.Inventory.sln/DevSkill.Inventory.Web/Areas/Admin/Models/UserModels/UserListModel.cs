using Inventory.Domain;

namespace DevSkill.Inventory.Web.Areas.Admin.Models.UserModels
{
    public class UserListModel : DataTables
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string? Role { get; set; }
    }
}
