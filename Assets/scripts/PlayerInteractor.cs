using UnityEngine;

public class PlayerInteractor : MonoBehaviour
{

    //interaction
    [SerializeField] private float interactRange = 5f;
    [SerializeField] private LayerMask interactableLayers;
    [SerializeField] private Camera playerCamera;

    private ChillaStats hovered;

    void Update()
    {
        if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
        {
            hovered = null;
            return;
        }

        if (playerCamera != null)
        {
            Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);

            if (Physics.Raycast(ray, out RaycastHit hit, interactRange, interactableLayers))
            {
                hovered = hit.collider.GetComponentInParent<ChillaStats>(); 
            }
        }

        if (hovered != null && Input.GetKeyDown(KeyCode.E))
        {
            ChillaUI.Instance.Open(hovered);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ChillaUI.Instance.Close();
        }
    }

    private void OnGUI()
    {
        if (hovered != null && ! ChillaUI.Instance.panel.activeSelf)
        {
            GUI.Label(new Rect(Screen.width / 2 - 60, Screen.height / 2 + 40, 200, 30), "Press E to interact");
        }
    }
}
