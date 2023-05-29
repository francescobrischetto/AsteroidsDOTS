using Unity.Entities;
using Unity.Mathematics;

namespace Components.Data
{
    [GenerateAuthoringComponent]
    public struct WeaponDataComponent : IComponentData
    {
        public float FireTimer;
        public float3 ShootDirection;
        public bool IsShooting;
    }
}