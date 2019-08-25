using UnityEngine;

public class GameManagerEvents : MonoBehaviour
{
    public delegate void GameModeChanged(Enums.GameStates state);
    public event GameModeChanged DoGameModeChanged;

    public void InvokeGameModeChanged(Enums.GameStates state)
    {
        if(DoGameModeChanged != null)
        {
            DoGameModeChanged(state);
        }
    }
}
