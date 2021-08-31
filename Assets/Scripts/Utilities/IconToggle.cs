using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class IconToggle : MonoBehaviour
{
    [SerializeField] private Sprite iconTrue;
    [SerializeField] private Sprite iconFalse;

    [SerializeField] private bool defaultIconState = true;

    private Image image;


    private void Start()
    {
        image = GetComponent<Image>();
        image.sprite = defaultIconState ? iconTrue : iconFalse;
    }

    public void Toggle(bool state)
    {
        if (image == null || iconTrue == null || iconFalse == null)
        {
            Debug.LogWarning("Warning! missing icon toggle values");
            return;
        }

        image.sprite = state ? iconTrue : iconFalse;
    }
}
