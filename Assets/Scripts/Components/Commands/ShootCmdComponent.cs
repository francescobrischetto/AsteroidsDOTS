using Unity.Entities;

namespace Components.Commands
{
    [GenerateAuthoringComponent]
    public struct ShootCmdComponent : IComponentData
    {
        public bool ShootCommand;
    }
}