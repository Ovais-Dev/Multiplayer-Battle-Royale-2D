using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;


public class CharacterHealth : NetworkBehaviour
{
   
    #region Network Variable Health, Dead
    public NetworkVariable<int> health = new NetworkVariable<int>(
        100,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Server);
    public NetworkVariable<bool> IsDead = new NetworkVariable<bool>(
    false,
    NetworkVariableReadPermission.Everyone,
    NetworkVariableWritePermission.Server);
    #endregion
    
    [Header("Health UI Setup")]
    public int maxHealth = 50;
    [SerializeField] private Slider healthSlider;
    [SerializeField] ColorController playerColorController;
    [SerializeField] ColorController enemyColorController;
    [SerializeField] private ColorFlash colFlash;
    [SerializeField] private float flashThresholdPercentage = 30f; // percentage

    #region Slider Color Flash Struct 
    [System.Serializable]
    public struct ColorController
    {
        public Color normalColor;
      public Color criticalColor;
    }
    float flashThresholdValue;
    #endregion

    #region PowerUps Setting Variables
    public GameObject shieldObj;
    bool IsInvincible = false;
    #endregion
    //private CharacterUIBinder uiBinder;
   
    #region Slider Setting Manager
   
    #region Control Health Slider Flash
    void SetHealthSliderColor(ColorController colCont)
    {
        if (!colFlash) return;
        colFlash.SetNormalColor(colCont.normalColor);
        colFlash.SetFlashColor(colCont.criticalColor);
        colFlash.SetInitialColor(); // will set normal color as initial
    }
    #endregion

    #region Slider Value Update
    public void UpdateHealthSlider(int oldValue, int newValue)
    {
        healthSlider.value = newValue;
        // healht Slider color flash contorller
        if (newValue <= flashThresholdValue)
        {
            colFlash.StartFlash();
        }
        else
        {
            colFlash.CancleFlash();
        }
        // end region
    }
    #endregion
    #endregion

    #region Initialization
    private void Start()
    {
        flashThresholdValue = maxHealth * flashThresholdPercentage * 0.01f;

    }

    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            //    if (uiBinder == null) uiBinder = GetComponent<CharacterUIBinder>();
            //    uiBinder.BindHealth(this);
            SetHealthSliderColor(playerColorController);
        }
        else
        {
            SetHealthSliderColor(enemyColorController);
        }
            IsDead.OnValueChanged += OnDeathStateChanges;
            health.OnValueChanged += UpdateHealthSlider;
            healthSlider.maxValue = maxHealth;
            healthSlider.value = maxHealth;

        if (!IsServer) return;

        health.Value = maxHealth;
        
    }
    #endregion

    #region Damage And Death Control
    [ServerRpc]
    public void DealDamageServerRpc(int damage,ulong attackerId) {
        if (IsInvincible) return;
        ApplyDamage(damage,attackerId);
    }

    public void ApplyDamage(int damage, ulong attackerId)
    {
        if (IsServer)
        {
            health.Value -= damage;
            Debug.Log("Health: " + health.Value);
            if (health.Value <= 0)
            {
                HandleDeath(attackerId);
                return;
            }
        }
        
    }
    void HandleDeath(ulong killerId)
    {
        if (!IsServer) return;
        IsDead.Value = true;
        if (NetworkManager.Singleton.ConnectedClients.TryGetValue(killerId,out var client))
        {
            var stats = client.PlayerObject.GetComponent<CharacterStats>();
            stats.KillCount.Value++;
        }
    }
    void OnDeathStateChanges(bool oldValue, bool newValue)
    {
        if (newValue)
        {
            if (IsOwner)
            {
                UIManager.Instance.ShowRespawnPanel();
            }
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
        }
    }
    #endregion
    #region Control Respawn
    [ServerRpc]
    public void RespawnServerRpc()
    {
        IsDead.Value = false;
        ApplyHeal(maxHealth);
    }
    #endregion
    #region Heal Settings
    [ServerRpc]
    public void DealHealServerRpc(int healAmt)
    {
        ApplyHeal(healAmt);
    }
    public void ApplyHeal(int healAmt)
    {
        if (IsServer)
        {
            health.Value += healAmt;
            healthSlider.value = health.Value;
            if (health.Value > maxHealth) health.Value = maxHealth;
        }
    }
    #endregion

    #region PowerUp Setting Handler
    public void SetInvincible(bool value)
    {
        shieldObj.SetActive(value);
        IsInvincible = value;
    }
    #endregion
}
