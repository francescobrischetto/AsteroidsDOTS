using Unity.Entities;

namespace Components.Commands
{
    [GenerateAuthoringComponent]
    public struct MovementCmdComponent : IComponentData
    {
        public int TurnCommand;
        public int ThrustCommand;
    }
}