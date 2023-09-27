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
        public UnityAction<Vector2> onStackFollowPlayer = delegate { };
        
        public UnityAction onUpdateType = delegate { };
        public UnityAction onInteractionConveyor = delegate { };
        public UnityAction onUpdateStack=delegate {  };
        public UnityAction<GameObject> onRemoveStackObject= delegate {  };
        
        public UnityAction<GameObject> onMinigameState=delegate {  };
        
        
        public UnityAction onStackUnFollowPlayer = delegate { };
        
    
        public UnityAction<GameObject> onFirstInFirstOutSignal=delegate {  };
    }
}