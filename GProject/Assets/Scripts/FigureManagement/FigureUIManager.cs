using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class FigureUIManager : MonoBehaviour
{
    public Image ImgHealthBar;
    public Image ImgManaBar;
    public Image Star;
    public Sprite OneStar;
    public Sprite TwoStar;
    public Sprite ThreeStar;
    public GameObject Tooltip;
    public Button SellButton;
    public Toggle PieceToggle;
    public Sprite ImgPawn;
    public Sprite ImgBishop;
    public Sprite ImgKnight;
    public Sprite ImgRook;
    public Sprite ImgQueen;
    public Image Spell;

    private float _currentHealthValue;
    private float _currentManaValue;
    private FigureSell _figureSell;
    private FigurePieceToggle _figurePieceToggle;

    public void CarryEnemyColors()
    {
        ImgHealthBar.color = Color.red;
    }

    public void SetHealth(float Health)
    {
        if (Health != _currentHealthValue)
        {
            _currentHealthValue = Health;

            ImgHealthBar.fillAmount = _currentHealthValue / 100f;
        }
    }

    public void SetMana(float Mana)
    {
        if (Mana != _currentManaValue)
        {
            _currentManaValue = Mana;

            ImgManaBar.fillAmount = _currentHealthValue / 100f;
        }
    }

    public float CurrentHealth
    {
        get { return _currentHealthValue; }
    }

    public float CurrentMana
    {
        get { return _currentManaValue; }
    }

    public void PromoteToOneStar()
    {
        Star.sprite = OneStar;
    }

    public void PromoteToTwoStar()
    {
        Star.sprite = TwoStar;
    }

    public void PromoteToThreeStar()
    {
        Star.sprite = ThreeStar;
    }

    public void SetSpellImage(Unit unit)
    {
        Spell.sprite = Resources.Load(unit.GetType().Name + "/pictures/SpellImage", typeof(Sprite)) as Sprite;
    }

    public void SetSpellTooltip(string text)
    {
        Spell.GetComponent<FigureTooltip>().SetTooltip(text);
    }

    private bool _isPawn;
    public void SetPieceToggleText(Enums.Piece piece)
    {
        StringBuilder tooltip = new StringBuilder();
        switch(piece)
        {
            case Enums.Piece.Pawn:
                ToggleDisable();
                _isPawn = true;
                tooltip.Append("Pawn");
                tooltip.AppendLine("No bonuses");
                PieceToggle.image.sprite = ImgPawn;
                break;
            case Enums.Piece.Bishop:
                tooltip.Append("Bishop");
                tooltip.AppendLine("OFF: Start battle with 100% mana");
                tooltip.AppendLine("ON: Get 2X mana");
                PieceToggle.image.sprite = ImgBishop;
                break;
            case Enums.Piece.Knight:
                tooltip.Append("Knight");
                tooltip.AppendLine("Passive: get bonus attack speed");
                tooltip.AppendLine("OFF: Transcend grand distances quickly");
                tooltip.AppendLine("ON: Double the Range");
                PieceToggle.image.sprite = ImgKnight;
                break;
            case Enums.Piece.Rook:
                tooltip.Append("Rook");
                tooltip.AppendLine("Passive: get bonus attack damage");
                tooltip.AppendLine("OFF: Bonus armor, magic resist and attack damage");
                tooltip.AppendLine("ON: Higher bonuses but unable to cast abilities");
                PieceToggle.image.sprite = ImgRook;
                break;
            case Enums.Piece.Queen:
                tooltip.Append("Queen");
                tooltip.AppendLine("OFF: Bonus on all stats");
                tooltip.AppendLine("ON: On start of the battle get shield equal to 100% of HP");
                PieceToggle.image.sprite = ImgQueen;
                break;
        }
        PieceToggle.GetComponent<FigureTooltip>().SetTooltip(tooltip.ToString());
    }

    public void ToggleDisable()
    {
        PieceToggle.interactable = false;
    }

    public void EnableToggle()
    {
        if (!_isPawn)
            PieceToggle.interactable = true;
    }

    public void ShowTooltip()
    {
        Tooltip.SetActive(true);
    }

    public void HideTooltip()
    {
        Tooltip.SetActive(false);
    }

    public delegate void PieceToggleClick(bool toggle);
    public event PieceToggleClick OnPieceToggleClick;
    public delegate void FigureSellClick();
    public event FigureSellClick OnFigureSellClick;
    // Start is called before the first frame update
    void Start()
    {
        PromoteToOneStar();
        _figurePieceToggle = PieceToggle.GetComponent<FigurePieceToggle>();
        _figureSell = SellButton.GetComponent<FigureSell>();
        _figurePieceToggle.OnPieceToggleClick += () => OnPieceToggleClick(PieceToggle.isOn);
        _figureSell.OnSellClick += () => OnFigureSellClick();
    }
}
