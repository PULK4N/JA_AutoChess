using System.Collections;
using System.Collections.Generic;
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
    public Image Spell;

    private float _currentHealthValue;
    private float _currentManaValue;
    private FigureSell _figureSell;
    private FigurePieceToggle _figurePieceToggle;

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

    public void SetPieceToggleText(Piece piece)
    {
        // To-Do: for each piece write default description
        //string tooltip;
        //PieceToggle.GetComponent<FigureTooltip>().SetTooltip(tooltip);
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
