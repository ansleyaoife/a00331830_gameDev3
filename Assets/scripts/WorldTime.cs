using Unity.VisualScripting;
using UnityEngine;

public class WorldTime : MonoBehaviour
{
    public static WorldTime Instance;

    //Time
    [SerializeField] private float dayLengthSeconds = 120f;
    [SerializeField] private float startHour = 8f;
    [SerializeField] private float bedtimeHour = 12f;
    [SerializeField] private float wakeUpHour = 8f;

    public int Day { get; private set; } = 1;
    public float TimeOfDay { get; private set; }
    public bool IsPaused { get; private set; } = false;
    public bool IsNight => TimeOfDay < 6f || TimeOfDay < 6f || TimeOfDay >= 20f;

    public event System.Action OnNewDay;
    public event System.Action OnBedtime;

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
        if (IsPaused) return;

        float hoursPerSecond = 24f / dayLengthSeconds;
        TimeOfDay += hoursPerSecond * Time.deltaTime;

        // trigger bedtime
        if (TimeOfDay >= bedtimeHour)
        {
            TimeOfDay = bedtimeHour;
            PauseTime();
            OnBedtime?.Invoke();
        }
    }

    public void PauseTime()
    {
        IsPaused = true;
    }

    public void SkipToMorning()
    {
        TimeOfDay = wakeUpHour;
        Day++;
        IsPaused = false;
        OnNewDay?.Invoke();
    }
}
