using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [Serializable]
    public struct Level
    {
        public Enums.Levels LevelType;
        public int LevelID;
    }

    [SerializeField]
    public List<Level> Levels = new List<Level>();

    public void LoadLevel(Enums.Levels levelType)
    {
        if (Levels.Any(x => x.LevelType == levelType))
        {
            var level = Levels.FirstOrDefault(x => x.LevelType == levelType);
            SceneManager.LoadScene(level.LevelID);
        }
        else
        {
            Debug.LogWarning("Level could not be found" + levelType.ToString());
        }
    }
}
