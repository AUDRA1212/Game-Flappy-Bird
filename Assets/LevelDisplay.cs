using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelDisplay : MonoBehaviour
{
    public static bool isShowing = false;

    void Start()
    {
        string scene = SceneManager.GetActiveScene().name;
        if (!scene.StartsWith("Level")) { isShowing = false; return; }
        if (!int.TryParse(scene.Replace("Level", ""), out int num)) { isShowing = false; return; }

        StartCoroutine(ShowLevelCard(num));
    }

    IEnumerator ShowLevelCard(int levelNum)
    {
        isShowing = true;
        // Root canvas — sort order 200 so it's above everything
        GameObject canvasObj = new GameObject("_LevelCard");
        Canvas cv = canvasObj.AddComponent<Canvas>();
        cv.renderMode = RenderMode.ScreenSpaceOverlay;
        cv.sortingOrder = 200;
        canvasObj.AddComponent<UnityEngine.UI.CanvasScaler>();
        canvasObj.AddComponent<UnityEngine.UI.GraphicRaycaster>();
        CanvasGroup cg = canvasObj.AddComponent<CanvasGroup>();
        cg.interactable = false;
        cg.blocksRaycasts = false;

        // Dark full-screen dimmer that fades
        GameObject dimmer = MakeImage(canvasObj.transform, new Color(0, 0, 0, 0.6f), true);
        SetStretch(dimmer);

        // Center card panel
        GameObject card = MakeImage(canvasObj.transform, new Color(0.05f, 0.05f, 0.15f, 0.95f), false);
        RectTransform cardRT = card.GetComponent<RectTransform>();
        cardRT.anchorMin = new Vector2(0.5f, 0.5f);
        cardRT.anchorMax = new Vector2(0.5f, 0.5f);
        cardRT.sizeDelta = new Vector2(520, 160);
        cardRT.anchoredPosition = new Vector2(0, 0);

        // Accent bar (top of card)
        GameObject bar = MakeImage(card.transform, new Color(1f, 0.75f, 0.1f, 1f), false);
        RectTransform barRT = bar.GetComponent<RectTransform>();
        barRT.anchorMin = new Vector2(0, 1);
        barRT.anchorMax = new Vector2(1, 1);
        barRT.sizeDelta = new Vector2(0, 6);
        barRT.anchoredPosition = Vector2.zero;
        barRT.pivot = new Vector2(0.5f, 1f);

        // "LEVEL" label (small, top)
        GameObject label = MakeText(card.transform, "L E V E L", 22, Color.white, FontStyles.Normal);
        RectTransform labelRT = label.GetComponent<RectTransform>();
        labelRT.anchorMin = new Vector2(0.5f, 0.65f);
        labelRT.anchorMax = new Vector2(0.5f, 0.65f);
        labelRT.sizeDelta = new Vector2(400, 40);
        labelRT.anchoredPosition = Vector2.zero;

        // Level number (big, bold, yellow)
        GameObject numObj = MakeText(card.transform, levelNum.ToString(), 90,
            new Color(1f, 0.85f, 0.15f, 1f), FontStyles.Bold);
        RectTransform numRT = numObj.GetComponent<RectTransform>();
        numRT.anchorMin = new Vector2(0.5f, 0.5f);
        numRT.anchorMax = new Vector2(0.5f, 0.5f);
        numRT.sizeDelta = new Vector2(400, 110);
        numRT.anchoredPosition = new Vector2(0, -22f);

        // Bottom accent bar
        GameObject bar2 = MakeImage(card.transform, new Color(1f, 0.75f, 0.1f, 1f), false);
        RectTransform bar2RT = bar2.GetComponent<RectTransform>();
        bar2RT.anchorMin = new Vector2(0, 0);
        bar2RT.anchorMax = new Vector2(1, 0);
        bar2RT.sizeDelta = new Vector2(0, 4);
        bar2RT.anchoredPosition = Vector2.zero;
        bar2RT.pivot = new Vector2(0.5f, 0f);

        // Animate: slide in from bottom + fade in
        float t = 0f;
        float inDur = 0.35f;
        while (t < inDur)
        {
            float p = t / inDur;
            float ease = 1f - Mathf.Pow(1f - p, 3f); // ease out cubic
            cg.alpha = ease;
            cardRT.anchoredPosition = new Vector2(0, Mathf.Lerp(-80f, 0f, ease));
            t += Time.deltaTime;
            yield return null;
        }
        cg.alpha = 1f;
        cardRT.anchoredPosition = Vector2.zero;

        yield return new WaitForSeconds(1.6f);

        // Animate: slide up + fade out
        t = 0f;
        float outDur = 0.3f;
        while (t < outDur)
        {
            float p = t / outDur;
            float ease = Mathf.Pow(p, 2f); // ease in
            cg.alpha = 1f - ease;
            cardRT.anchoredPosition = new Vector2(0, Mathf.Lerp(0f, 60f, ease));
            t += Time.deltaTime;
            yield return null;
        }

        isShowing = false;
        Destroy(canvasObj);
        Destroy(gameObject);
    }

    GameObject MakeImage(Transform parent, Color color, bool raycast)
    {
        GameObject go = new GameObject("img");
        go.transform.SetParent(parent, false);
        var img = go.AddComponent<UnityEngine.UI.Image>();
        img.color = color;
        img.raycastTarget = raycast;
        return go;
    }

    GameObject MakeText(Transform parent, string text, float size, Color color, FontStyles style)
    {
        GameObject go = new GameObject("txt");
        go.transform.SetParent(parent, false);
        var tmp = go.AddComponent<TextMeshProUGUI>();
        tmp.text = text;
        tmp.fontSize = size;
        tmp.fontStyle = style;
        tmp.alignment = TextAlignmentOptions.Center;
        tmp.color = color;
        tmp.raycastTarget = false;
        return go;
    }

    void SetStretch(GameObject go)
    {
        var rt = go.GetComponent<RectTransform>();
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;
    }
}
