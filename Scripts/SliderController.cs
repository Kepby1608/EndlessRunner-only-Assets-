using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using TMPro.Examples;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class SliderController : MonoBehaviour
{
    public Slider slider;
    public TextMeshProUGUI sliderText;

    void Update()
    {
        sliderText.text = (Math.Round(slider.value * 100)).ToString();
    }
}
