namespace ProductService.Application.Permissions;
public static class ProductPermission
{
    public const string Default = "Permissions.Products";
    public const string View = Default + ".View";
    public const string Create = Default + ".Create";
    public const string Edit = Default + ".Edit";
    public const string Delete = Default + ".Delete";
}