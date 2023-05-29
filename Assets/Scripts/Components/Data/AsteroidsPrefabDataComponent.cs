using Unity.Entities;

namespace Components.Data
{
    [GenerateAuthoringComponent]
    public struct AsteroidsPrefabDataComponent : IComponentData
    {
        public Entity LargeAsteroidPrefab;
        public Entity MediumAsteroidPrefab;
        public Entity SmallAsteroidPrefab;
    }
}
