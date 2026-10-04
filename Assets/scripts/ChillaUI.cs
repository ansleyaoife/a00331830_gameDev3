using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChillaUI : MonoBehaviour
{
    public static ChillaUI Instance;

    public GameObject panel;

    //sliders
    public Slider hungerBar;
    public Slider energyBar;
    public Slider cleanlinessBar;
    public Slider happinessBar;

    //buttons
    public Button feedButton;
    public Button sleepButton;
    public Button cleanButton;
    public Button playButton;
    public Button closeButton;

    //labels
    public TMP_Text titleLabel;
    public TMP_Text sleepButtonLabel;

    private ChillaStats currentChilla;

    void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    void Start()
    {
        panel.SetActive(false);

        feedButton.onClick.AddListener(() => currentChilla?.Feed());

        cleanButton.onClick.AddListener(() => currentChilla?.Clean());

        playButton.onClick.AddListener(() => currentChilla?.Play());

        sleepButton.onClick.AddListener(() =>
        {
            currentChilla?.ToggleSleep();
            RefreshSleepLabel();
        });

        closeButton.onClick.AddListener(Close);
    }


    void Update()
    {
        if (currentChilla == null || !panel.activeSelf) return;

        //set bars to the chillas stats
        hungerBar.value = currentChilla.hunger;
        energyBar.value = currentChilla.energy;
        cleanlinessBar.value = currentChilla.cleanliness;
        happinessBar.value = currentChilla.happiness;
    }

    public void Open(ChillaStats chilla)
    {
        currentChilla = chilla;

        panel.SetActive(true);

        if (titleLabel != null) titleLabel.text = chilla.gameObject.name;

        RefreshSleepLabel();
    }

    public void Close()
    {
        panel.SetActive(false);
        currentChilla = null;
    }

    void RefreshSleepLabel()
    {
        if (currentChilla == null || sleepButtonLabel == null) return;

        // if chilla asleep > option to wake them, if awake > option to send to sleep.
        sleepButtonLabel.text = currentChilla.IsSleeping ? "Wake" : "Sleep";
    }
}
