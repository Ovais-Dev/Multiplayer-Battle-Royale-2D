using UnityEngine;
using TMPro;
public static class PlayerData
{
    public static string PlayerName;
}
public class UIManager : MonoBehaviour
{
    private static UIManager _instance;
    public static UIManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindFirstObjectByType<UIManager>();
            }
            return _instance;
        }
    }

    [Header("Health System")]
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private RespawnManager respawnManager;
    [Header("Kill System")]
    [SerializeField] private TMP_Text killText;

    [Header("Name System")]
    public TMP_InputField nameInputField;

    public void SetHealth(int val)
    {
        healthText.text = "Health: " + val;
    }
    public void SetKill(int val)
    {
        killText.text = "Kill: " + val;
    }
    public void ShowRespawnPanel()
    {
        respawnManager.RespawnOpen();
    }
    public void OnNameChanged()
    {
        PlayerData.PlayerName = nameInputField.text;
    }
}
