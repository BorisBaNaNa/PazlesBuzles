public class AllServices
{
    private class Implementation <TService> where TService : IService
    {
        public static TService Instance;
    }

    public static void RegisterService<TService>(TService instanceObj) where TService : IService
        => Implementation<TService>.Instance = instanceObj;

    public static TService GetService<TService>() where TService : IService
        => Implementation<TService>.Instance;
}
