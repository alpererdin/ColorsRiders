using Runtime.Extentions;
using UnityEngine.Events;
using Runtime.Enums;
namespace Runtime.Signals
{
    public class CollectableSignals : MonoSingleton<CollectableSignals>
    {
        public UnityAction<int> onCollectableUpgrade = delegate { };
        public UnityAction<CollectableAnimationStates> onChangeCollectableAnimationState = delegate { };
    }
}