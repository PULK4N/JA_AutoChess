using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FigureTooltip : MonoBehaviour
{
    public Text Text;
    public void ShowTooltip(GameObject tooltip)
    {
        tooltip.SetActive(true);
    }

    public void HideTooltip(GameObject tooltip)
    {
        tooltip.SetActive(false);
    }

    public void SetTooltip(string text)
    {
        Text.text = text;
    }

}
