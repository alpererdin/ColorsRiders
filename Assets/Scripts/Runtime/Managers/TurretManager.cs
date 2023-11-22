using System;
using System.Collections.Generic;
using Runtime.Signals;
using UnityEngine;

namespace Runtime.Managers
{
    public class TurretManager : MonoBehaviour
    {
        public GameObject bulletPrefab;
        public Transform firePoint;

        private GameObject targetToShoot; 
        public float delayBetweenShots = 0.3f; 

        private bool isFiring = false;

        private void OnEnable()
        {
            CoreGameSignals.Instance.onEnterTurretArea += ActivateTurret;
            CoreGameSignals.Instance.onExitTurretArea += DeactivateTurret;
            
        }

        private void DeactivateTurret()
        {
            //
            isFiring = false;
             
        }

        private void ActivateTurret(GameObject target)
        {
      
            targetToShoot = target;

         
            if (!isFiring)
            {
                isFiring = true;
                FireAtTarget();
            }
        }

        private void FireAtTarget()
        {
            if (targetToShoot != null && bulletPrefab != null && firePoint != null)
            {
              
                GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);

                Vector3 targetPosition = targetToShoot.transform.position+new Vector3(0,2,0);
                Vector3 direction = (targetPosition - bullet.transform.position).normalized;
                bullet.GetComponent<Rigidbody>().velocity = direction * 40;

                float bulletLifetime = 3.0f;
                Destroy(bullet, bulletLifetime);

            
                Invoke("NextShot", delayBetweenShots);
            }
            else
            {
                Debug.LogError("BulletPref");
            }
        }

        private void NextShot()
        {
            isFiring = false;
        }

        private void OnDisable()
        {
            CoreGameSignals.Instance.onEnterTurretArea -= ActivateTurret;
            CoreGameSignals.Instance.onExitTurretArea -= DeactivateTurret;
        }
    }
}
