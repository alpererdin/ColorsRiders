using Runtime.Interfaces;
using Runtime.Signals;

namespace Runtime.Commands.Input
{
    public class OnInputReleasedCommand : ICommand
    {
        public void Execute()
        {
            InputSignals.Instance.onInputReleased?.Invoke();
        }

        public void Execute(int value)
        {
            throw new System.NotImplementedException();
        }
    }
}