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
                _data.JumpSpeed);
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
                float directY = Mathf.Lerp(collectableStack[i].transform.localPosition.y, pos.y, _data.JumpSpeed);
                
                collectableStack[i].transform.localPosition = new Vector3(direct, directY, pos.z);
                
        
            }
        }
        

        /*  public void Execute(float directionX,float directionY, List<GameObject> collectableStack)
        {
            
            float direct = Mathf.Lerp(collectableStack[0].transform.localPosition.x, directionX,
                _data.LerpSpeed);
            float directY = Mathf.Lerp(collectableStack[0].transform.localPosition.y, directionY,
                _data.JumpSpeed);
            
            
            float movementFactor = Mathf.InverseLerp(0f, 5f, Mathf.Abs(collectableStack[0].transform.localPosition.x - direct));
            float rotationY = Mathf.Lerp(-180f, 180+directionX, movementFactor);
            collectableStack[0].transform.rotation = Quaternion.Euler(0f, rotationY, 0f);
            
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
                float directY = Mathf.Lerp(collectableStack[i].transform.localPosition.y, pos.y, _data.JumpSpeed);
               
               
                float movementFactor = Mathf.InverseLerp(0f, 1f, Mathf.Abs(collectableStack[i].transform.localPosition.x - direct));
                float rotationY = Mathf.Lerp(-180, collectableStack[i - 1].transform.rotation.y, movementFactor);
               
               
               
                collectableStack[i].transform.localPosition = new Vector3(direct, directY, pos.z);
                
                
                collectableStack[i].transform.rotation = Quaternion.Euler(0f, rotationY, 0f);
        
            }*/
    }
}