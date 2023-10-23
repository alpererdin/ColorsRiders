using System;
using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;
using MK.Toon;
using Runtime.Signals;

public class DroneManager : MonoBehaviour
{
    public GameObject droneTestObj;
    public float endXPosition;
    public float duration ;
    public GameObject bulletPrefab;
    private Vector3 initialPosition;
    private List<GameObject> targetList;
    private int currentTargetIndex = 0;

    private GameObject targetObj;
    private void OnEnable()
    {
        CoreGameSignals.Instance.onPlayDroneAnim += droneAnim;
        CoreGameSignals.Instance.wrongSideDestroyer += destroy;
    }

    private void destroy(GameObject arg0)
    {
        targetObj = arg0;
        
    }

    private void Start()
    {
        droneTestObj.SetActive(false);
        currentTargetIndex = 0;
        targetList.Clear();
    }

    private void droneAnim(List<GameObject> list)
    {
        
        initialPosition = droneTestObj.transform.position;
        targetList = list;
        currentTargetIndex = 0;
        DOVirtual.DelayedCall(.5f,
            () =>
                CoreGameSignals.Instance.onOutLineKiller.Invoke(OutlineData.Normal))
            .OnComplete(() =>
            {
                DOVirtual.DelayedCall(.7f,
                    () =>
                
            targetObj.transform.DOScale(new Vector3(0, targetObj.transform.localScale.y, 0), 3f))
                .OnComplete(() =>
            
                {
                    droneTestObj.SetActive(true);
                    MoveToNextTarget();
                
                });

            });
      
        
    }

    private void MoveToNextTarget()
    {
       
                if (currentTargetIndex < targetList.Count)
                {
                    GameObject currentTarget = targetList[currentTargetIndex];

                    droneTestObj.transform.DOMoveY(initialPosition.y + 8f, duration / 2f)
                        .SetEase(Ease.OutQuad)
                        .OnComplete(() =>
                        {
                            droneTestObj.transform.DOMoveX(currentTarget.transform.position.x, duration / 2f)
                                .SetEase(Ease.Linear)
                                .OnComplete(() =>
                                {
                                    ShootBullet(currentTarget);
                                });
                        });
                }
                else
                {
                    droneTestObj.transform.DOMoveX(endXPosition, duration / 2f)
                        .SetEase(Ease.Linear)
                        .OnComplete(() =>
                        {
                            droneTestObj.transform.DOMoveY(initialPosition.y, duration / 2f)
                                .SetEase(Ease.InQuad)
                                .OnComplete(() =>
                                {
                                    droneTestObj.SetActive(false); 
                                    CoreGameSignals.Instance.onOutLineKiller.Invoke(OutlineData.Baked);
                                    CoreGameSignals.Instance.onExitDroneArea?.Invoke();
                        
                                });
                        });

                }
            
       
    }

    private void ShootBullet(GameObject target)
    {
        if (target != null)  
        {
            GameObject bullet = Instantiate(bulletPrefab, droneTestObj.transform.position, Quaternion.identity);
            bullet.transform.DOMove(target.transform.position, .2f)
                .OnComplete(() =>
                {
                    if (Vector3.Distance(bullet.transform.position, target.transform.position) < 0.5f)
                    {
                        Destroy(target);
                    }
                    Destroy(bullet);
                    currentTargetIndex++;
                    MoveToNextTarget();
                });
        }
        else
        {
             
            currentTargetIndex++;
            MoveToNextTarget();
        }
    }

    private void OnDisable()
    {
        CoreGameSignals.Instance.onPlayDroneAnim -= droneAnim;
        CoreGameSignals.Instance.wrongSideDestroyer -= destroy;
    }
}
