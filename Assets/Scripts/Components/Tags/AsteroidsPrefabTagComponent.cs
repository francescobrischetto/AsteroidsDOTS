using Unity.Entities;

namespace Components.Tag
{
    //Used as a "Singleton" to get Large/Medium/Small Asteroids entities that will be spawned by SplitAsteroidSystem
    [GenerateAuthoringComponent]
    public struct AsteroidsPrefabTagComponent : IComponentData
    {
    }
}
