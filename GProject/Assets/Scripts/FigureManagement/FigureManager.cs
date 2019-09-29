using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FigureManager : MonoBehaviour
{
    public static GameObject CreateFigure(string name)
    {
        GameObject figureOnBoard = new GameObject();
        figureOnBoard.name = name + "Figure";

        GameObject unitPrefab = Resources.Load(name + "/" + "footman_Red", typeof(GameObject)) as GameObject;
        GameObject go = Instantiate(unitPrefab, Vector3.zero, Quaternion.identity) as GameObject;
        go.AddComponent(Type.GetType(name));
        go.transform.SetParent(figureOnBoard.transform);

        GameObject unitUI = Resources.Load("FigureUI", typeof(GameObject)) as GameObject;
        GameObject go1 = Instantiate(unitUI, Vector3.zero, Quaternion.identity) as GameObject;
        go1.transform.SetParent(figureOnBoard.transform);

        figureOnBoard.AddComponent<Figure>();
        figureOnBoard.GetComponent<Figure>().Unit = go.GetComponent<Unit>();
        figureOnBoard.GetComponent<Figure>().FigureUIManager = go1.GetComponent<FigureUIManager>();

        return figureOnBoard;
    }

    void Awake()
    {
        SetupSingleton();
    }

    #region Singleton
    public static FigureManager Instance;
    /// <summary>
    /// Creates a single instance of this object
    /// </summary>
    private void SetupSingleton()
    {
        if (FigureManager.Instance != null && FigureManager.Instance != this)
        {
            Destroy(this);
            return;
        }

        FigureManager.Instance = this;
        GameObject.DontDestroyOnLoad(transform.root.gameObject);
    }
    #endregion
}
