
using Unity.Entities;

namespace Components.Stats
{
    [GenerateAuthoringComponent]
    public struct ProjectileStatsComponent : IComponentData
    {
        public float DamagePower;
    }
}