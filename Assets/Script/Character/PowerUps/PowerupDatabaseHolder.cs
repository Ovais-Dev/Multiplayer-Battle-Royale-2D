using UnityEngine;
public class PowerupDatabaseHolder : MonoBehaviour
{
    public PowerupDatabase database;

    public static PowerupDatabase Instance;

    void Awake()
    {
        Instance = database;
    }
}