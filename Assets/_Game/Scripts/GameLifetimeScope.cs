using VContainer;
using VContainer.Unity;

public static class SizeFormatter
{
    static readonly string[] Units = { "B", "KB", "MB", "GB", "TB" };

    public static string Format(long bytes)
    {
        double size = bytes;
        int unit = 0;

        while (size >= 1024 && unit < Units.Length - 1)
        {
            size /= 1024;
            unit++;
        }

        // Byte thì không cần số thập phân
        return unit == 0 ? $"{bytes} {Units[0]}" : $"{size:F2} {Units[unit]}";
    }
}

public class GameLifetimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterEntryPoint<GameManager>(Lifetime.Singleton);
        builder.RegisterEntryPoint<FactoryService>(Lifetime.Singleton);
    }

    // private void Start()
    // {
    //     // Debug.Log("GameLifetimeScope Start");
    //     // LoadSceneAsyncs().Forget();
    // }

}
