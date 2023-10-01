using System;
using DG.Tweening;
using Runtime.Managers;
using Runtime.Signals;
using Unity.VisualScripting;
using UnityEngine;

namespace Runtime.Controllers.Collectables
{
    public class CollectablePhysicsController : MonoBehaviour
    {
        #region Self Variables

        #region Serialized Variables

        [SerializeField] private CollectableManager manager;
        [SerializeField] private GameObject animator;
        [SerializeField] private CapsuleCollider testCollider;

        #endregion

        #region Private Variables
     
        private byte stackInit=0;
        
        private readonly string _collectable = "Collectable";
        private readonly string _collected = "Collected";
        private readonly string _gate = "Gate";
        private readonly string _atm = "ATM";
        private readonly string _obstacle = "Obstacle";
        private readonly string _conveyor = "Conveyor";
        private readonly string _gunBullet = "GunBullet";
        private readonly string _drone = "DroneArea";
        private readonly string _droneGate = "DroneGate";
        

        #endregion

        #endregion
       
        private void OnTriggerExit(Collider other)
        {
            
            if (other.CompareTag("StageArea"))
            {
                manager.InteractionWithPlayer(animator);
                
                testCollider.enabled = true;
                
            }
        }
        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag("StageArea"))
            {
                if (manager.currentColorType==other.GetComponentInParent<GateManager>().currentColorType)
                {
                   
                    testCollider.isTrigger = false;
                }
                else
                {
                    testCollider.enabled = true;
                    manager.InteractionWithTurret(gameObject);
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(_droneGate))
            {
                manager.InteractionWithStage(animator);
                
            }
          if (other.CompareTag(_droneGate)&&(manager.currentColorType==other.gameObject.GetComponent<DroneAreaManager>().currentColorType))
            {
               // PlayerSignals.Instance.onSetTotalScore?.Invoke(1);
               // ScoreSignals.Instance.onTestAciton?.Invoke();
                Debug.Log("1 artacaktı");

            }
           /*      if (other.CompareTag(_drone))
            {
              DOVirtual.DelayedCall(.3f,
                    () => StackSignals.Instance.onFirstInFirstOutSignal?.Invoke());

               transform.parent.gameObject.transform.DOMove(new Vector3(transform.position.x, transform.position.y,
                    other.transform.position.z), 2);
                // 

            }*/
             
           /* if (other.CompareTag(_collectable) && CompareTag(_collected))
            {
                other.tag = _collected;
                manager.InteractionWithCollectable(other.transform.parent.gameObject);
            }*/
           if (other.CompareTag("StageArea"))
           {
               
               manager.InteractionWithStage(animator);
               
               
           }
            if (other.CompareTag(_gate) && CompareTag(_collected))
            {
                //manager.CollectableUpgrade(manager.GetCurrentValue());
                manager.InteractionWithGate(transform.parent.gameObject);
                manager.currentColorType=other.GetComponent<GateManager>().currentColorType;

            }

            if (other.CompareTag(_atm) && CompareTag(_collected))
            {
                manager.InteractionWithAtm(transform.parent.gameObject);
                
            } 
            if (other.CompareTag(_gunBullet) && CompareTag(_collected))
            {
                manager.InteractionWithAtm(transform.parent.gameObject);
                
            }

            if (other.CompareTag(_obstacle) && CompareTag(_collected))
            {
                manager.InteractionWithObstacle(transform.parent.gameObject);
            }

            if (other.CompareTag(_conveyor) && CompareTag(_collected))
            {
                manager.InteractionWithConveyor();
            }

            if (other.CompareTag("Player") /*|| other.CompareTag(_collected)*/)
            {
                if (manager.currentColorType==other.GetComponentInParent<PlayerManager>().currentColor)
                {
                    manager.InteractionWithPlayer(animator);
                    manager.ChangeMaterial(manager.baseMaterial);
                }
                
               // manager.ChangeMaterial(other.GetComponentInChildren<SkinnedMeshRenderer>().material);
            }

 
        }
    }
}