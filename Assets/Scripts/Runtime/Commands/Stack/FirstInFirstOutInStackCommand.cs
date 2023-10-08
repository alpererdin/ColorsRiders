using System.Collections.Generic;
using Runtime.Managers;
using Runtime.Signals;
using UnityEngine;

using DG.Tweening;

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

        public void Execute()
        {
             
       
            // int index = _collectableStack.IndexOf(collectableGameObject);
            
            int last = 0;
             _collectableStack[0].transform.SetParent(_levelHolder.transform.GetChild(0));
             _collectableStack[0].transform.DOMove(new Vector3(_collectableStack[0].transform.position.x,
                 _collectableStack[0].transform.position.y,
                 _collectableStack[0].transform.position.z + 4), 2);
            // _stackManager.StackJumperCommand.Execute(last, index);
            _collectableStack.RemoveAt(last);
            _collectableStack.TrimExcess();
           _stackManager.UpdateStack();
           PlayerSignals.Instance.onSetTotalScore?.Invoke(-1);
            //_stackManager.StackTypeUpdaterCommand.Execute();
            if (_collectableStack.Count == 0)
            {
                //Debug.Log("sonobje de gecebilirdi");
                StackSignals.Instance.onLastCollectableEnterDroneArea?.Invoke();
                _collectableStack.TrimExcess();
                 
            }
        }
    }
}