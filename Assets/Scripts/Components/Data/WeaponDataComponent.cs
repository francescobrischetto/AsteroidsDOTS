using Unity.Entities;

namespace Components.Data
{
    [GenerateAuthoringComponent]
    public struct WeaponDataComponent : IComponentData
    {
        public float FireTimer;
        public bool IsShooting;
    }
}