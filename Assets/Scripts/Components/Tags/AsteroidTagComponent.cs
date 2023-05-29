using Unity.Entities;

namespace Components.Tags
{
    public enum AsteroidType
    {
        Large,
        Medium,
        Small
    }

    [GenerateAuthoringComponent]
    public struct AsteroidTagComponent : IComponentData
    {
        public AsteroidType Type;
    }
}