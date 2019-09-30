using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;


public class PlayerNameManager : NetworkBehaviour
{
    [SerializeField]
    GameObject displayData;

    [SerializeField]
    GameObject displayDataChild;
    private void Start()
    {
        RpcChangeName(PlayerNames.Instance.PlayerNamesString[0], "DisplayData");

        for (int i = 1; i < PlayerNames.Instance.PlayerNamesString.Count; i++)
        {
            RpcChangeName(PlayerNames.Instance.PlayerNamesString[i], "DisplayData" + i);
        }
    }
    [ClientRpc]
    public void RpcChangeName(string name, string DisplayData)
    {
            Debug.Log("Now \"" + DisplayData + "\" should be changed");
            displayData = GameObject.Find(DisplayData); 
            displayDataChild = displayData.transform.Find("Name").gameObject;
            Text text = displayDataChild.GetComponent<Text>();
            text.text = name;
    }

}

