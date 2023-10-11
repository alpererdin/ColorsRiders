using Runtime.Data.ValueObject;
using UnityEngine;
using System.Collections.Generic;
namespace Runtime.Data.UnityObject
{
    [CreateAssetMenu(fileName = "CD_Build", menuName = "ColorsRunners/CD_Build", order = 0)]
    public class CD_Build : ScriptableObject
    {
         
        public  List<BuildingData>  data;
    }
    
}