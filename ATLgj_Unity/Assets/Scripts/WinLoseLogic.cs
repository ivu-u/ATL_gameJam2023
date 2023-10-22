using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinLoseLogic : MonoBehaviour {
    public Canvas winCanvas;
    public int numEnemies = 8;

    public void Win() {
        winCanvas.gameObject.SetActive(true);
        Time.timeScale = 0;
    }

    public void updateNumEnemies() {
        numEnemies--;

        if (numEnemies <= 0) {
            Win();
        }
    }
}