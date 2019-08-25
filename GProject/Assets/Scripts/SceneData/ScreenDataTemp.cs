using UnityEngine;
/// <summary>
/// Temporary settings only valid for this one session
/// </summary>
public class SceneDataTemp : MonoBehaviour{
    private Enums.GameStates _gameState = Enums.GameStates.Menu;
    public Enums.GameStates GameState
    {
        get { return _gameState; }
        set
        {
            _gameState = value;
            GameManager.Instance.Events.InvokeGameModeChanged(_gameState);
        }
    }

    public Enums.GameTypes GameType = Enums.GameTypes.EightVsEight;
}
