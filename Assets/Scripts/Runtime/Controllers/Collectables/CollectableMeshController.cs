using System;
using Runtime.Data.ValueObject;
using Runtime.Enums;
using Runtime.Managers;
using Runtime.Signals;
using UnityEngine;
using UnityEngine.Events;

namespace Runtime.Controllers.Collectables
{
    public class CollectableMeshController : MonoBehaviour
    {
        #region Self Variables

        #region Serialized Variables

         
        [SerializeField] private CollectableManager manager;
        [SerializeField] private SkinnedMeshRenderer renderer;

        #endregion

        #region Private Variables

        private CollectableMeshData _data;

         
        #endregion

        #endregion

        

        private void OnEnable()
        {
            ActivateMeshVisuals();
             
            CollectableSignals.Instance.onChangeCollectedMaterial += onChangemat;
        }

        public void onChangemat(Material mat)
        {
            renderer.material = mat;
        }
         
         

        internal void SetMeshData(CollectableMeshData meshData)
        {
            _data = meshData;
        }

        private void ActivateMeshVisuals()
        {
            
        }

        internal void UpgradeCollectableVisual(int value)
        {
             
        }
      
    }
}