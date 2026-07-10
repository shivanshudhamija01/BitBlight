using UnityEngine;

namespace Infrastructure.Services.Interfaces
{
    public interface IInputService
    {
        Vector2 MoveInput { get; }
        void SetInputActive(bool isActive);
        void ResetJoystick();
    }
}