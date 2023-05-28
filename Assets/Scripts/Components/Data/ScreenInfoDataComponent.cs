using Unity.Entities;

namespace Components.Data
{
    public struct ScreenInfoDataComponent : IComponentData
    {
        public float Height;
        public float Width;
    }
}