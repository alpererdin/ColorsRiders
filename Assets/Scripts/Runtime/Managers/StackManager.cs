using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Runtime.Commands;
using Runtime.Commands.Stack;
using Runtime.Data.UnityObject;
using Runtime.Data.ValueObject;
using Runtime.Signals;
using UnityEngine;
using UnityEngine.Events;

namespace Runtime.Managers
{
    public class StackManager : MonoBehaviour
    {
        #region Self Variables

        #region Public Variables

        public StackJumperCommand StackJumperCommand { get; private set; }

        public StackTypeUpdaterCommand StackTypeUpdaterCommand { get; private set; }

        public ItemAdderOnStackCommand AdderOnStackCommand { get; private set; }
        
      //  public ResetStackCommand resetStack { get; private set; }
        
      

        public bool LastCheck { get; set; }

        #endregion

        #region Seralized Veriables

        [SerializeField] private GameObject money;
        
         [SerializeField] public List<GameObject> _collectableList = new List<GameObject>();
        [SerializeField] public List<GameObject> _tempList = new List<GameObject>();

        [SerializeField] public List<GameObject> _wrongSideList = new List<GameObject>();
        #endregion

        #region Private Variables

        private int countlvl;

        private StackData _data;
        private List<GameObject> _collectableStack = new List<GameObject>();

        private StackMoverCommand _stackMoverCommand;
        private ItemRemoverOnStackCommand _itemRemoverOnStackCommand;
        private StackAnimatorCommand _stackAnimatorCommand;
        private StackInteractionWithConveyorCommand _stackInteractionWithConveyorCommand;
        private StackInitializerCommand _stackInitializerCommand;
        private StackRemoverOnStackCommand _stackRemoverCommand;
        private FirstInFirstOutInStackCommand _firstInOutStack;
        private StackNewInitalizerCommand _newInit;
       private StackToSizeCommand _sizeCommand;
       private SizeToBuildCommand _buildCommand;
       private SizeStackMoverCommand _sizeMoverCommand;
        private readonly string _stackDataPath = "Data/CD_Stack";
        
        private IEnumerator currentDecreaseStackCoroutine;

        #endregion

        #endregion
        
        private bool isPlayerBuilding = false;
        
        

        private void Awake()
        {
            _data = GetStackData();
            Init();
        }

        private void Init()
        {
            _stackMoverCommand = new StackMoverCommand(ref _data);
            AdderOnStackCommand = new ItemAdderOnStackCommand(this, ref _collectableStack, ref _data);
            _itemRemoverOnStackCommand = new ItemRemoverOnStackCommand(this, ref _collectableStack);
            _stackAnimatorCommand = new StackAnimatorCommand(this, _data, ref _collectableStack);
            StackJumperCommand = new StackJumperCommand(_data, ref _collectableStack);
            _stackInteractionWithConveyorCommand = new StackInteractionWithConveyorCommand(this, ref _collectableStack);
            StackTypeUpdaterCommand = new StackTypeUpdaterCommand(ref _collectableStack);
            _stackInitializerCommand = new StackInitializerCommand(this, ref money);
            _stackRemoverCommand = new StackRemoverOnStackCommand(this, ref _collectableStack);
            _firstInOutStack = new FirstInFirstOutInStackCommand(this, ref _collectableStack);
            _newInit = new StackNewInitalizerCommand(this, ref _collectableList );
            _sizeCommand=new StackToSizeCommand(this, ref _collectableStack,ref _tempList);
            _buildCommand=new SizeToBuildCommand(this, ref _collectableStack,ref _tempList);
            _sizeMoverCommand = new SizeStackMoverCommand();

        }

