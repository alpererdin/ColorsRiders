using System;
using System.Collections.Generic;
using Runtime.Enums;
using UnityEngine;

namespace Runtime.Data.ValueObject
{
    [Serializable]
    public class BuildingData
    {
        public BuildingTypes BuildingType;
        public float BuildScore;
        public float CurrentScore;
    }
 
}