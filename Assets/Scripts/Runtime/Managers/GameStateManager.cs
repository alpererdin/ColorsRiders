using UnityEngine;

namespace Runtime.Managers
{
    public class GameStateManager : MonoBehaviour
    {
        public enum GameState
        {
            Idle,
            Runner,
         
        }

        public static GameState CurrentGameState { get; private set; }

        public static void SetGameState(GameState newState)
        {
            CurrentGameState = newState;
        }
    }
}