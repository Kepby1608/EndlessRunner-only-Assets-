using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : Singleton<Pause>
{
    public void PauseGame()
    {
        //PlayerController.Instance.musicSource.Pause();
        Time.timeScale = 0;
    }

    public void ContinueGame()
    {
        //PlayerController.Instance.musicSource.UnPause();
        Time.timeScale = 1f;
    }
}
