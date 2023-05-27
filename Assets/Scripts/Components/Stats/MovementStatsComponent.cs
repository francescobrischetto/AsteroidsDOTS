using Unity.Entities;

namespace Components.Stats
{
    [GenerateAuthoringComponent]
    public struct MovementStatsComponent : IComponentData
    {
        public float MovementFriction;
        public float Acceleration;
        public float TurnRate;
    }
}