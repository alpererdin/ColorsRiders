using System.Collections.Generic;
using Runtime.Data.ValueObject;
using Runtime.Managers;
using Runtime.Signals;
using UnityEngine;

namespace Runtime.Commands.Stack
{
    public class StackNewInitalizerCommand
    {
        private StackManager _stackManager;
        private List<GameObject> _collectedStack;
         
        public StackNewInitalizerCommand(StackManager stackManager, ref List<GameObject> collectedStack)
        {
            _stackManager = stackManager;
            _collectedStack = collectedStack;
             
            
        }

        public void Execute()
        {
            int counter = _collectedStack.Count ;
            for (int i = 0; i < counter; i++)
            {
                Debug.Log("kadar denedi");
               _stackManager.AdderOnStackCommand.Execute(_collectedStack[i]);
               PlayerSignals.Instance.onSetTotalScore?.Invoke(1);

            }
            _stackManager.StackTypeUpdaterCommand.Execute();
            CollectableSignals.Instance.OnAfterDroneArea.Invoke();

        }
      
    }
}