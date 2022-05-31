namespace Mousetrap
{
    public class AwakeEventArgs : EventArgs
    {
        public AwakeEventArgs(bool isAwakeState, bool isLocked, TimeSpan period = default)
        {
            IsAwakeState = isAwakeState;
            IsLocked = isLocked;
            Period = period;
        }

        public bool IsAwakeState { get; }

        public bool IsLocked { get; }

        public TimeSpan Period { get; }
    }
}