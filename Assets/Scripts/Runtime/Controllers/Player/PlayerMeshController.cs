using System;
using Runtime.Data.ValueObject;
using Runtime.Enums;
using Runtime.Signals;
using TMPro;
using UnityEngine;
using DG.Tweening;
namespace Runtime.Controllers.Player
{
    public class PlayerMeshController : MonoBehaviour
    {
        #region Self Variables

        #region Serialized Variables

        [SerializeField] private TextMeshPro scoreText;
        [SerializeField] public Material RunnersColorMaterial;

        [SerializeField] private  GameObject renderer;
        
        #endregion

        private PlayerMeshData _data;
        #endregion

        internal void SetData(PlayerMeshData scaleData)
        {
            _data = scaleData;
        }
        internal void SetTotalScore(int value)
        {
            scoreText.text = value.ToString();
        }
        internal void ScaleUpPlayer()
        {
            renderer.transform.DOScale(_data.ScaleCounter, 1).SetEase(Ease.Flash);
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
       internal void OnReset()
       {
           renderer.gameObject.transform.DOScaleX(1, 1).SetEase(Ease.Linear);
       }
        
    }
}