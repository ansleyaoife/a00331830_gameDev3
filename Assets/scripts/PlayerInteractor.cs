using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerInteractor : MonoBehaviour
{

    //interaction
    [SerializeField] private float interactRange = 5f;
    [SerializeField] private LayerMask interactableLayers;
    [SerializeField] private Camera playerCamera;

    private GameObject hovered;

    void Update()
    {
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
        {
            hovered = null;
            return;
        }

        hovered = null;

        if (playerCamera != null)
        {
            Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
            
            if (Physics.Raycast(ray, out RaycastHit hit, interactRange, interactableLayers))
                hovered = hit.collider.gameObject;
        }

        //click to inspect
        if (hovered != null && Input.GetMouseButtonDown(0))
        {
            Interact(hovered);
        }

        //escape to close menu
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ChillaUI.Instance.Close();
        }
    }

    void Interact(GameObject obj)
    {
        //Is it Food?
        FoodPickup food = obj.GetComponentInParent<FoodPickup>();

        if (food != null)
        {
            food.Pickup();
            return;
        }

        //Is it a Chilla?
        ChillaStats chilla = obj.GetComponentInParent<ChillaStats>();

        if (chilla != null)
        {
            ChillaUI.Instance.Open(chilla);
            return;
        }
    }

    private void OnGUI()
    {
        if (hovered == null) return;

        if (ChillaUI.Instance.panel.activeSelf) return;

        string prompt = null;

        if (hovered.GetComponentInParent<ChillaStats>() != null) prompt = "Click to inspect";

        if (prompt != null) GUI.Label(new Rect(Screen.width / 2 - 60, Screen.height / 2 + 40, 200, 30), prompt);
    }
}
