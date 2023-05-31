using Unity.Entities;

namespace Components.Stats
{
    public enum PowerUpType
    {
        TriShot,
        DoubleFireRate,
        Invulnerable
    }

    [GenerateAuthoringComponent]
    public struct PowerUpStatsComponent : IComponentData
    {
        public PowerUpType Type;
        public float GrantedTime;
    }
}