using Unity.Entities;

namespace Components.Data
{
    [GenerateAuthoringComponent]
    public struct UfoIADataComponent : IComponentData
    {
        public float AttemptChangeDirectionTimer;
        public float ChangeDirectionRate;
        public float CurrentTimeSinceDirectionChangeAttempt;
    }
}
