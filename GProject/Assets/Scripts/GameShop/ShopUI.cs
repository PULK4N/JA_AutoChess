using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopUI : MonoBehaviour
{
    public List<Image> Images = new List<Image>();
    public GameObject DefaultPiece;
    private DefaultPieceUI _defaultPieceUI;
    public Button BtnLock;
    public Sprite Locked;
    public Sprite Unlocked;
    public void Start()
    {
        foreach (Image image in Images)
            image.GetComponent<UnitImageClick>().OnBuy += BuyUnit;
        MatchManager.Instance.OnStateChage += state => {
            if (state == Enums.MatchState.Preparation && UnitShop.ShopLocked)
                ToggleLock(); };
        BtnLock.image.sprite = Unlocked;
        _defaultPieceUI = DefaultPiece.GetComponent<DefaultPieceUI>();
        _defaultPieceUI.OnDefaultPieceClick += (defaultPiece) =>
        {
            foreach (Image image in Images)
                image.GetComponent<UnitImageClick>().DefaultPiece = defaultPiece;
        };
    }

    public void PlaceNewUnits(List<GameObject> units)
    {
        for (int i = 0; i < units.Count; ++i)
        {
            GameObject unit = units[i];
            Images[i].GetComponent<UnitImageClick>().DisplayUnit(unit);
            Images[i].gameObject.SetActive(true);
        }
    }

    public void BuyUnit(UnitImageClick image, GameObject unit, Enums.Piece piece)
    {
        if (!UnitShop.BuyUnit(unit, piece))
            return;
        image.DisableDisplay();
    }

    public void Reroll()
    {
        UnitShop.Reroll();
        if (UnitShop.ShopLocked)
            ToggleLock();
    }

    public void ToggleLock()
    {
        UnitShop.ShopLocked = !UnitShop.ShopLocked;
        BtnLock.image.sprite = UnitShop.ShopLocked ? Locked : Unlocked;
    }
}
