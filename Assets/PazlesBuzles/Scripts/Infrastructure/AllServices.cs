public class AllServices
{
    private class Implementation <TService> where TService : IService
    {
        public static TService Instance;
    }

    public static AllServices Instance => _instance ??= new AllServices();

    private static AllServices _instance;

    public void RegisterService<TService>(TService instanceObj) where TService : IService
        => Implementation<TService>.Instance = instanceObj;

    public TService GetService<TService>() where TService : IService
        => Implementation<TService>.Instance;
}
