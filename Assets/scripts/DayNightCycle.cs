using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    //references
    public Light sun;
    public Camera mainCamera;
    public bool tintCameraBackground = true;

    //gradient curves
    public AnimationCurve intensityCurve = new AnimationCurve(
        new Keyframe(0f, 0.05f),
        new Keyframe(5f, 0.05f),
        new Keyframe(7f, 0.8f),
        new Keyframe(12f, 1.2f),
        new Keyframe(18f, 0.8f),
        new Keyframe(20f, 0.05f),
        new Keyframe(24f, 0.05f)
        );

    //light colour
    public Gradient lightColour = new Gradient();

    //rotation
    public Transform sunPivot;

    public float rotationOffset = -90f;

    //ambient light
    public Color dayAmbient = new Color(0.6f, 0.15f, 0.7f);
    public Color nightAmbient = new Color(0.15f, 0.15f, 0.25f);

    //lifecycle
    void Start()
    {
        if (sunPivot == null) sunPivot = transform;

        //set default gradient
        if (lightColour.colorKeys.Length <= 1)
        {
            GradientColorKey[] colours = new GradientColorKey[]
            {
                new GradientColorKey(new Color(0.4f, 0.4f, 0.7f), 0f / 24f), //midnight
                new GradientColorKey(new Color(0.5f, 0.4f, 0.6f), 5f / 24f), //predawn
                new GradientColorKey(new Color(1.0f, 0.7f, 0.5f), 7f / 24f), //sunrise
                new GradientColorKey(new Color(1.0f, 0.7f, 0.05f), 12f / 24f), //noon
                new GradientColorKey(new Color(1.0f, 0.6f, 0.4f), 18f / 24f), //sunset
                new GradientColorKey(new Color(0.4f, 0.4f, 0.7f), 21f / 24f), //dusk
                new GradientColorKey(new Color(0.4f, 0.4f, 0.7f), 1f) //midnight
            };

            GradientAlphaKey[] alphas = new GradientAlphaKey[]
            {
                new GradientAlphaKey(1f, 0f),
                new GradientAlphaKey(1f, 1f),
            };

            lightColour.SetKeys(colours, alphas);

        }
    }

    void Update()
    {
        if (WorldTime.Instance == null || sun == null) return;

        float time = WorldTime.Instance.TimeOfDay;
        float normalized = time / 24f;

        //rotate the sun
        float sunAngle = normalized * 360f + rotationOffset;
        sunPivot.rotation = Quaternion.Euler(sunAngle, 30f, 0f);

        //update light intensity
        sun.intensity = intensityCurve.Evaluate(time);

        //update light colour
        sun.color = lightColour.Evaluate(normalized);

        //update ambient lighting (based on light intensity)
        float ambientBlend = Mathf.Clamp01(sun.intensity);
        RenderSettings.ambientLight = Color.Lerp(nightAmbient, dayAmbient, ambientBlend);

        //tint camera background
        if (tintCameraBackground && mainCamera != null)
        {
            mainCamera.backgroundColor = Color.Lerp(nightAmbient, dayAmbient, ambientBlend);
        }
    }
}