        private StackData GetStackData()
        {
            return Resources.Load<CD_Stack>(_stackDataPath).Data;
        }

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            StackSignals.Instance.onInteractionCollectable += OnInteractionWithCollectable;
            StackSignals.Instance.onInteractionObstacle += _stackRemoverCommand.Execute;
            StackSignals.Instance.onInteractionATM += OnInteractionWithATM;
            StackSignals.Instance.onInteractionConveyor +=
                _stackInteractionWithConveyorCommand.Execute;
            StackSignals.Instance.onStackFollowPlayer += OnStackMove;
            StackSignals.Instance.onUpdateType += StackTypeUpdaterCommand.Execute;
            CoreGameSignals.Instance.onPlay += OnPlay;
            CoreGameSignals.Instance.onReset += OnReset;
            StackSignals.Instance.onUpdateStack += OnUpdateStack;
            StackSignals.Instance.onRemoveStackObject += _itemRemoverOnStackCommand.Execute;
            CoreGameSignals.Instance.onEnterDroneArea += DroneArea;
            StackSignals.Instance.onLastCollectableEnterDroneArea += OnlastCollectale;
            StackSignals.Instance.onSetStackCount += setStack;
              StackSignals.Instance.droneareaAdder  += onAdderDroneareaObject;
             StackSignals.Instance.onPrepareBuildingStae += MiniGameArea;
             StackSignals.Instance.onSizeStackFollowPlayer += OnSizeStackMove;
             StackSignals.Instance.isPlayerBuildState += SubscribeToPlayerBuildState;
             
             StackSignals.Instance.wrongSideAdder  += onAdderWrongSideList;

        }
        private void UnSubscribeEvents()
        {
            StackSignals.Instance.onInteractionCollectable -= OnInteractionWithCollectable;
            StackSignals.Instance.onInteractionObstacle -= _stackRemoverCommand.Execute;
            StackSignals.Instance.onInteractionATM -= OnInteractionWithATM;
            StackSignals.Instance.onInteractionConveyor -=
                _stackInteractionWithConveyorCommand.Execute;
            StackSignals.Instance.onStackFollowPlayer -= OnStackMove;
            StackSignals.Instance.onUpdateType -= StackTypeUpdaterCommand.Execute;
            CoreGameSignals.Instance.onPlay -= OnPlay;
            CoreGameSignals.Instance.onReset -= OnReset;
            StackSignals.Instance.onLastCollectableEnterDroneArea -= OnlastCollectale;
            CoreGameSignals.Instance.onEnterDroneArea -= DroneArea;
            StackSignals.Instance.onUpdateStack -= OnUpdateStack;
            StackSignals.Instance.onRemoveStackObject -= _itemRemoverOnStackCommand.Execute;
            StackSignals.Instance.onSetStackCount -= setStack;
            StackSignals.Instance.droneareaAdder  -= onAdderDroneareaObject;
            StackSignals.Instance.onPrepareBuildingStae -= MiniGameArea;
            StackSignals.Instance.onSizeStackFollowPlayer -= OnSizeStackMove;
            StackSignals.Instance.isPlayerBuildState -= SubscribeToPlayerBuildState;
            
            StackSignals.Instance.wrongSideAdder  -= onAdderWrongSideList;
        
 
        }

         

        #region building102 commandlines
/*  private void SubscribeToPlayerBuildState(bool isBuilding,Transform target)
        {
            if (isBuilding)
            {
                
                isPlayerBuilding = true;
                build101(target);
                
            }
            else
            {
                
                isPlayerBuilding = false;
                StopCoroutine(DecreaseStackListCount(target));
            }
        }
       

        private void build101(Transform target)
        {
            if (target != null)
            {
                StartCoroutine(DecreaseStackListCount(target));
                
                 

            }else
                Debug.Log("transform bost");
        
        }
        
        private IEnumerator DecreaseStackListCount(Transform target)
        {
             
                 
                int temp = _tempList.Count;
                if (temp > 0)
                {
                   // _tempList.RemoveAt(temp - 1);
                    _buildCommand.Execute(target);
                }

                 
                yield return new WaitForSeconds(1f);
                if (isPlayerBuilding)
                {
                    StartCoroutine(DecreaseStackListCount(target));
                }
             
        }
        */

        #endregion

        private void stackCheck()
        {
            if (_collectableStack.Count==0)
            {
                
            }
        }

