using System;
using UnityEngine;
using MK.Toon;
using DG.Tweening;
using Runtime.Data.UnityObject;
using Runtime.Data.ValueObject;
using Runtime.Enums;
using Runtime.Signals;
using TMPro;
namespace Runtime.Managers
{
    public class BuildingManager : MonoBehaviour
    {
        #region Variables

        
        public float buildScore;
        private float currentScore;
        public BuildingTypes BuildingTypes;
        public Renderer renderer;
        private Material testMaterial;
        private float increAseamount;
        public Collider selfCollider;
        private float totalAmount;
        public TextMeshPro scoreTxt;
        private BuildingData data;
        public GameObject SizeUpPlane;
        #endregion
        private void Awake()
        {
            GetBuildType();
        }
        private void GetBuildType()
        {
            data = Resources.Load<CD_Build>("Data/CD_Build").data[(int)BuildingTypes];
            currentScore = data.CurrentScore;
            buildScore = data.BuildScore;
            increAseamount = 1f/ buildScore;
            totalAmount = increAseamount * currentScore;
            
        }
        private void Start()
        {
            if (currentScore==buildScore)
            {
                selfCollider.enabled = false;
            }
            scoreTxt.text =  buildScore +"/"+ currentScore;
            testMaterial = renderer.material;
            testMaterial.DOFloat(totalAmount,"_Saturation", 0f);
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Collected")&& currentScore <=buildScore)
            {
                
                if (currentScore == buildScore-1)
                {
                    StackSignals.Instance.isPlayerBuildState(false, transform);
                    selfCollider.enabled = false;
                }
                currentScore++;
                data.CurrentScore++;
                saturation101(increAseamount);
                scoreTxt.text =  buildScore +"/"+ currentScore;
            }

            if (other.CompareTag("Player"))
            {
                SizeUpPlane.transform.DOScaleX(6.5f, 1);
                SizeUpPlane.transform.DOScaleZ(7f, 1);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                SizeUpPlane.transform.DOScaleX(5.53f, 1);
                SizeUpPlane.transform.DOScaleZ(6.18f, 1);
            }
        }

        private void saturation101(float increaseAmount)
        {
            totalAmount += increaseAmount;
            testMaterial.DOFloat(totalAmount,"_Saturation", .5f);
        }
    }
}