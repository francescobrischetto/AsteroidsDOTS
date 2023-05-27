using Components;
using Components.Data;
using Unity.Entities;
using UnityEngine;

namespace Systems
{
    public class InputSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            Entities.ForEach((ref InputDataComponent input) =>
            {
                input.InputArrowUp = Input.GetKey(KeyCode.W);
                input.InputArrowLeft = Input.GetKey(KeyCode.A);
                input.InputArrowRight = Input.GetKey(KeyCode.D);
                input.InputAction = Input.GetKey(KeyCode.Space);
            }).Run();
        }
    }
}
