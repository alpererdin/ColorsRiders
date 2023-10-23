using System;
using Runtime.Data.UnityObject;
using Runtime.Data.ValueObject;
using UnityEngine;
using Runtime.Signals;
using Extentions;
using Runtime.Enums;
using UnityEngine.Events;

namespace Runtime.Managers
{
    public class DroneAreaManager : MonoBehaviour
    {
        #region SelfVariables

        #region PrivateVariables

        private ColorData _data;
        private const string GateDataPath = "Data/CD_Color";
        public MaterialColorTypes currentColorType;
       

        private Material _material;

      

        #endregion

        #endregion
        private void Awake()
        {
            _material = GetComponent<MeshRenderer>().material;
            GetGateColorData();
            _material.color = _data.Color;
  


        }
        private void GetGateColorData() => _data = Resources.Load<CD_Color>("Data/CD_Color").Colors[(int)currentColorType];

       
    }
    
}