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
        private float size = 1.1f;
        
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
            size += 0.1f;
            renderer.transform.DOScale(size, 1).SetEase(Ease.Flash);
             
        }
        internal void ScaleDownPlayer()
        {
            size -= 0.1f;
            renderer.transform.DOScale(size, 1).SetEase(Ease.Flash);
             
        }
      
        public void ChangeMaterialColor(Color color)
        {
            RunnersColorMaterial.color = color;
        }
 
       internal void OnReset()
       {
           renderer.gameObject.transform.DOScaleX(1, 1).SetEase(Ease.Linear);
       }
        
    }
}