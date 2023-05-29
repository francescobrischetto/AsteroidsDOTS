using Unity.Entities;
using Unity.Mathematics;

namespace Components.Data
{
    public struct RandomDataComponent : IComponentData
    {
        public Unity.Mathematics.Random Random;
    }
}