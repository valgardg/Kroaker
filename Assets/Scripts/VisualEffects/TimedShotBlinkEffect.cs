using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(SpriteRenderer))]
public class BlinkEffect : MonoBehaviour
{
    [Header("Target")]
    [Tooltip("SpriteRenderer to blink. If not set, will use the SpriteRenderer on this GameObject.")]
    [SerializeField] private SpriteRenderer target;

    [Header("Blink Settings")]
    [Tooltip("Start blinking automatically when the component is enabled.")]
    [SerializeField] private bool playOnEnable = true;

    [Tooltip("Blink frequency (pulses per second). Higher values blink faster.")]
    [SerializeField] [Range(0.1f, 20f)] private float frequency = 2f;

    [Tooltip("Minimum alpha during blink (0 = fully transparent, 1 = fully opaque).")]
    [SerializeField] [Range(0f, 1f)] private float minAlpha = 0.3f;

    [Tooltip("Maximum alpha during blink (0 = fully transparent, 1 = fully opaque).")]
    [SerializeField] [Range(0f, 1f)] private float maxAlpha = 1f;

    [Tooltip("Curve shaping the pulse across 0..1; used with PingPong time.")]
    [SerializeField] private AnimationCurve pulseCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

    [Tooltip("Use unscaled time (ignores Time.timeScale). Useful for UI or pause screens.")]
    [SerializeField] private bool useUnscaledTime = false;

    [Tooltip("Apply a random phase offset so multiple objects don't blink in sync.")]
    [SerializeField] private bool randomPhase = false;

    [Tooltip("Restore the original color when the component is disabled.")]
    [SerializeField] private bool resetColorOnDisable = true;

    private Color _baseColor;
    private bool _isBlinking;
    private float _phaseOffset;

    private void Awake()
    {
        if (target == null)
        {
            target = GetComponent<SpriteRenderer>();
        }

        if (target != null)
        {
            _baseColor = target.color;
        }
    }

    private void OnEnable()
    {
        if (randomPhase)
        {
            _phaseOffset = Random.value;
        }

        if (playOnEnable)
        {
            StartBlink();
        }
    }

    private void OnDisable()
    {
        StopBlink();

        if (resetColorOnDisable && target != null)
        {
            target.color = _baseColor;
        }
    }

    private void Update()
    {
        if (!_isBlinking || target == null)
            return;

        float t = useUnscaledTime ? Time.unscaledTime : Time.time;
        float ping = Mathf.PingPong((t + _phaseOffset) * frequency, 1f);

        // Shape the pulse with the provided curve
        float shaped = pulseCurve != null ? pulseCurve.Evaluate(ping) : ping;

        float a = Mathf.Lerp(minAlpha, maxAlpha, shaped);
        Color c = target.color;
        c.r = _baseColor.r;
        c.g = _baseColor.g;
        c.b = _baseColor.b;
        c.a = a;
        target.color = c;
    }

    public void StartBlink()
    {
        _isBlinking = true;
    }

    public void StopBlink()
    {
        _isBlinking = false;
    }

    public void ToggleBlink()
    {
        _isBlinking = !_isBlinking;
    }

    private void OnValidate()
    {
        if (minAlpha > maxAlpha)
        {
            float tmp = minAlpha;
            minAlpha = maxAlpha;
            maxAlpha = tmp;
        }
        frequency = Mathf.Max(0.1f, frequency);

        if (target == null)
        {
            target = GetComponent<SpriteRenderer>();
        }
    }
}
