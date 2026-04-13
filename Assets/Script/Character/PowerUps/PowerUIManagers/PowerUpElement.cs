using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
public class PowerUpElement : MonoBehaviour
{
    private PowerupData pData;
    [Header("UI ")]
    [SerializeField] private Slider tickSlider;

    public Action<PowerUpElement> OnDestroy;

    float timer;
    public void Initialize(PowerupData data)
    {
        pData = data;
        SetupInitialization();
        StartCoroutine(PowerTickCoroutine());
        OnDestroy += PowerupUIHandler.Instance.RemovePowerData;
    }
    
    private void SetupInitialization()
    {
        tickSlider.maxValue = pData.duration;
        tickSlider.value = pData.duration;
        timer = pData.duration;
        // add icons 
    }
    public int GetPowerDataId() => pData.id;
    public void RefillTickValue()
    {
        timer = pData.duration;
    }
    IEnumerator PowerTickCoroutine()
    {
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            tickSlider.value = timer;
            yield return null;
        }
        OnDestroy?.Invoke(this);
        yield return new WaitForSeconds(0.1f);
        Deactivate();
    }
    public void Deactivate()
    {
        OnDestroy -= PowerupUIHandler.Instance.RemovePowerData;
        Destroy(gameObject);
    }
    
}
