using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameFlow : MonoBehaviour
{
    public GameObject canvas_gameover;
    private void Start()
    {
        canvas_gameover.SetActive(false);
    }
    public void OnPlayerDied()
    {

        Time.timeScale = 0;
        print("Player died");
        canvas_gameover.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}
