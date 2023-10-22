using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinLoseLogic : MonoBehaviour {
    public Canvas winCanvas;

    public void Win() {
        winCanvas.gameObject.SetActive(true);
        Time.timeScale = 0;
    }
}