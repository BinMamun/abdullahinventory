using AutoMapper;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Infrastructure.Identity;
using DevSkill.Inventory.Web.Areas.Admin.Models.CategoryModels;
using DevSkill.Inventory.Web.Areas.Admin.Models.ItemModels;
using DevSkill.Inventory.Web.Areas.Admin.Models.MeasurementUnitModels;
using DevSkill.Inventory.Web.Areas.Admin.Models.RoleModels;
using DevSkill.Inventory.Web.Areas.Admin.Models.ServiceModels;
using DevSkill.Inventory.Web.Areas.Admin.Models.StockAdjustmentModels;
using DevSkill.Inventory.Web.Areas.Admin.Models.StockTransferModels;
using DevSkill.Inventory.Web.Areas.Admin.Models.UserModels;
using DevSkill.Inventory.Web.Areas.Admin.Models.WarehouseModel;

namespace DevSkill.Inventory.Web.Modules
{
    public class WebProfile : Profile
    {
        public WebProfile()
        {
            CreateMap<CategoryCreateModel, ItemCategory>().ReverseMap();
            CreateMap<CategoryUpdateModel, ItemCategory>().ReverseMap();

            CreateMap<MeasurementUnitCreateModel, MeasurementUnit>().ReverseMap();
            CreateMap<MeasurementUnitUpdateModel, MeasurementUnit>().ReverseMap();

            CreateMap<ServiceCreateModel, Service>().ReverseMap();
            CreateMap<ServiceUpdateModel, Service>().ReverseMap();

            CreateMap<ItemCreateModel, Item>().ReverseMap();
            CreateMap<Item, ItemUpdateModel>().ForMember(x => x.Picture, opt => opt.Ignore());
            CreateMap<ItemUpdateModel, Item>().ForMember(x => x.Picture, opt => opt.Ignore());

            CreateMap<WarehouseCreateModel, Warehouse>().ReverseMap();
            CreateMap<WarehouseUpdateModel, Warehouse>().ReverseMap();

            CreateMap<StockTransferCreateModel, StockTransfer>().ReverseMap();
            CreateMap<StockTransferItemModel, StockTransferItem>().ReverseMap();

            CreateMap<StockAdjustmentCreateModel, StockAdjustment>().ReverseMap();
            CreateMap<StockAdjustmentItemModel, StockAdjustmentItem>().ReverseMap();

            CreateMap<RoleUpdateModel, ApplicationRole>().ReverseMap();

            CreateMap<UserUpdateModel, ApplicationUser>().ReverseMap();

        }
    }
}
