using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class ChillaStats : MonoBehaviour
{

    //Stats
    [UnityEngine.Range(0, 100)] public float hunger = 80f;
    [UnityEngine.Range(0, 100)] public float energy = 80f;
    [UnityEngine.Range(0, 100)] public float cleanliness = 80f;
    [UnityEngine.Range(0, 100)] public float happiness = 80f;

    //Decay per in game hour
    public float hungerDecay = 4f;
    public float energyDecay = 3f;
    public float cleanlinessDecay = 2f;
    public float happinessDecay = 1.5f;

    public bool IsSleeping {  get; private set; }

    private float lastTimeOfDay;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        lastTimeOfDay = WorldTime.Instance != null ? WorldTime.Instance.TimeOfDay : 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (WorldTime.Instance == null) return;

        float current = WorldTime.Instance.TimeOfDay;
        
        float hoursPassed = current - lastTimeOfDay;
        
        if (hoursPassed < 0) hoursPassed += 24f;
        
        lastTimeOfDay = current;

        if (hoursPassed <= 0f || hoursPassed > 12f) return;

        //go to sleep at 8pm
        if (WorldTime.Instance.IsNight && !IsSleeping)
        {
            if (WorldTime.Instance.TimeOfDay >= 20f || WorldTime.Instance.TimeOfDay < 6f)
            {
                SetSleeping(true);
            }
        }

        //wake up at 6am
        if (!WorldTime.Instance.IsNight && IsSleeping)
        {
            SetSleeping(false);
        }

        Decay(hoursPassed);
    }

    void Decay(float hours)
    {
        hunger = Mathf.Max(0f, hunger - hungerDecay * hours);
        cleanliness = Mathf.Max(0f, cleanliness - cleanlinessDecay * hours);

        if(IsSleeping)
        {
            energy = Mathf.Min(100f, energy + 15f * hours);
        }
        else
        {
            energy = Mathf.Max(0f, energy - energyDecay * hours);
        }

        float nightMultiplier = WorldTime.Instance.IsNight ? 0.5f : 1f;

        hunger = Mathf.Max(0f, hunger - hungerDecay * hours * nightMultiplier);

        cleanliness = Mathf.Max(0f, cleanliness - cleanlinessDecay * hours * nightMultiplier);


        //speed up happiness decay with low needs
        float penalty = 1f;

        if (hunger < 30f) penalty += 0.5f;
        
        if (cleanliness < 30f) penalty += 0.5f;

        if (energy < 30f) penalty += 0.5f;

        happiness = Mathf.Max(0f, happiness - happinessDecay * hours * penalty);

    }

    public void Feed()
    {
        hunger = Mathf.Min(100f, hunger + 40f);

        happiness = Mathf.Min(100f, happiness + 5f);
    }

    public void Clean()
    {
        cleanliness = Mathf.Min(100f, cleanliness + 50f);

        happiness = Mathf.Min(100f, happiness + 5f);
    }

    public void Play()
    {
        //too tired to play?
        if (energy < 10f)
        {
            return;
        }

        happiness = Mathf.Min(100f, happiness + 30f);

        energy = Mathf.Max(0f, energy - 10f);
    }

    public void SetSleeping(bool sleeping)
    {
        IsSleeping = sleeping;
    }

    public void ToggleSleep()
    {
        IsSleeping = !IsSleeping;
    }
}
