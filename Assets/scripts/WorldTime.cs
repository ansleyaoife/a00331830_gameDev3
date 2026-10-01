using Unity.VisualScripting;
using UnityEngine;

public class WorldTime : MonoBehaviour
{
    public static WorldTime Instance;

    //Time
    [SerializeField] private float dayLengthSeconds = 120f;
    [SerializeField] private float startHour = 8f;

    public int Day { get; private set; } = 1;
    public float TimeOfDay { get; private set; }
    public event System.Action OnNewDay;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        TimeOfDay = startHour;
    }

    // Update is called once per frame
    void Update()
    {
        float hoursPerSecond = 24f / dayLengthSeconds;
        TimeOfDay += hoursPerSecond * Time.deltaTime;

        if(TimeOfDay >= 24f)
        {
            TimeOfDay -= 24f;
            Day++;
            OnNewDay ?.Invoke();
        }
    }

    public bool IsNight => TimeOfDay < 6f || TimeOfDay >= 20f;
}
