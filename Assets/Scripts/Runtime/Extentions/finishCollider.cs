using System;
using DG.Tweening;
using UnityEngine;

namespace Runtime.Extentions
{
    public class finishCollider : MonoBehaviour
    {
        public GameObject go;
        private void Start()
        {
            
            go.SetActive(false);
        }

        private void OnTriggerEnter(Collider other)
        {
            DOVirtual.DelayedCall(1.5f, () =>  go.SetActive(true));
        }
    }
}