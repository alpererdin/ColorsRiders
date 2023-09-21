using Runtime.Data.UnityObject;
using Runtime.Data.ValueObject;
using UnityEngine;
using Runtime.Signals;
using Extentions;
namespace Runtime.Managers
{
    public class GateManager : MonoBehaviour
    {
        #region SelfVariables

        #region PrivateVariables

        private ColorData _data;
        private const string GateDataPath = "Data/CD_Color";

        #endregion

        #endregion
        private void Awake()
        {
            _data = GetGateData();
            
        }
        private ColorData GetGateData() => Resources.Load<CD_Color>(GateDataPath).Colors[0];
        
        private void GatePassed()
        { 
            PlayerSignals.Instance.OnGatePassed.Invoke();
        }
    }
    
}