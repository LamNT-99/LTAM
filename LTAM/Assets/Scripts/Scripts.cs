using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using UnityEngine.Windows.Speech;
using Image = UnityEngine.UI.Image;

public class Scripts : MonoBehaviour
{
    public string[] keywords = new string[] { };
    public ConfidenceLevel confidence = ConfidenceLevel.Medium;
    public float speed = 1;

    public Text results;
    public Camera cameras;
    public List<Vector3> vector3s;

    protected PhraseRecognizer recognizer;
    protected string word = "Bed";
    public Image image;
    public List<Sprite> FULLHP;
    private void Start()
    {
        if (keywords != null)
        {
            recognizer = new KeywordRecognizer(keywords, confidence);
            recognizer.OnPhraseRecognized += Recognizer_OnPhraseRecognized;
            recognizer.Start();
            Debug.Log(recognizer.IsRunning);
        }

        foreach (var device in Microphone.devices)
        {
            Debug.Log("Name: " + device);
        }
        image.sprite = FULLHP[0];
    }

    private void Recognizer_OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        word = args.text;
        results.text = "You said: <b>" + word + "</b>";
    }

    private void Update()
    {
        var x = cameras.transform.position.x;
        var y = cameras.transform.position.y;
        var z = cameras.transform.position.z;
        var x1 = cameras.transform.eulerAngles.x;
        var y1 = cameras.transform.eulerAngles.y;
        var z1 = cameras.transform.eulerAngles.z;
        switch (word)
        {
            case "Door":
                if (z <= 12.9) z += 0.1f;
                else if (z >= 13.1) z -= 0.1f;
                else z = 13f;
                x1 = y1 = z1 = 0;
                image.sprite = FULLHP[1];
                break;
            case "b"://curtains
                if (z <= 15.4f) z += 0.1f;
                else if (z >= 15.6f) z -= 0.1f;
                else z = 15.5f;
                if (x >= 11.4f) x -= 0.1f;
                else if (x <= 11.2f) x += 0.1f;
                else x = 11.3f;
                if (y1 <= 271) y1 += 4f;
                else if (y1 > 279) y1 -= 4f;
                else y1 = 275;
                x1 = z1 = 0;y = 3.7f;
                image.sprite = FULLHP[2];
                break;
            case "Bed":
                if (x >= 9.1) x -= 0.1f;
                else if (x <= 8.9) x += 0.1f;
                else x = 9f;
                if (y <= 4.9f) y += 0.1f;
                else if (y >= 5.1f) y -= 0.1f;
                else y = 5f;
                if (z <= 16.4f) z += 0.1f;
                else if (z >= 16.6f) z -= 0.1f;
                else z = 16.5f;
                if (x1 <= 18f) x1 += 2f;
                else if (x1 >= 22f) x1 -= 2f;
                else x1 = 20f;
                if (y1 >= 188f) y1 -= 3f;
                else if (y1 <= 182f) y1 += 3f;
                else y1 = 185f;
                z1 = 0;
                image.sprite = FULLHP[3];
                break;
            case "Tivi":
                if (x <= 13) x += 0.1f;
                if (y <= 6.5f) y += 0.1f;
                if (z >= 7.5f) z -= 0.1f;
                if (x1 >= 0) x1 -= 2f;
                break;
            default:
                break;
        }
        cameras.transform.position = new Vector3(x, y, z);
        cameras.transform.localEulerAngles = new Vector3(x1,y1,z1);
    }

    private void OnApplicationQuit()
    {
        if (recognizer != null && recognizer.IsRunning)
        {
            recognizer.OnPhraseRecognized -= Recognizer_OnPhraseRecognized;
            recognizer.Stop();
        }
    }
}
