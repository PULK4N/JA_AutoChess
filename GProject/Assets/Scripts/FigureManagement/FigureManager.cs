using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FigureManager : MonoBehaviour
{
    public static GameObject CreateFigure(GameObject unit)
    {
        GameObject figureOnBoard = new GameObject();
        figureOnBoard.name = unit.GetComponent<Unit>().GetType().Name + "Figure";

        unit.SetActive(true);

        unit.transform.SetParent(figureOnBoard.transform);

        GameObject unitUI = Resources.Load("FigureUI", typeof(GameObject)) as GameObject;
        GameObject UI = Instantiate(unitUI, Vector3.zero, Quaternion.identity) as GameObject;
        UI.transform.SetParent(figureOnBoard.transform);

        figureOnBoard.AddComponent<Figure>();
        figureOnBoard.GetComponent<Figure>().Unit = unit.GetComponent<Unit>();
        figureOnBoard.GetComponent<Figure>().FigureUIManager = UI.GetComponent<FigureUIManager>();

        return figureOnBoard;
    }

    public static void Disassemble(GameObject figure)
    {
        GameObject unit = figure.GetComponent<Figure>().Unit.gameObject;
        figure.transform.DetachChildren();
        unit.transform.SetParent(null);
        figure.GetComponent<Figure>().Unit = null;
        Destroy(figure);
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
