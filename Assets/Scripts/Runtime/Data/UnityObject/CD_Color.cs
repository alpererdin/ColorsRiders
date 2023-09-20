using UnityEngine;
using System.Collections.Generic;
using Runtime.Data.ValueObject;

namespace Runtime.Data.UnityObject
{
    [CreateAssetMenu(fileName = "CD_Color", menuName = "ColorsRunners/CD_Color", order = 0)]
    public class CD_Color : ScriptableObject
    {
        public List<ColorData> Colors;
    }
}