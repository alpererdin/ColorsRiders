using System.Collections.Generic;
using Runtime.Managers;
using Runtime.Signals;
using UnityEngine;

namespace Runtime.Commands.Stack
{
    public class FirstInFirstOutInStackCommand
    {
        private StackManager _stackManager;
        private List<GameObject> _collectableStack;
        private Transform _levelHolder;
         

        public FirstInFirstOutInStackCommand(StackManager stackManager, ref List<GameObject> collectableStack)
        {
            _stackManager = stackManager;
            _collectableStack = collectableStack;
            _levelHolder = GameObject.Find("LevelHolder").transform;
        }

        public void Execute(GameObject collectableGameObject)
        {
            // int index = _collectableStack.IndexOf(collectableGameObject);
            int last = 0;
             collectableGameObject.transform.SetParent(_levelHolder.transform.GetChild(0));
            
            // _stackManager.StackJumperCommand.Execute(last, index);
            _collectableStack.RemoveAt(last);
            _collectableStack.TrimExcess();
            
            
            //_stackManager.StackTypeUpdaterCommand.Execute();
             
        }
    }
}