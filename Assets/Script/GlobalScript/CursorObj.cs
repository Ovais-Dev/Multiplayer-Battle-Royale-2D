using UnityEngine;
using UnityEngine.InputSystem;

public class CursorObj : MonoBehaviour
{
    public static CursorObj Instance;
    [SerializeField] private GameObject cursorGameObject;
    Vector2 mousePos; // screen
    Vector2 mouseDelta;

    
    private void Awake()
    {
        Instance = this;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Cursor.visible = false;
       // Cursor.lockState = CursorLockMode.Locked;
       // Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        mousePos = Mouse.current.position.ReadValue();
        if (Keyboard.current.eKey.IsPressed())
        {
            MakeVisible();
        }
        transform.position = GetMouseWorldPosition();
    }
    public void Deactivate()
    {
        cursorGameObject.SetActive(false);
        Cursor.visible = true;
    }
    public void Activate()
    {
        cursorGameObject.SetActive(true);
        Cursor.visible = false;
    }
    public Vector2 GetMouseDelta()
    {
        return mouseDelta;
    }
    public Vector2 GetMouseWorldPosition()
    {
        if (float.IsNaN(mousePos.x) || float.IsNaN(mousePos.y)) return Vector2.zero;
        return Camera.main.ScreenToWorldPoint(mousePos);
    }
    void MakeVisible()
    {
        Cursor.visible = !Cursor.visible;
    }
}