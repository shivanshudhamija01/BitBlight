using Infrastructure.Services.Interfaces;
using UnityEngine;

namespace Infrastructure.Services
{
    public class JoystickInputService : IInputService
    {
        private readonly Joystick _joystick;
        private bool _isActive = true;

        public JoystickInputService(Joystick joystick)
        {
            _joystick = joystick;
        }

        public void SetInputActive(bool isActive)
        {
            _isActive = isActive;
        }

        public Vector2 MoveInput
        {
            get
            {
                if (!_isActive)
                {
                    return Vector2.zero;
                }

                return new Vector2(_joystick.Horizontal, _joystick.Vertical);
            }
        }

        public virtual void ResetJoystick()
        {
            _joystick.ResetJoystick();
        }
    }
}