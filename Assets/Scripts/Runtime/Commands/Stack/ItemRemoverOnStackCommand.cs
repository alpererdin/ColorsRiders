using UnityEngine;
using Runtime.Managers;
using System.Collections.Generic;
using Runtime.Signals;

namespace Runtime.Commands.Stack
{
    public class ItemRemoverOnStackCommand
    {
        private StackManager _stackManager;
        private List<GameObject> _collectableStack;
        private Transform _levelHolder;

        public ItemRemoverOnStackCommand(StackManager stackManager, ref List<GameObject> collectableStack)
        {
            _stackManager = stackManager;
            _collectableStack = collectableStack;
            _levelHolder = GameObject.Find("LevelHolder").transform;
        }

        public void Execute(GameObject collectableGameObject)
        {
            if (_collectableStack.Contains(collectableGameObject))
            {
                int index = _collectableStack.IndexOf(collectableGameObject);
                int last = _collectableStack.Count - 1;
                collectableGameObject.transform.SetParent(_levelHolder.transform.GetChild(0));
                collectableGameObject.SetActive(false);
                PlayerSignals.Instance.onSetTotalScore?.Invoke(-1);

                _collectableStack.Remove(collectableGameObject);
                _collectableStack.TrimExcess();
 
                for (int i = index; i < last; i++)
                {
                    if (_collectableStack.Contains(collectableGameObject))
                    {
                        GameObject nextObject = _collectableStack[i + 1];
                        Vector3 newPosition = nextObject.transform.position;
                        newPosition.z += 2f;  
                        nextObject.transform.position = newPosition;
                    }
                }
                _stackManager.UpdateStack();

            }
        }
    }
}