using System.Collections.Generic;
using Runtime.Managers;
using UnityEngine;

namespace Runtime.Commands.Stack
{
    public class StackPositionUpdaterCommand
    {
        private StackManager _stackManager;
        private List<GameObject> _collectableStack;
        private Transform _levelHolder;
         

        public StackPositionUpdaterCommand(StackManager stackManager, ref List<GameObject> collectableStack)
        {
            _stackManager = stackManager;
            _collectableStack = collectableStack;
            _levelHolder = GameObject.Find("LevelHolder").transform;
        }

        public void Execute(GameObject collectableGameObject)
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