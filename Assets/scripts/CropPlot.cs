using UnityEngine;

public class CropPlot : MonoBehaviour
{
    public enum PlotState { Empty, Growing, Ready }

    //growth states
    [SerializeField] private PlotState state = PlotState.Empty;
    [SerializeField] private float growthDuration = 30f;
    [SerializeField] private float waterDuration = 15f;
    [SerializeField] private int harvestYield = 2;

    //visuals
    [SerializeField] private MeshRenderer soilRenderer;
    [SerializeField] private Transform plantVisual;
    [SerializeField] private Color dryColour = new Color(0.45f, 0.3f, 0.18f);
    [SerializeField] private Color wetColour = new Color(0.25f, 0.15f, 0.08f);

    private float growthProgress;
    private float waterTimer;

    private bool IsWatered => waterTimer > 0f;

    void Start()
    {
        if (plantVisual != null)
        {
            plantVisual.gameObject.SetActive(state != PlotState.Empty);
        }
    }

    void Update()
    {
        if (waterTimer > 0f)
        {
            waterTimer -= Time.deltaTime;
        }

        if (state == PlotState.Growing && IsWatered)
        {
            growthProgress += Time.deltaTime / growthDuration;

            if (plantVisual != null)
            {
                float scale = Mathf.Lerp(0.1f, 1f, Mathf.Clamp01(growthProgress));
                plantVisual.localScale = Vector3.one * scale;
            }
        }

        if (growthProgress >= 1f)
        {
            growthProgress = 1f;
            state = PlotState.Ready;
        }
        if (soilRenderer != null)
        {
            soilRenderer.material.color = IsWatered ? wetColour : wetColour;
        }
    }

    public void Interact(GameObject obj)
    {
        switch (state)
        {
            case PlotState.Empty : Plant();
                break;
            case PlotState.Growing: Water();
                break;
            case PlotState.Ready: Harvest();
                break;
        }
    }

    public string GetPrompt()
    {
        switch (state)
        {
            case PlotState.Empty: return "Click to plant";
            case PlotState.Growing: return IsWatered ? "Growing..." : "Click to water";
            case PlotState.Ready: return "Click to harvest";
        }
        return "";
    }

    void Plant()
    {
        state = PlotState.Growing;
        growthProgress = 0f;
        waterTimer = 0f;

        if (plantVisual != null)
        {
            plantVisual.gameObject.SetActive(true);
            plantVisual.localScale = Vector3.one * 0.1f;
        }
    }

    void Water()
    {
        waterTimer = waterDuration;
    }

    void Harvest()
    {
        if(PlayerInventory.Instance != null)
        {
            PlayerInventory.Instance.AddFood(harvestYield);

            state = PlotState.Empty;
            growthProgress = 0f;
            waterTimer = 0f;
        }

        if (plantVisual != null)
        {
            plantVisual.gameObject.SetActive(false);
        }
    }

}
