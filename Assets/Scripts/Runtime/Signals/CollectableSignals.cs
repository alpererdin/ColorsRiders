using Runtime.Extentions;
using UnityEngine.Events;
using Runtime.Enums;
using Runtime.Managers;
using UnityEngine;
 
namespace Runtime.Signals
{
    public class CollectableSignals : MonoSingleton<CollectableSignals>
    {
        public UnityAction<CollectableAnimationStates, GameObject> onChangeCollectableAnimationState = delegate { };
        
        public UnityAction<Material> onChangeCollectedMaterial=delegate {  };
       
        
        public UnityAction<MaterialColorTypes> onGatePassed=delegate {  };
        
        public UnityAction OnAfterDroneArea=delegate {  };
        
        public UnityAction  InitChangeCollectedMaterial=delegate {  };
        
    }
}