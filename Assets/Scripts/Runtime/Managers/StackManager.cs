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
        
      

        public bool LastCheck { get; set; }

        #endregion

        #region Seralized Veriables

        [SerializeField] private GameObject money;
        
        //[SerializeField] private List<Transform> _collectableList = new List<Transform>();
       // [SerializeField] private List<Transform> _tempList = new List<Transform>();

        #endregion

        #region Private Variables

        private StackData _data;
        private List<GameObject> _collectableStack = new List<GameObject>();

        private StackMoverCommand _stackMoverCommand;
        private ItemRemoverOnStackCommand _itemRemoverOnStackCommand;
        private StackAnimatorCommand _stackAnimatorCommand;
        private StackInteractionWithConveyorCommand _stackInteractionWithConveyorCommand;
        private StackInitializerCommand _stackInitializerCommand;
        private StackRemoverOnStackCommand _stackRemoverCommand;
        private FirstInFirstOutInStackCommand _firstInOutStack;
 
       // private StackEnterDroneAreaCommand _stackEnterDroneAreaCommand;
        
        private readonly string _stackDataPath = "Data/CD_Stack";

        #endregion

        #endregion

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
            
            //_stackEnterDroneAreaCommand = new StackEnterDroneAreaCommand(ref _collectableList, ref _tempList);

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

            //StackSignals.Instance.onFirstInFirstOutSignal += _test.Execute;
           // StackSignals.Instance.onFirstInFirstOutSignal += _firstInOutStack.Execute;
            
            
            
            // StackSignals.Instance.onMinigameState +=;
            //StackSignals.Instance.onStackEnterDroneArea += _stackEnterDroneAreaCommand.OnStackEnterDroneArea;
 
            
            CoreGameSignals.Instance.onEnterDroneArea += DroneArea;
            
            StackSignals.Instance.onLastCollectableEnterDroneArea += OnlastCollectale;
        }

        private void DroneArea()
        {
            //StartCoroutine(WaitStackObjects());
            int collectableCount = _collectableStack.Count;
            for (int i = 0; i < collectableCount; i++)
            {
                DOVirtual.DelayedCall(i,
                    () => 
               _firstInOutStack.Execute());
        
            }
     
        }
        

        private void OnlastCollectale()
        {
             
            CoreGameSignals.Instance.onExitDroneArea?.Invoke();

        }


        
        private void OnUpdateStack()
        {
            _stackInitializerCommand.Execute();
        }

        private void OnStackMove(Vector2 direction)
        {
            transform.position = new Vector3(0, gameObject.transform.position.y, direction.y - 2f);
            if (gameObject.transform.childCount > 0)
            {
                _stackMoverCommand.Execute(direction.x, _collectableStack);
            }
        }
        
        //+
        public void UpdateStack()
        {
            float stackOffset = 0f;
            for (int i = 0; i < _collectableStack.Count; i++)
            {
                Vector3 newPos = new Vector3(0f, 1f, -0.335f + stackOffset);
                _collectableStack[i].transform.localPosition = newPos;
                stackOffset += _data.CollectableOffsetInStack;
            }
        }
        
       
        private void OnInteractionWithATM(GameObject collectableGameObject)
        {
            //ScoreSignals.Instance.onSetAtmScore?.Invoke((int)collectableGameObject.GetComponent<CollectableManager>()
           //     .GetCurrentValue() + 1);
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
            DOTween.Complete(StackJumperCommand);
            AdderOnStackCommand.Execute(collectableGameObject);
            StartCoroutine(_stackAnimatorCommand.Execute());
            StackTypeUpdaterCommand.Execute();
        }

        private void OnPlay()
        {
            _stackInitializerCommand.Execute();
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
            
           // StackSignals.Instance.onFirstInFirstOutSignal -= _firstInOutStack.Execute;
            // StackSignals.Instance.onMinigameState +=;
            //StackSignals.Instance.onStackEnterDroneArea += _stackEnterDroneAreaCommand.OnStackEnterDroneArea;

            StackSignals.Instance.onLastCollectableEnterDroneArea -= OnlastCollectale;
            
            
            CoreGameSignals.Instance.onEnterDroneArea -= DroneArea;
 
        }

        private void OnDisable()
        {
            UnSubscribeEvents();
        }

        private void OnReset()
        {
            LastCheck = false;
            _collectableStack.Clear();
            _collectableStack.TrimExcess();
        }
    }
}