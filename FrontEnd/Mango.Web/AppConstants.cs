namespace Mango.Web
{
    public static class AppConstants
    {
        static AppConstants()
        {
            ProductApiBase = string.Empty;
            ShoppingCartApi = string.Empty;
            CouponApi = string.Empty;
        }

        public static string ProductApiBase { get; set; }

        public static string ShoppingCartApi { get; set;}

        public static string CouponApi { get; set; }
    }
}
