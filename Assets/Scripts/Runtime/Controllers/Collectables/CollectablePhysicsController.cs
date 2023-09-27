using System;
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
        private Transform _stackHolder;
        
        private readonly string _collectable = "Collectable";
        private readonly string _collected = "Collected";
        private readonly string _gate = "Gate";
        private readonly string _atm = "ATM";
        private readonly string _obstacle = "Obstacle";
        private readonly string _conveyor = "Conveyor";
        private readonly string _gunBullet = "GunBullet";
        private readonly string _drone = "DroneArea";
        

        #endregion

        #endregion
        private void Awake()
        {
            _stackHolder= GameObject.Find("StackManager").transform;
        }
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
            if (other.CompareTag(_drone))
            {
               // manager.InteractionWithDroneArea(transform.parent.gameObject);
                

 
            }
             
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