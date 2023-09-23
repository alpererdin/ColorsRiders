using System;
using Runtime.Enums;
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
        [SerializeField] public Material RunnersColorMaterial;

        
        
        #endregion

        #endregion

        internal void SetTotalScore(int value)
        {
            scoreText.text = value.ToString();
        }
       /* internal void ChangeMaterial()
        {
            RunnersColorMaterial.SetColor("_Color", Color.red);
            Debug.Log("degistimi");
        }*/

      /*  private void OnEnable()
        {
            PlayerSignals.Instance.OnGatePassed +=ChangeMaterial;
        }
         
        
        private void ChangeMaterial(MaterialColorTypes colorType)
        {
            // Debug.Log(colorType);


        }
        */
        public void ChangeMaterialColor(Color color)
        {
            RunnersColorMaterial.color = color;
        }

         

       /* private void OnDisable()
        {
            PlayerSignals.Instance.OnGatePassed -=ChangeMaterial;
        }
      */
        
    }
}