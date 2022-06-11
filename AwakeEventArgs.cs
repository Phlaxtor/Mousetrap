namespace Mousetrap
{
    public class AwakeEventArgs : EventArgs
    {
        public AwakeEventArgs(bool isAwakeState, bool isLocked)
        {
            IsAwakeState = isAwakeState;
            IsLocked = isLocked;
        }

        public bool IsAwakeState { get; }

        public bool IsLocked { get; }
    }
}