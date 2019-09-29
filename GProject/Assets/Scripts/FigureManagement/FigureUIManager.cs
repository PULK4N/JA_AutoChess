using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FigureUIManager : MonoBehaviour
{
    public Image ImgHealthBar;
    public Sprite OneStar;
    public Sprite TwoStar;
    public Sprite ThreeStar;

    public Image Star;
    private float _CurrentHealthValue;
    private float _CurrentManaValue;

    public void SetHealth(float Health)
    {
        if (Health != _CurrentHealthValue)
        {
            _CurrentHealthValue = Health;

            ImgHealthBar.fillAmount = _CurrentHealthValue / 100f;
        }
    }

    public void SetMana(float Mana)
    {
        // To-Do: implement mana visualization
    }

    public float CurrentHealth
    {
        get { return _CurrentHealthValue; }
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

    // Start is called before the first frame update
    void Start()
    {
        PromoteToOneStar();
    }
}
