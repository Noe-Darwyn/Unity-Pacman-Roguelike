using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Button_Hover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    [SerializeField] public GameObject MenuPanel;
    void Start()
    {
        // Initialize any required setup here
        if (MenuPanel == null)
        {
            //Debug.LogWarning("MenuPanel is not assigned in the inspector.");
            MenuPanel = GetComponentInParent<MenuUI>()?.gameObject;
            if (MenuPanel == null)
            {
                Debug.LogWarning("MenuPanel could not be found in parent objects.");
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Code to execute when the pointer enters the button area
        //Debug.Log("Pointer entered button area.");
        MenuPanel?.GetComponent<MenuUI>().OnHoverButton(this.transform);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Code to execute when the pointer exits the button area
        //Debug.Log("Pointer exited button area.");
        MenuPanel?.GetComponent<MenuUI>().OnHoverButtonExit(this.transform);
    }
}
