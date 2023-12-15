using System;
using DG.Tweening;
using Runtime.Signals;
using TMPro;
using UnityEngine;

namespace Runtime.Managers
{
    public class AtmManager : MonoBehaviour
    {
        #region Self Variables

        #region Serialized Variables

        //[SerializeField] private DOTweenAnimation doTweenAnimation;
      //  [SerializeField] private TextMeshPro atmText;

        #endregion

        #endregion

        private void Awake()
        {
            GetReferences();
        }

        private void GetReferences()
        {
           // doTweenAnimation 
        }

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            CoreGameSignals.Instance.onAtmTouched += OnAtmTouched;
             
        }


        private void OnAtmTouched(GameObject touchedATMObject)
        {
            if (touchedATMObject.GetInstanceID() == gameObject.GetInstanceID())
            {
                Debug.Log("gidecek");
            }
        }

        private void OnSetAtmScoreText(int value)
        {
           // atmText.text  
        }

        private void UnSubscribeEvents()
        {
            CoreGameSignals.Instance.onAtmTouched -= OnAtmTouched;
       
        }

        private void OnDisable()
        {
            UnSubscribeEvents();
        }
    }
}