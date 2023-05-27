
using Unity.Entities;

namespace Components.Stats
{
    [GenerateAuthoringComponent]
    public struct WeaponStatsComponent : IComponentData
    {
        public float FireRate;
        public float ProjectileSpeed;
        public Entity ProjectileEntity;
    }
}