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
    public GameObject explosionPrefab;
    public AudioClip impact;
    public AudioClip impact2;
    public AudioSource audioSource;
    public Transform shotpoint;
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

                    droneTestObj.transform.DOMoveY(initialPosition.y + 20f, duration / 2f)
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
            GameObject bullet = Instantiate(bulletPrefab, shotpoint.transform.position, Quaternion.identity);
            audioSource.PlayOneShot(impact2, 0.7F);
            bullet.transform.DOMove(target.transform.position, 1f)
                .OnComplete(() =>
                {
                    for (int i = 0; i < targetList.Count; i++)
                    {
                        Destroy(targetList[i]);
                        
                    }
                    Destroy(bullet);
                    ShowExplosion(target.transform.position);
                    currentTargetIndex++;
                    audioSource.PlayOneShot(impact, 0.7F);
                     targetList.Clear();
                     MoveToNextTarget();
                });
        }
        else
        {
             targetList.Clear();
            currentTargetIndex++;
            MoveToNextTarget();
        }
    }
    private void ShowExplosion(Vector3 position)
    {
        Instantiate(explosionPrefab, position, Quaternion.identity);
    }

    private void OnDisable()
    {
        CoreGameSignals.Instance.onPlayDroneAnim -= droneAnim;
        CoreGameSignals.Instance.wrongSideDestroyer -= destroy;
    }
}
