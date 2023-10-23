using System;
using Runtime.Extentions;
using UnityEngine;
using UnityEngine.Events;

namespace Runtime.Signals
{
    public class StackSignals : MonoSingleton<StackSignals>
    {
        public UnityAction<GameObject> onInteractionATM = delegate { };
        public UnityAction<GameObject> onInteractionObstacle = delegate { };
        public UnityAction<GameObject> onInteractionCollectable = delegate { };
        public UnityAction<Vector3> onStackFollowPlayer = delegate { };
        
        public UnityAction onUpdateType = delegate { };
        public UnityAction onInteractionConveyor = delegate { };
        public UnityAction onUpdateStack=delegate {  };
        public UnityAction<GameObject> onRemoveStackObject= delegate {  };
        
        public UnityAction<GameObject> onMinigameState=delegate {  };
        
        
        public UnityAction onStackUnFollowPlayer = delegate { };
        
    
       

        public Action<Transform> onStackEnterDroneArea;
        public UnityAction onLastCollectableEnterDroneArea = delegate { };
        
        public Func<int> onSetStackCount= delegate { return 0; };
        
        public UnityAction<GameObject> droneareaAdder = delegate {  };
        
        public UnityAction<GameObject> droneareaExitAdder = delegate {  };
        
        public UnityAction JumperArea= delegate {  };
        
        public UnityAction onPrepareBuildingStae=delegate {  };
        public UnityAction onRemoveFromSize=delegate {  };

        
        
        public UnityAction<Vector3> onSizeStackFollowPlayer = delegate { };

        public UnityAction<bool ,Transform> isPlayerBuildState = delegate {  };
        
        public UnityAction<GameObject> wrongSideAdder = delegate {  };
      
    }
}