using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DamageVisual : MonoBehaviour
{
    MeshRenderer meshRenderer;
    [ColorUsage(true, true)]
    public Color hitColor;

    private void Awake() {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    public void OnDamage() {
        meshRenderer.material.color = Color.white;
        meshRenderer.material.DOColor(hitColor, 0.01f).OnComplete(() => meshRenderer.material.DOColor(Color.cyan, 0.01f));
    }

    public void OnDeath() {
        meshRenderer.material.DOColor(Color.white, 0.02f);
    }
}
