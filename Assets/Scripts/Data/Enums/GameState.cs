namespace Data.Enums
{
    public enum GameState
    {
        Boot,
        Splash,
        MainMenu,
        Gameplay,
        Paused,
        Recipes
    }

    public enum MainMenuSubState
    {
        Root,
        Options,
    }

    public enum PauseSubState
    {
        Root,
        Options
    }
}