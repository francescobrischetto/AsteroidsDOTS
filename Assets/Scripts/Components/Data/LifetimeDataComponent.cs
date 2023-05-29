using Unity.Entities;

namespace Components.Data
{
    [GenerateAuthoringComponent]
    public struct LifetimeDataComponent : IComponentData
    {
        public float Lifetime;
        public float CurrentTimeSinceSpawning;
    }
}