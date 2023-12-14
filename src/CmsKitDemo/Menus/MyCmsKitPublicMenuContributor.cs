using Volo.Abp.Features;
using Volo.Abp.GlobalFeatures;
using Volo.Abp.UI.Navigation;
using Volo.CmsKit.Features;
using Volo.CmsKit.GlobalFeatures;
using Volo.CmsKit.Localization;
using Volo.CmsKit.Menus;
using Volo.CmsKit.Public.Menus;
using Volo.CmsKit.Public.Web.Menus;

namespace CmsKitDemo.Menus;

public class MyCmsKitPublicMenuContributor: IMenuContributor
{
    public async Task ConfigureMenuAsync(MenuConfigurationContext context)
    {
        if (context.Menu.Name == CmsKitMenus.Public)
        {
            await ConfigureMainMenuAsync(context);
        }
    }

    private async Task ConfigureMainMenuAsync(MenuConfigurationContext context)
    {
        var featureChecker = context.ServiceProvider.GetRequiredService<IFeatureChecker>();
        if (GlobalFeatureManager.Instance.IsEnabled<MenuFeature>() && await featureChecker.IsEnabledAsync(CmsKitFeatures.MenuEnable))
        {
            var menuAppService = context.ServiceProvider.GetRequiredService<IMenuItemPublicAppService>();

            var menuItems = await menuAppService.GetListAsync();
            var l = context.GetLocalizer<CmsKitResource>();

            if (!menuItems.IsNullOrEmpty())
            {
                foreach (var menuItemDto in menuItems.Where(x => x.ParentId == null && x.IsActive))
                {
                    menuItemDto.DisplayName = l[menuItemDto.DisplayName];
                    AddChildItems(menuItemDto, menuItems, context.Menu);
                }
            }
        }
    }

    private void AddChildItems(MenuItemDto menuItem, List<MenuItemDto> source, IHasMenuItems parent = null)
    {
        var applicationMenuItem = CreateApplicationMenuItem(menuItem);

        foreach (var item in source.Where(x => x.ParentId == menuItem.Id && x.IsActive))
        {
            AddChildItems(item, source, applicationMenuItem);
        }

        parent?.Items.Add(applicationMenuItem);
    }

    private ApplicationMenuItem CreateApplicationMenuItem(MenuItemDto menuItem)
    {
        return new ApplicationMenuItem(
            menuItem.DisplayName,
            menuItem.DisplayName,
            menuItem.Url,
            menuItem.Icon,
            menuItem.Order,
            menuItem.Target,
            menuItem.ElementId,
            menuItem.CssClass
        );
    }
}
