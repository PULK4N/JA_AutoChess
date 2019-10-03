using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DPSmanager : MonoBehaviour      // subject for optimization
{
    public List<GameObject> AllyFigures;
    public List<GameObject> EnemyFigures;
    public Text DpsText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(MatchManager.Instance.MatchState==Enums.MatchState.Battle)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            foreach (GameObject figure in AllyFigures)
                sb.AppendLine(figure.GetComponent<Figure>().Unit.GetType().Name + figure.GetComponent<Figure>().DPS.ToString());
        }
    }

    void Awake()
    {
        SetupSingleton();
    }

    #region Singleton
    public static DPSmanager Instance;
    /// <summary>
    /// Creates a single instance of this object
    /// </summary>
    private void SetupSingleton()
    {
        if (DPSmanager.Instance != null && DPSmanager.Instance != this)
        {
            Destroy(this);
            return;
        }

        DPSmanager.Instance = this;
        GameObject.DontDestroyOnLoad(transform.root.gameObject);
    }
    #endregion
}
