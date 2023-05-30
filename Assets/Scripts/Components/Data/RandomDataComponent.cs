using Unity.Entities;
using Random = Unity.Mathematics.Random;

namespace Components.Data
{
    [GenerateAuthoringComponent]
    public struct RandomDataComponent : IComponentData
    {
        public Random Random;
    }
}