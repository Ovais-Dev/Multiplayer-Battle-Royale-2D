using UnityEngine;
using UnityEngine.UI;
using System.Collections;
public class ColorFlash : MonoBehaviour
{
    [SerializeField] private Image targetImage;
    [SerializeField] private Color normalColor;
    [SerializeField] private Color flashColor;

    [SerializeField] private float transitionDuration = 1.5f;
    
    
    IEnumerator Flash()
    {
        Color col1 = normalColor;
        Color col2 = flashColor;
        float elapsedTime = 0f;
        while (true)
        {
            elapsedTime += Time.deltaTime;
            targetImage.color = Color.Lerp(col1, col2, elapsedTime / transitionDuration);
            if (elapsedTime >= transitionDuration)
            {
                elapsedTime = 0f;
                Color col3 = col1;
                col1 = col2;
                col2 = col3;
            }
            yield return null;
        }
    }
    public void StartFlash()
    {
        StartCoroutine(Flash());
    }
    public void CancleFlash()
    {
        StopCoroutine(Flash());
    }
    public void SetNormalColor(Color col)
    {
        normalColor = col;
    }
    public void SetFlashColor(Color col)
    {
        flashColor = col;
    }
    public void SetInitialColor()
    {
        targetImage.color = normalColor;
    }
}
