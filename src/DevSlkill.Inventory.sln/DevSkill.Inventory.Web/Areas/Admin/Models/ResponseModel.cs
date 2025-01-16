namespace DevSkill.Inventory.Web.Areas.Admin.Models
{
    public enum ResponseTypes
    {
        Success,
        Danger,
        Warning
    }
    public class ResponseModel
    {
        public string? Message { get; set; }
        public ResponseTypes Type { get; set; }
    }
}
