using System;
using Runtime.Controllers.Collectables;
using Runtime.Data.UnityObject;
using Runtime.Data.ValueObject;
using Runtime.Signals;
using Runtime.Enums;
using Unity.VisualScripting;
using UnityEngine;

namespace Runtime.Managers
{
    public class CollectableManager : MonoBehaviour
    {
        #region Self Variables
        
        public MaterialColorTypes currentColorType;
        public SkinnedMeshRenderer _mesh;
        public Material baseMaterial;
        #region Serialized Variables

        [SerializeField] private CollectableMeshController meshController;
        [SerializeField] private CollectablePhysicsController physicsController;
        [SerializeField] private CollectableAnimationController animController;

        #endregion

        #region Private Variables
        private ColorData _colorData;
         private CollectableData _data;
         private byte _currentValue = 0;
         
         private Material _material;

        private readonly string _collectableDataPath = "Data/CD_Collectable";

        #endregion

        #endregion

        private void Awake()
        {
            _data = GetCollectableData();
            SendDataToController();
            GetCollectableColorData();
           Init();
        }

        public void Init()
        {
             
            _material = _mesh.material;
            _material.color = _colorData.Color;
            
        }

        private void GetCollectableColorData() => _colorData = Resources.Load<CD_Color>("Data/CD_Color").Colors[(int)currentColorType];
        private CollectableData GetCollectableData() => Resources.Load<CD_Collectable>(_collectableDataPath).Data;

        private void SendDataToController()
        {
            meshController.SetMeshData(_data.MeshData);
        }

        public void ChangeMaterial(Material mat)
        {
            meshController.onChangemat(mat);
        }
      
        public void CollectableUpgrade(int value)
        {
            if (_currentValue < 2) _currentValue++;
            meshController.UpgradeCollectableVisual(_currentValue);
            StackSignals.Instance.onUpdateType?.Invoke();
        }

        public byte GetCurrentValue()
        {
            return _currentValue;
        }

        public void InteractionWithCollectable(GameObject collectableGameObject)
        {
            StackSignals.Instance.onInteractionCollectable?.Invoke(collectableGameObject);
           

        }
        public void InteractionWithPlayer(GameObject collectableGameObject)
        {
            
            CollectableSignals.Instance.onChangeCollectableAnimationState?.Invoke(CollectableAnimationStates.Run, collectableGameObject);
     
        } 
        public void InteractionWithStage(GameObject collectableGameObject)
        {
            
            CollectableSignals.Instance.onChangeCollectableAnimationState?.Invoke(CollectableAnimationStates.Crouch, collectableGameObject);
     
        }
         

        public void InteractionWithAtm(GameObject collectableGameObject)
        {
            StackSignals.Instance.onInteractionATM?.Invoke(collectableGameObject);
        }

        public void InteractionWithObstacle(GameObject collectableGameObject)
        {
            StackSignals.Instance.onInteractionObstacle?.Invoke(collectableGameObject);
        }

        public void InteractionWithConveyor()
        {
            StackSignals.Instance.onInteractionConveyor?.Invoke();
        } 
        public void InteractionWithGate(GameObject collectedGO)
        {
            SkinnedMeshRenderer thisGO = collectedGO.GetComponentInChildren<SkinnedMeshRenderer>();
   
        }
      
 
    }
}