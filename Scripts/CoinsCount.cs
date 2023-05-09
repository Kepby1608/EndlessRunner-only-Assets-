using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CoinsCount : Singleton<CoinsCount>
{
    public Text coinsCount;
    public int countCoin = 0;
    public int sumCountCoin;

    private void Update()
    {
        coinsCount.text = "Coins: " + countCoin;
    }
}
