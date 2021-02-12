using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkText : MonoBehaviour {
    public int numBlinks;
    public float timeOnsScreen;
    public float timeOffScreen;
    public string caption;

    private TextMesh textMesh;

    public void SetText(string txt, Color color, int size)
    {
        textMesh = GetComponent<TextMesh>();
        caption = txt;
        textMesh.fontSize = size;
        textMesh.color = color;
        StartCoroutine(Blink());
    }

    IEnumerator Blink()
    {
        for (int i = 0; i < numBlinks; i++)
        {
            textMesh.text = caption;
            yield return new WaitForSeconds(timeOnsScreen);
            textMesh.text = "";
            yield return new WaitForSeconds(timeOffScreen);
        }
        Destroy(gameObject);
    }

}
