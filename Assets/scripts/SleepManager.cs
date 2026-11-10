using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SleepManager : MonoBehaviour
{
    public static SleepManager Instance;

    // ui references
    [SerializeField] private GameObject bedtimePopup;
    [SerializeField] private Image fadeImage;
    [SerializeField] private TMP_Text bedtimeText;

    private bool isSleeping = false;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return;}
        Instance = this;
    }

    void Start()
    {
        bedtimePopup.SetActive(false);
        fadeImage.color = new Color(0, 0, 0, 0);
        fadeImage.gameObject.SetActive(false);

        if (WorldTime.Instance != null) { WorldTime.Instance.OnBedtime += ShowBedTimePrompt; }
    }

    private void OnDestroy()
    {
        if (WorldTime.Instance != null) { WorldTime.Instance.OnBedtime -= ShowBedTimePrompt; }
    }

    void ShowBedTimePrompt()
    {
        bedtimePopup.SetActive(true);
    }

    public void GoToSleep()
    {
        if (isSleeping) return;
        StartCoroutine(SleepRoutine());
    }

    IEnumerator SleepRoutine()
    {
        isSleeping = true;
        bedtimePopup.SetActive(false);
        fadeImage.gameObject.SetActive(true);

        // fade to black
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime;
            fadeImage.color = new Color(0, 0, 0, t);
            yield return null;
        }

        // skip time
        WorldTime.Instance.SkipToMorning();
        yield return new WaitForSeconds(1f);

        // fade back in
        t = 1f;
        while (t > 0f)
        {
            t -= Time.deltaTime;
            fadeImage.color = new Color(0, 0, 0, t);
            yield return null;
        }

        fadeImage.gameObject.SetActive(false);
        isSleeping = false;
    }
}
