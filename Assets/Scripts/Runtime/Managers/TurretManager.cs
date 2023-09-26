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

        private GameObject targetToShoot; // Hedef olarak atanacak GameObject
        public float delayBetweenShots = 0.3f; // Ateşler arasındaki gecikme süresi (0.2 saniye)

        private bool isFiring = false; // Ateş etme durumu

        private void OnEnable()
        {
            CoreGameSignals.Instance.onEnterTurretArea += ActivateTurret;
        }

        private void ActivateTurret(GameObject target)
        {
            // Hedef olarak atanacak GameObject'i ayarla
            targetToShoot = target;

            // Eğer ateş etme işlemi devam etmiyorsa, ateş etmeyi başlatın
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
                // Mermiyi ateşle
                GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);

                Vector3 targetPosition = targetToShoot.transform.position+new Vector3(0,1,0);
                Vector3 direction = (targetPosition - bullet.transform.position).normalized;
                bullet.GetComponent<Rigidbody>().velocity = direction * 40;

                float bulletLifetime = 3.0f;
                Destroy(bullet, bulletLifetime);

                // Belirtilen süre sonra bir sonraki hedefe ateş etmeyi başlat
                Invoke("NextShot", delayBetweenShots);
            }
            else
            {
                Debug.LogError("BulletPrefab, FirePoint, or Target not assigned in the TurretManager.");
            }
        }

        private void NextShot()
        {
            // Ateş etmeyi bitir
            isFiring = false;
        }

        private void OnDisable()
        {
            CoreGameSignals.Instance.onEnterTurretArea -= ActivateTurret;
        }
    }
}
