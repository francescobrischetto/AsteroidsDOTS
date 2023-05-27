using Unity.Entities;

namespace Components.Data
{
    public struct InputDataComponent : IComponentData
    {
        public bool InputArrowUp;
        public bool InputArrowLeft;
        public bool InputArrowRight;
        public bool InputAction;
    }
}
