namespace Mango.Services.ShoppingCartAPI
{
    public static class AppConstants
    {
        static AppConstants()
        {
            CouponApi = string.Empty;
        }

        public static string CouponApi { get; set; }

        public static class Cache
        {
            public const int UserCartAbsoluteExpirationMin = 5;
            public const string UserCartCacheKey = "{0}_cart";
        }
    }
}
