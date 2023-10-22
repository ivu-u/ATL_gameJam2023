using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class UIHooks : MonoBehaviour
{
    [SerializeField] TMP_Text healthText = default;
    [SerializeField] Image healthBar = default;
    [SerializeField] Image healthBarEffect = default;
    [SerializeField] Image fadeImage;

    public void SetHealth(int current, int total) {
        healthText.text = $"{current}/{total}";
        healthBar.fillAmount = current / (float)total;
        healthBarEffect.fillAmount = current / (float)(total);
    }

    public void ReloadScene() {
        fadeImage.DOFade(1, 3).OnComplete(() => SceneManager.LoadSceneAsync("something"));  // change later
    }
}
