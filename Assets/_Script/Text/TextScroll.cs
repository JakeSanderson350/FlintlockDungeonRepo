using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class TextScroll : MonoBehaviour
{
    [SerializeField] ScrollTextData textData;
    [SerializeField] TextMeshProUGUI textBox;
    [SerializeField] TextMeshProUGUI promptBox;
    [SerializeField] bool playOnAwake = true;
    [SerializeField] KeyCode nextButton = KeyCode.Space; //tmep
    [SerializeField] UnityEvent onTextFinished;

    Coroutine scrollText;
    private int index = 0;

    private void Start()
    {
        if(playOnAwake)
            Invoke("NextText", 1f);
    }

    private void Update()
    {
        if(Input.GetKeyUp(nextButton))
            NextText();
    }

    void NextText()
    {
        if (scrollText != null) StopCoroutine(scrollText);
        if (index >= textData.text.Count) { TextFinished(); return; }

        scrollText = StartCoroutine(ScrollText(GetNextText()));
        promptBox.gameObject.SetActive(false);
    }


    ScrollTextData.Text GetNextText()
    {
        ScrollTextData.Text output = textData.text[index];
        index++;
        return output;
    }

    IEnumerator ScrollText(ScrollTextData.Text data)
    {
        WaitForSeconds tick = new WaitForSeconds(data.scrollSpeed);
        textBox.text = "";

        for(int i = 0;  textBox.text != data.text; i++)
        {
            textBox.text += data.text[i];
            yield return tick;
        }

        FinishScroll();
    }

    void FinishScroll()
    {
        promptBox.gameObject.SetActive(true);
    }

    void TextFinished()
    {
        index = 0;
        textBox.text = "";
        onTextFinished?.Invoke();
    }
}
