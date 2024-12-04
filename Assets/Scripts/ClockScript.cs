using UnityEngine;

public class ClockScript : MonoBehaviour
{
    private float gameTime;
    private TMPro.TextMeshProUGUI clcok;
    void Start()
    {
        gameTime= 0f;
        clcok = GetComponent<TMPro.TextMeshProUGUI>();
    }

    void Update()
    {
        gameTime += Time.deltaTime;
    }

    private void LateUpdate()
    {
        int t = (int)gameTime;
        int h = t / 3600;
        int m = (t % 3600) / 60;
        int s = t % 60;

        clcok.text = $"{h:D2}:{m:D2}:{s:D2}";
    }
}
