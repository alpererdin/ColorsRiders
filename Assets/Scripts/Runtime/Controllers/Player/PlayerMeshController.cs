using System;
using Runtime.Signals;
using TMPro;
using UnityEngine;

namespace Runtime.Controllers.Player
{
    public class PlayerMeshController : MonoBehaviour
    {
        #region Self Variables

        #region Serialized Variables

        [SerializeField] private TextMeshPro scoreText;
        [SerializeField] private Material RunnersColorMaterial;

      

        #endregion

        #endregion

        internal void SetTotalScore(int value)
        {
            scoreText.text = value.ToString();
        }
        internal void ChangeMaterial()
        {
            RunnersColorMaterial.SetColor("_Color", Color.red);
            Debug.Log("degistimi");
        }

        private void OnEnable()
        {
            PlayerSignals.Instance.OnGatePassed +=ChangeMaterial;
        }

        private void OnDisable()
        {
            PlayerSignals.Instance.OnGatePassed -=ChangeMaterial;
        }
      
        
    }
}