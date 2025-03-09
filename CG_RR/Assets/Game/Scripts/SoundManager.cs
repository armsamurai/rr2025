using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    [SerializeField] Sprite soundOn;
    [SerializeField] Sprite soundOff;

    [SerializeField] Image icon;

    bool isOn = true;

    public void SwitchSound()
    {
        if (isOn)
        {
            isOn = false;
            icon.sprite = soundOff;
            AudioListener.volume = 0;
        }
        else
        {
            isOn = true;
            icon.sprite = soundOn;
            AudioListener.volume = 1;
        }
    }
}
