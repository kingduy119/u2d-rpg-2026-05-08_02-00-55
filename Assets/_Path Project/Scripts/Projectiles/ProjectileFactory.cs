
namespace TDGame
{
    // public class ProjectileFactory : MonoBehaviour
    // {
    //     private readonly Dictionary<ProjectileType, GenericPool<Projectile>> m_Pool = new();

    //     [Serializable]
    //     private class Config
    //     {
    //         public ProjectileType type;
    //         public GameObject prefab;
    //     }
    //     [SerializeField] private List<Config> _configs;
    //     private readonly Dictionary<ProjectileType, Config> m_ConfigMap = new();


    //     private void Awake()
    //     {
    //         foreach (var config in _configs)
    //         {
    //             m_ConfigMap.Add(config.type, config);
    //         }
    //     }

    //     private GenericPool<Projectile> CreatePool(Config config)
    //     {
    //         GameObject prefab = config.prefab;
    //         return new GenericPool<Projectile>(prefab, transform);
    //     }

    //     public Projectile GetObject(ProjectileType type)
    //     {
    //         if (!m_ConfigMap.TryGetValue(type, out var config))
    //             return null;

    //         // Get or create new pool by type from dictionary
    //         if (!m_Pool.TryGetValue(type, out var pool))
    //         {
    //             pool = CreatePool(config);
    //             m_Pool.Add(type, pool);
    //         }

    //         return pool.Get();
    //     }
    // }

    public class ProjectileFactory : FactoryAbstract<ProjectileType, Projectile>
    {
    }
}