using Unity.Entities;

namespace Components.Data
{
    [GenerateAuthoringComponent]
    public struct PowerUpDataComponent : IComponentData
    {
        public float MaxTime;
        public float ElapsedTime;
        public Entity TargetEntity;
    }
}