using System.Collections.Generic;
using Runtime.Data.ValueObject;
using Runtime.Managers;
using UnityEngine;

namespace Runtime.Commands.Stack
{
    public class StackRemoverOnStackCommand
    {
        private StackManager _stackManager;
        private List<GameObject> _collectedStack;
        private Transform _stackHolder;
        private StackData _data;

        public StackRemoverOnStackCommand(StackManager stackManager, ref List<GameObject> collectedStack,
            ref StackData stackData)
        {
            _stackManager = stackManager;
            _collectedStack = collectedStack;
            _stackHolder = GameObject.Find("StackManager").transform;
            _data = stackData;
        }

        public void Execute()
        {
            Transform firstChild = _stackHolder.GetChild(0);  
            GameObject firstChildObject = firstChild.gameObject;
        }
    }
}