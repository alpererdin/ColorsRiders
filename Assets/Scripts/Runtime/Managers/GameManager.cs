using System;
using Runtime.Enums;
using Runtime.Extentions;
using Runtime.Signals;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    #region musicManager

    public MusicManager musicManager;

    #endregion
    #region singleton

    public static GameManager Instance;
 
    #endregion
    #region Self Variables

    #region Public Variables

   

    #endregion

    #endregion

    private void Start()
    {
        musicManager.Start();
        
    }

    protected override void Awake()
    {
        Instance = this;
        
        Application.targetFrameRate = 60;
    }

    private void Update()
    {
        musicManager.Update();
    }
}