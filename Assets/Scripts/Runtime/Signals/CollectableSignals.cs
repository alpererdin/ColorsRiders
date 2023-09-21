using Runtime.Extentions;
using UnityEngine.Events;
using Runtime.Enums;
using UnityEngine;
 
namespace Runtime.Signals
{
    public class CollectableSignals : MonoSingleton<CollectableSignals>
    {
        public UnityAction<CollectableAnimationStates, GameObject> onChangeCollectableAnimationState = delegate { };
    }
}