public class Transitions
{
    public const string stateName = "State"; //The name of the int param in the animator/statemachine

    public enum TransitionToState //The different states the AI can be in, add more states if needed
    {
        IDLE,       //0
        MOVEAWAY    //1
    }
}
