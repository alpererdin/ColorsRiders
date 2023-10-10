using System.Collections.Generic;
using Runtime.Data.ValueObject;
using Runtime.Managers;
using UnityEngine;


namespace Runtime.Commands.Stack
{
    public class SizeStackMoverCommand
    {
        public void Execute(float directionX,float directionY ,float directionZ ,List<GameObject> collectableStack)
        {
            float direct = Mathf.Lerp(collectableStack[0].transform.localPosition.x, directionX,
                5);
            // collectableStack[0].transform.localPosition = new Vector3(direct, 1f, 0.335f);
            
            //y axis
            float directY = Mathf.Lerp(collectableStack[0].transform.localPosition.y, directionY, 5);
            
            float directZ = Mathf.Lerp(collectableStack[0].transform.localPosition.z, directionZ, 5);
            
            collectableStack[0].transform.localPosition = new Vector3(direct, directY, directZ);
            //y ax
            StackItemsLerpMove(collectableStack);
        }

        public void StackItemsLerpMove(List<GameObject> collectableStack)
        {
            for (int i = 1; i < collectableStack.Count; i++)
            {
                Vector3 pos = collectableStack[i].transform.localPosition;
                pos.x = collectableStack[i - 1].transform.localPosition.x;
                pos.y = collectableStack[i - 1].transform.localPosition.y;
                pos.z = collectableStack[i - 1].transform.localPosition.z;
                float direct = Mathf.Lerp(collectableStack[i].transform.localPosition.x, pos.x, 5);
                //
                float directY = Mathf.Lerp(collectableStack[i].transform.localPosition.y, pos.y, 5);
                float directZ = Mathf.Lerp(collectableStack[i].transform.localPosition.z, pos.z, 5);
                
                collectableStack[i].transform.localPosition = new Vector3(direct, directY, directZ);
            }
        }

    }
}