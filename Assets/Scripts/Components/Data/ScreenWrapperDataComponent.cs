using Unity.Entities;

namespace Components.Data
{
    public struct ScreenWrapperDataComponent : IComponentData
    {
        public float Bounds;
        public bool IsOffScreenLeft;
        public bool IsOffScreenRight;
        public bool IsOffScreenTop;
        public bool IsOffScreenBottom;
    }
}
