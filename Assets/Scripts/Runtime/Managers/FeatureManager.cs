using Runtime.Commands.Feature;
using Runtime.Signals;
 
using UnityEngine;
using UnityEngine.Events;

namespace Runtime.Managers
{
    public class FeatureManager : MonoBehaviour
    {
        #region Self Variables

        #region Public Variables

        public FeatureManager()
        {
            
            _onClickStackCommand = new OnClickStackCommand(this, ref _newPriceTag, ref _stackLevel);
        }

        #endregion

        #region Private Variables

           
         private byte _stackLevel = 0;
         private int _newPriceTag;
          

        
        private readonly OnClickStackCommand _onClickStackCommand;

        #endregion

        #endregion

        private void Awake()
        {
         
            _stackLevel = LoadStackData();
        }

        private void OnEnable()
        {
            Subscription();
        }

        private void Subscription()
        {
             
            UISignals.Instance.onClickStack += _onClickStackCommand.Execute;
            
            CoreGameSignals.Instance.onGetStackLevel += OnGetStackLevel;

             
            ScoreSignals.Instance.onTestAciton += ontest;
        }

        private void ontest()
        {
            _stackLevel++;
        }


        private byte OnGetStackLevel() => _stackLevel;

        private void UnSubscription()
        {
             
            UISignals.Instance.onClickStack -= _onClickStackCommand.Execute;
        
            CoreGameSignals.Instance.onGetStackLevel -= OnGetStackLevel;
            ScoreSignals.Instance.onTestAciton -= ontest;
        }

        private void OnDisable()
        {
            UnSubscription();
        }
        

        private byte LoadStackData()
        {
            if (!ES3.FileExists()) return 1;
            return (byte)(ES3.KeyExists("StackLevel") ? ES3.Load<int>("StackLevel") : 1);
        }

        internal void SaveFeatureData()
        {
            SaveSignals.Instance.onSaveGameData?.Invoke();
        }
    }
}