using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHPManager : MonoBehaviour
{
    public Image ImgHealthBar;
    public Text TxtHP;

    public int Min;
    public int Max;

    private int _CurrentValue;

    public void SetHealth(int Health)
    {
        if(Health != _CurrentValue)
        {
            _CurrentValue = Health;

            TxtHP.text = string.Format("{0}%", _CurrentValue);
            ImgHealthBar.fillAmount = _CurrentValue / 100f;
        }
    }

    public int CurrentHealth
    {
        get { return _CurrentValue; }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }
}
