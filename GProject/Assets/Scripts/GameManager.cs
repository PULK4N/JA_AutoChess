using UnityEngine;

public class GameManager : MonoBehaviour
{

    void Awake()
    {
        SetupSingleton();
        InitData();
    }

    #region Singleton
    public static GameManager Instance;
    /// <summary>
    /// Creates a single instance of this object
    /// </summary>
    private void SetupSingleton()
    {
        if (GameManager.Instance != null && GameManager.Instance != this)
        {
            Destroy(this);
            return;
        }

        GameManager.Instance = this;
        GameObject.DontDestroyOnLoad(transform.root.gameObject);
    }
    #endregion

    public GameManagerEvents Events = new GameManagerEvents();
    public LevelManager Level;

    #region Data

    public SceneDataTemp DataTemp = new SceneDataTemp();
    public SceneDataSave DataSave = new SceneDataSave();
    public SceneDataSettings DataSettings = new SceneDataSettings();
    public string DataPath;

   // public GameData Gamedata;

    private void InitData()
    {
        DataPath = Application.persistentDataPath + "/";
        DataSettings = DataSettings.Load();
        DataSave = DataSave.Load();
    }

    #endregion
}