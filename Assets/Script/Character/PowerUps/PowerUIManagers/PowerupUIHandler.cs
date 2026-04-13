using UnityEngine;
using System.Collections.Generic;
using System.Linq;
public class PowerupUIHandler : MonoBehaviour
{
    private static PowerupUIHandler _instance;
    public static PowerupUIHandler Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindFirstObjectByType<PowerupUIHandler>();
            }
            return _instance;
        }
    }

    [SerializeField] private GameObject powerUpElementObj;
    [SerializeField] private Transform pElementContainer;

    private List<PowerUpElement> activePowerupIds = new List<PowerUpElement>();

    public void IntializePowerupUIElement(PowerupData pData)
    {
        if (CheckExistingElement(pData.id))
        {
            ReActivatePowerUIElement(pData.id);
            return;
        }
        ActivatePowerUIElement(pData);
    }
    #region UiElementSetup
    void ActivatePowerUIElement(PowerupData pData)
    {
        GameObject pElementObj = Instantiate(powerUpElementObj, pElementContainer);
        PowerUpElement pElement = pElementObj.GetComponent<PowerUpElement>();

        pElement.Initialize(pData);
        activePowerupIds.Add(pElement);
    }

    bool CheckExistingElement(int id)
    {
        return activePowerupIds.Find(ele => ele.GetPowerDataId() == id);
    }
    void ReActivatePowerUIElement(int id)
    {
        activePowerupIds.Find(ele => ele.GetPowerDataId() == id).RefillTickValue();
    }
    #endregion
    
    public void RemovePowerData(PowerUpElement pElement)
    {
        activePowerupIds.Remove(pElement);
    }
}
