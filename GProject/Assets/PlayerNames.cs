using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public sealed class PlayerNames : MonoBehaviour
{

    private void Awake()
    {
        SetupSingleton();
    }

    #region Singleton
    public static PlayerNames Instance;
    /// <summary>
    /// Creates a single instance of this object
    /// </summary>
    private void SetupSingleton()
    {
        if (PlayerNames.Instance != null && PlayerNames.Instance != this)
        {
            Destroy(this);
            return;
        }

        PlayerNames.Instance = this;
    }
    #endregion

    public List<string> PlayerNamesString = new List<string>();

    public void AddToPlayerNameString(string name)
    {
        PlayerNamesString.Add(name);
    }
}
