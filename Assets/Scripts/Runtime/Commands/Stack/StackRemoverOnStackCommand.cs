using System.Collections.Generic;
using Runtime.Managers;
using Runtime.Signals;
using UnityEngine;

namespace Runtime.Commands.Stack
{
    public class StackRemoverOnStackCommand
    {
        private StackManager _stackManager;
        private List<GameObject> _collectableStack;
        private Transform _levelHolder;
         

        public StackRemoverOnStackCommand(StackManager stackManager, ref List<GameObject> collectableStack)
        {
            _stackManager = stackManager;
            _collectableStack = collectableStack;
            _levelHolder = GameObject.Find("LevelHolder").transform;
        }

        public void Execute(GameObject collectableGameObject)
        {
            if (_collectableStack.Count==0)
            {
                PlayerSignals.Instance.onPlayConditionChanged?.Invoke(false);
                Debug.Log("arraediniz");
                CoreGameSignals.Instance.onLevelFailed?.Invoke();
              
            }
            else
            {
                int last = _collectableStack.Count - 1;
                _collectableStack.RemoveAt(last);
                _collectableStack.TrimExcess();
                GameObject a = collectableGameObject.transform.GetChild(last).gameObject;
                Object.Destroy(a);
                _stackManager.StackTypeUpdaterCommand.Execute();
            }
           
        }
    }
}