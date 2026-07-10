using Data.Enums;

namespace Features.Interaction
{
    public interface IInteractable
    {
        void OnPlayerEnter();
        void OnPlayerExit();
        bool IsActive { get; }
        StationType StationType { get; }
    }
}