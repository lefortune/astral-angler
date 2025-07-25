using UnityEngine;

public class BackgroundSwitcher : MonoBehaviour
{
    public SpriteRenderer backgroundRenderer;

    public Sprite staticBackground;
    public Sprite[] animatedFrames;
    public float frameRate = 10f;

    private bool useAnimation = false;
    private float timer = 0f;
    private int frameIndex = 0;

    void Start()
    {
        backgroundRenderer.sprite = staticBackground;
    }

    void Update()
    {
        if (useAnimation && animatedFrames.Length > 0)
        {
            timer += Time.deltaTime;
            if (timer >= 1f / frameRate)
            {
                timer = 0f;
                frameIndex = (frameIndex + 1) % animatedFrames.Length;
                backgroundRenderer.sprite = animatedFrames[frameIndex];
            }
        }
    }

    // Call this when your condition becomes true
    public void TriggerAnimatedBackground()
    {
        useAnimation = true;
        frameIndex = 0;
        timer = 0f;
    }
}
