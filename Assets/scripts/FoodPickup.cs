using UnityEngine;

public class FoodPickup : MonoBehaviour
{
    public int foodAmount = 1;

    //regenerate
    public bool respawn = true;
    public float respawnTime = 20f;

    private MeshRenderer meshRenderer;
    private Collider col;

    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        col = GetComponent<Collider>();
    }

    public void Pickup()
    {
        PlayerInventory.Instance.AddFood(foodAmount);

        //hide object
        meshRenderer.enabled = false;
        col.enabled = false;

        if (respawn)
        {
            Invoke(nameof(Respawn), respawnTime);
        }else
        {
            Destroy(gameObject);
        }
    }

    void Respawn()
    {
        meshRenderer.enabled = true;
        col.enabled = true;
    }
}