        private void SubscribeToPlayerBuildState(bool isBuilding,Transform target)
        {
            if (isBuilding)
            {
                isPlayerBuilding = true;
                if (currentDecreaseStackCoroutine != null)
                {
                    StopCoroutine(currentDecreaseStackCoroutine);
                }
                currentDecreaseStackCoroutine = DecreaseStackListCount(target);
                StartCoroutine(currentDecreaseStackCoroutine);
            }
            else
            {
                isPlayerBuilding = false;
            }
        }
        private IEnumerator DecreaseStackListCount(Transform target)
        {
            yield return new WaitForSeconds(1f);
            float initialWaitTime = 1f;
            float minWaitTime = 0.1f;
            float waitTimeDecrement = 0.1f;
    
            while (isPlayerBuilding)
            {
                int temp = _tempList.Count;
                if (temp > 0)
                { 
                    _buildCommand.Execute(target);
                }

                
                float waitTime = Mathf.Max(initialWaitTime - temp * waitTimeDecrement, minWaitTime);
                yield return new WaitForSeconds(waitTime);
            }
        }

   
       /* private IEnumerator DecreaseStackListCount(Transform target)
        {
            yield return new WaitForSeconds(1f);
            while (isPlayerBuilding)
            {
                int temp = _tempList.Count;
                if (temp > 0)
                { 
                    _buildCommand.Execute(target);
                }

                yield return new WaitForSeconds(.5f);
            }
        }*/
        private void OnSizeStackMove(Vector3 direction)
        {
            transform.position = new Vector3(0, 0, 0);
            if (_tempList.Count > 0)
            {
                _sizeMoverCommand.Execute(direction.x,direction.y,direction.z, _tempList);
                
            }
        }
        private void onJumperArea()
        {
            StartCoroutine(_stackAnimatorCommand.Execute());

        }
        private void onAdderDroneareaObject(GameObject collectableGameObject)
        {
            _collectableList.Add(collectableGameObject);
            
        }
        private int setStack() => countlvl;
        private void DroneArea()
        {
            int collectableCount = _collectableStack.Count;
 
            for (int i = 0; i < collectableCount; i++)
            {
                DOVirtual.DelayedCall((i+1)/4f,
                    () => 
               _firstInOutStack.Execute());
            }
        }
        private void MiniGameArea()
        {
            _collectableStack[0].SetActive(false);
            int collectableCount = _collectableStack.Count;
 
            for (int i = 0; i < collectableCount; i++)
            {
                 
                DOVirtual.DelayedCall((i+1)/5f,
                    () => 
                        _sizeCommand.Execute());
                
            }
        }
        private void onAdderWrongSideList(GameObject collectableGameObject)
        {
            _wrongSideList.Add(collectableGameObject);
            
        }
     
        private void OnlastCollectale()
        {
            
            CoreGameSignals.Instance.onPlayDroneAnim?.Invoke(_wrongSideList);
            //CoreGameSignals.Instance.onExitDroneArea?.Invoke();

        }
    
        private void OnUpdateStack( )
        { 
            _newInit.Execute();
        }

        private void OnStackMove(Vector3 direction)
        {
 
            transform.position = new Vector3(0, 0, direction.z - 2f);
            if (gameObject.transform.childCount > 0)
            {
                _stackMoverCommand.Execute(direction.x,direction.y, _collectableStack);
              
            }
           
        }
       
  
        public void UpdateStack()
        {
            float stackOffset = 0f;
            for (int i = 0; i < _collectableStack.Count; i++)
            {
                Vector3 newPos = new Vector3(_collectableStack[i].transform.position.x, _collectableStack[i].transform.position.y, -1.5f + stackOffset);
              _collectableStack[i].transform.localPosition = newPos;
                stackOffset += _data.CollectableOffsetInStack;
            }
        }
        
        private void OnInteractionWithATM(GameObject collectableGameObject)
        {
            if (LastCheck == false)
            {
                _itemRemoverOnStackCommand.Execute(collectableGameObject);
            }
            else
            {
                collectableGameObject.SetActive(false);
            }
        }
        
        private void OnInteractionWithCollectable(GameObject collectableGameObject)
        {
            //DOTween.Complete(StackJumperCommand);
            AdderOnStackCommand.Execute(collectableGameObject);
            StartCoroutine(_stackAnimatorCommand.Execute());
            StackTypeUpdaterCommand.Execute();
        }

        private void OnPlay()
        {
           // _stackInitializerCommand.Execute();
        }
        
        private void OnDisable()
        {
            UnSubscribeEvents();
        }

        
        private void DestroyChildObj()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                Destroy(transform.GetChild(i).gameObject);
            }
        }

        private void OnReset()
        {
            isPlayerBuilding = false;
            LastCheck = false;
            _collectableStack.Clear();
            _collectableList.Clear();
            _collectableList.TrimExcess();
            _collectableStack.TrimExcess();
            _wrongSideList.Clear();
            _wrongSideList.TrimExcess();
            _tempList.Clear();
            _tempList.TrimExcess();
            DestroyChildObj();
        }
    }
}