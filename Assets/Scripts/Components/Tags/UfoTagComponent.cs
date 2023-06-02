using Unity.Entities;

namespace Components.Tags
{
    public enum UfoType
    {
        Large,
        Small
    }
    [GenerateAuthoringComponent]
    public struct UfoTagComponent : IComponentData
    {
        public UfoType Type;
    }
}