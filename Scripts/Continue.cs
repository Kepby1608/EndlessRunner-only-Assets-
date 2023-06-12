using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Continue : Singleton<Continue>
{
    //public GameObject panel;

    public void ContinueGame()
    {
        //panel.SetActive(false);
        Time.timeScale = 1f;
    }
}
