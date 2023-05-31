using Unity.Entities;

namespace Components.Data
{
    [GenerateAuthoringComponent]
    public struct DestroyableDataComponent : IComponentData
    {
        public bool ShouldBeDestroyed;
        public bool IsInvulnerable;
    }
}