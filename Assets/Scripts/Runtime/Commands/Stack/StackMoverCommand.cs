using System.Collections.Generic;
using Runtime.Data.ValueObject;
using Runtime.Managers;
using UnityEngine;

namespace Runtime.Commands.Stack
{
    public class StackMoverCommand
    {
        private StackData _data;
        

        public StackMoverCommand(ref StackData stackData)
        {
            _data = stackData;
             
        }

        public void Execute(float directionX,float directionY, List<GameObject> collectableStack)
        {
            float direct = Mathf.Lerp(collectableStack[0].transform.localPosition.x, directionX,
                _data.LerpSpeed);
            float directY = Mathf.Lerp(collectableStack[0].transform.localPosition.y, directionY,
                _data.LerpSpeed);
            collectableStack[0].transform.localPosition = new Vector3(direct, directY, -1.5f);
            
            StackItemsLerpMove(collectableStack);
            
            
        }

        public void StackItemsLerpMove(List<GameObject> collectableStack)
        {
            for (int i = 1; i < collectableStack.Count; i++)
            {
                Vector3 pos = collectableStack[i].transform.localPosition;
                pos.x = collectableStack[i - 1].transform.localPosition.x;
                pos.y = collectableStack[i - 1].transform.localPosition.y;
                float direct = Mathf.Lerp(collectableStack[i].transform.localPosition.x, pos.x, _data.LerpSpeed);
               //
               float directY = Mathf.Lerp(collectableStack[i].transform.localPosition.y, pos.y, .2f);
                
                collectableStack[i].transform.localPosition = new Vector3(direct, directY, pos.z);
                
        
            }
        }
        

        
    }
}