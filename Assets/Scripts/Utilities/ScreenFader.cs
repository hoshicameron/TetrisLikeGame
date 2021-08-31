using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(MaskableGraphic))]
public class ScreenFader : MonoBehaviour
{
    [SerializeField] private float startAlpha = 1f;
    [SerializeField] private float targetAlpha = 0f;
    [SerializeField] private float delay = 0f;
    [SerializeField] private float timeToFade = 0f;

    private float increment;
    private float currentAlpha;
    private MaskableGraphic maskableGraphic;
    private Color originalColor;

    private void Start()
    {
        maskableGraphic = GetComponent<MaskableGraphic>();
        originalColor = maskableGraphic.color;
        currentAlpha = startAlpha;

        Color tempColor=new Color(originalColor.r,originalColor.g,originalColor.b,originalColor.a);

        maskableGraphic.color = tempColor;

        increment = ((targetAlpha - startAlpha) / timeToFade) * Time.deltaTime;

        StartCoroutine(nameof(FadeRoutine));

    }

    IEnumerator FadeRoutine()
    {
        yield return new WaitForSeconds(delay);

        while (Mathf.Abs(targetAlpha-currentAlpha)>0.01f)
        {
            yield return new WaitForEndOfFrame();
            currentAlpha += increment;

            Color tempColor=new Color(originalColor.r,originalColor.g,originalColor.b,currentAlpha);

            maskableGraphic.color = tempColor;
        }

        print("Screen Fader Finished!");
    }
}
