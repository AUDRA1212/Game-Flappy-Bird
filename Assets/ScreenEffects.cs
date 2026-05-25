using System.Collections;
using UnityEngine;

public class ScreenEffects : MonoBehaviour
{
    public static ScreenEffects instance;

    void Awake()
    {
        if (instance == null) instance = this;
        else { Destroy(gameObject); return; }
    }

    public void ShakeCamera(float duration = 0.3f, float magnitude = 0.15f)
    {
        StartCoroutine(DoShake(duration, magnitude));
    }

    public void FlashScreen(Color color, float duration = 0.2f)
    {
        StartCoroutine(DoFlash(color, duration));
    }

    IEnumerator DoShake(float duration, float magnitude)
    {
        Camera cam = Camera.main;
        if (cam == null) yield break;

        Vector3 original = cam.transform.position;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;
            cam.transform.position = new Vector3(original.x + x, original.y + y, original.z);
            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }

        cam.transform.position = original;
    }

    IEnumerator DoFlash(Color color, float duration)
    {
        GameObject flashObj = new GameObject("Flash");
        Canvas c = flashObj.AddComponent<Canvas>();
        c.renderMode = RenderMode.ScreenSpaceOverlay;
        c.sortingOrder = 999;

        GameObject imgObj = new GameObject("FlashImage");
        imgObj.transform.SetParent(flashObj.transform, false);
        UnityEngine.UI.Image img = imgObj.AddComponent<UnityEngine.UI.Image>();
        img.color = new Color(color.r, color.g, color.b, 0.6f);
        img.raycastTarget = false;
        RectTransform rt = imgObj.GetComponent<RectTransform>();
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;

        float t = 0f;
        while (t < duration)
        {
            float alpha = Mathf.Lerp(0.6f, 0f, t / duration);
            img.color = new Color(color.r, color.g, color.b, alpha);
            t += Time.unscaledDeltaTime;
            yield return null;
        }

        Destroy(flashObj);
    }
}
