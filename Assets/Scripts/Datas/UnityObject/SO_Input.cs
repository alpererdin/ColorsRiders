using Datas.ValueObject;
using UnityEngine;

namespace Datas.UnityObject
{
    [CreateAssetMenu(fileName = "SO_Input", menuName = "Input/SO_Input", order = 0)]
    public class SO_Input : ScriptableObject
    {
        public InputData InputData;

    }
}