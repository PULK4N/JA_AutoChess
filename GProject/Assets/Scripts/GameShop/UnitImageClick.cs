using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitImageClick : MonoBehaviour
{
    public void OnClicked(Image image)
    {
        if (!UnitShop.BuyUnit(image.sprite.name))
            return;
        image.enabled = false;
    }
}
