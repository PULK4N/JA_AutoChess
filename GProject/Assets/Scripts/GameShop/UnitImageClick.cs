using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitImageClick : MonoBehaviour
{
    public Image Owner;
    public Image Frame;
    public Sprite CommonFrame;
    public Sprite UncommonFrame;
    public Sprite RareFrame;
    public Sprite EpicFrame;
    public Sprite LegendaryFrame;
    public Text TextCost;
    public Text TextName;
    public Text TextSynergy;

    private GameObject DisplayedUnit;
    public GameObject PiecePanel;

    public delegate void Buy(UnitImageClick image, GameObject unit, Enums.Piece piece);
    public event Buy OnBuy;

    public void DisplayUnit(GameObject unit)
    {
        DisplayedUnit = unit;
        Sprite unitImage = Resources.Load(unit.GetComponent<Unit>().GetType().Name +
                "/pictures/" + unit.GetComponent<Unit>().GetType().Name, typeof(Sprite)) as Sprite;
        Owner.sprite = unitImage;

        TextName.text = unit.GetComponent<Unit>().GetType().Name;
        string synergies = "";
        foreach (Enums.Synergy synergy in unit.GetComponent<Unit>().Stats.Synergies)
            synergies += synergy.ToString();
        TextSynergy.text = synergies;

        TextCost.text = unit.GetComponent<Unit>().Cost.ToString();
        switch (unit.GetComponent<Unit>().Cost)
        {
            case 1:
                Frame.sprite = CommonFrame;
                break;
            case 2:
            case 3:
                Frame.sprite = UncommonFrame;
                break;
            case 4:
            case 5:
                Frame.sprite = RareFrame;
                break;
            case 6:
            case 7:
                Frame.sprite = EpicFrame;
                break;
            default:
                Frame.sprite = LegendaryFrame;
                break;
        }
    }

    public void DisableDisplay()
    {
        Owner.gameObject.SetActive(false);
        Frame.gameObject.SetActive(false);
    }

    public Enums.Piece DefaultPiece;
    public void OnClicked()
    {
        switch(DefaultPiece)
        {
            case Enums.Piece.none:
                PiecePanel.SetActive(true);
                break;
            case Enums.Piece.Pawn:
                PawnClicked();
                break;
            case Enums.Piece.Bishop:
                BishopClicked();
                break;
            case Enums.Piece.Knight:
                KnightClicked();
                break;
            case Enums.Piece.Rook:
                RookClicked();
                break;
            case Enums.Piece.Queen:
                QueenClicked();
                break;
        }
    }

    public void PawnClicked()
    {
        OnBuy(this, DisplayedUnit, Enums.Piece.Pawn);
        DisablePanel();
    }

    public void BishopClicked()
    {
        OnBuy(this, DisplayedUnit, Enums.Piece.Bishop);
        DisablePanel();
    }

    public void KnightClicked()
    {
        OnBuy(this, DisplayedUnit, Enums.Piece.Knight);
        DisablePanel();
    }

    public void RookClicked()
    {
        OnBuy(this, DisplayedUnit, Enums.Piece.Rook);
        DisablePanel();
    }

    public void QueenClicked()
    {
        OnBuy(this, DisplayedUnit, Enums.Piece.Queen);
        DisablePanel();
    }

    private void DisablePanel()
    {
        PiecePanel.SetActive(false);
    }
}
