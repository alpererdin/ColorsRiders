using Runtime.Interfaces;
using Runtime.Signals;

namespace Runtime.Commands.Input
{
    public class OnInputTakenCommand : ICommand
    {
        public void Execute()
        {
            InputSignals.Instance.onInputTaken?.Invoke();
        }

        public void Execute(int value)
        {
            throw new System.NotImplementedException();
        }
    }

}