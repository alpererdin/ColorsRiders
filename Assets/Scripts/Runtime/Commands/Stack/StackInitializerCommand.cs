using Runtime.Managers;
using Runtime.Signals;
using UnityEngine;

namespace Runtime.Commands.Stack
{
    public class StackInitializerCommand
    {
        private StackManager _stackManager;
        private GameObject _money;

        public StackInitializerCommand(StackManager stackManager,
            ref GameObject money)
        {
            _stackManager = stackManager;
            _money = money;
        }

        public void Execute()
        {
           // var stackLevel =StackSignals.Instance.onSetStackCount();
            //var stackLevel = CoreGameSignals.Instance.onGetStackLevel();
            var stackLevel =StackSignals.Instance.onSetStackCount();
            
            for (int i = 1; i < stackLevel; i++)
            {
                Debug.Log("kadar calisir");
               GameObject obj = Object.Instantiate(_money);
               _stackManager.AdderOnStackCommand.Execute(obj);
            }

            _stackManager.StackTypeUpdaterCommand.Execute();
        }
    }
}