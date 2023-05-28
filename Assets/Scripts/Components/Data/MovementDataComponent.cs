using Unity.Entities;
using Unity.Mathematics;

namespace Components.Data
{
    [GenerateAuthoringComponent]
    public struct MovementDataComponent : IComponentData
    {
        public float3 CurrentVelocity;
        public float CurrentAcceleration;
        public float CurrentTurnAngle;
        public float3 PreviousPosition;
    }
}