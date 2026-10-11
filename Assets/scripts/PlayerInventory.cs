using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory Instance;

    //inventory
    public int foodCount;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void AddFood(int amount)
    {
        foodCount += amount;
        Debug.Log($"Picked up {amount} food. Total: {foodCount}");
    }

    public bool UseFood(int amount)
    {
        if (foodCount >= amount)
        {
            foodCount -= amount;
            return true;
        }
        return false;
    }
}
