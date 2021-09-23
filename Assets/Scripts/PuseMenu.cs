using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuseMenu : MonoBehaviour
{
    [SerializeField]private GameObject _pauseMenu;

    public void Resume()
    {
        _pauseMenu.SetActive(false);
        Time.timeScale = 1f;
    }

    public void Pause()
    {
        _pauseMenu.SetActive(true);
        Time.timeScale = 0f;
    }
}
