using UnityEngine;
using Unity.Netcode;
public class CharacterColorSetup : NetworkBehaviour
{
    [Header("Colors Setup")]
    public SpriteRenderer spriteRenderer;
    public Color localPlayerColor;
    public Color localEnemyColor;

    //public NetworkVariable<Color> playerColor = new NetworkVariable<Color>(
    //    Color.white,
    //    NetworkVariableReadPermission.Everyone,
    //    NetworkVariableWritePermission.Server);

    
    public override void OnNetworkSpawn()
    {
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        }
            if (IsOwner)
            {
                //playerColor.Value = localPlayerColor;
                ApplyColor(localPlayerColor);
            }
            else
            {
                // playerColor.Value = localEnemyColor;
                ApplyColor(localEnemyColor);
            }
        
        
        //playerColor.OnValueChanged += OnColorChanged;
        //ApplyColor(playerColor.Value);
    }
    void OnColorChanged(Color oldColor, Color newColor)
    {
        ApplyColor(newColor);
    }
    public void ApplyColor(Color col)
    {
        spriteRenderer.color = col;
    }
}
