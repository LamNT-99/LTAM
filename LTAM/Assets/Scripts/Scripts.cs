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
    protected string word = "Table work";
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
                if (y >= 3.8f) y -= 0.1f;
                else if (y <= 3.6f) y += 0.1f;
                else y = 3.7f;
                if (y1 <= 271) y1 += 4f;
                else if (y1 > 279) y1 -= 4f;
                else y1 = 275;
                x1 = z1 = 0;
                image.sprite = FULLHP[2];
                break;
            case "Bed":
                if (x >= 12.2f) x -= 0.1f;
                else if (x <= 12) x += 0.1f;
                else x = 12.1f;
                if (y <= 5.3f) y += 0.1f;
                else if (y >= 5.5f) y -= 0.1f;
                else y = 5.4f;
                if (z <= 5.9f) z += 0.1f;
                else if (z >= 6.2f) z -= 0.1f;
                else z = 6.1f;
                if (x1 <= 16f) x1 += 2f;
                else if (x1 >= 20f) x1 -= 2f;
                else x1 = 18.5f;
                if (y1 >= 273f) y1 -= 3f;
                else if (y1 <= 267f) y1 += 3f;
                else y1 = 270f;
                z1 = 0;
                image.sprite = FULLHP[3];
                break;
            case "Book":
                if (x <= 4.55f) x += 0.1f;
                else if (x >= 4.75f) x -= 0.1f;
                else x = 4.65f;
                if (y <= 2.35f) y += 0.1f;
                else if (y >= 2.55f) y -= 0.1f;
                else y = 2.45f;
                if (z >= 11.2f) z -= 0.1f;
                else if (z <= 10.9f) z += 0.1f;
                else z = 11.1f;
                if (x1 >= 46f) x1 -= 2f;
                else if (x1 <= 42f) x1 += 2f;
                else x1 = 44f;
                if (y1 >= 273f) y1 -= 3f;
                else if (y1 <= 267f) y1 += 3f;
                else y1 = 270f;
                z1 = 0;
                image.sprite = FULLHP[3];
                break;
            case "Sofa":
                if (x <= 16.5f) x += 0.1f;
                else if (x >= 16.7f) x -= 0.1f;
                else x = 16.6f;
                if (y <= 4.4f) y += 0.1f;
                else if (y >= 4.6f) y -= 0.1f;
                else y = 4.5f;
                if (z >= 10.8f) z -= 0.1f;
                else if (z <= 10.6f) z += 0.1f;
                else z = 10.7f;
                if (x1 >= 14f) x1 -= 2f;
                else if (x1 <= 10f) x1 += 2f;
                else x1 = 12.2f;
                if (y1 >= 183f) y1 -= 3f;
                else if (y1 <= 177f) y1 += 3f;
                else y1 = 180f;
                z1 = 0;
                image.sprite = FULLHP[3];
                break;
            case "Phone":
                if (x <= 18f) x += 0.1f;
                else if (x >= 18.2f) x -= 0.1f;
                else x = 18.1f;
                if (y <= 3.3f) y += 0.1f;
                else if (y >= 3.5f) y -= 0.1f;
                else y = 3.4f;
                if (z >= 6.6f) z -= 0.1f;
                else if (z <= 6.4f) z += 0.1f;
                else z = 6.5f;
                if (x1 >= 32f) x1 -= 2f;
                else if (x1 <= 28f) x1 += 2f;
                else x1 = 30f;
                if (y1 >= 183f) y1 -= 3f;
                else if (y1 <= 177f) y1 += 3f;
                else y1 = 180f;
                z1 = 0;
                image.sprite = FULLHP[3];
                break;
            case "Tivi":
                if (x <= 22.6f) x += 0.1f;
                else if (x >= 22.8f) x -= 0.1f;
                else x = 22.7f;
                if (y <= 3.3f) y += 0.1f;
                else if (y >= 3.5f) y -= 0.1f;
                else y = 3.4f;
                if (z >= 8.3f) z -= 0.1f;
                else if (z <= 8.1f) z += 0.1f;
                else z = 8.2f;
                if (y1 >= 93f) y1 -= 3f;
                else if (y1 <= 87f) y1 += 3f;
                else y1 = 90f;
                x1 = z1 = 0;
                image.sprite = FULLHP[3];
                break;
            case "Table work":
                if (x <= 20.2f) x += 0.1f;
                else if (x >= 20.4f) x -= 0.1f;
                else x = 20.3f;
                if (y <= 5.2f) y += 0.1f;
                else if (y >= 5.4f) y -= 0.1f;
                else y = 5.3f;
                if (z >= 14.6f) z -= 0.1f;
                else if (z <= 14.4f) z += 0.1f;
                else z = 14.5f;
                if (x1 >= 18f) x1 -= 2f;
                else if (x1 <= 14f) x1 += 2f;
                else x1 = 16.5f;
                if (y1 >= 36f) y1 -= 3f;
                else if (y1 <= 31f) y1 += 3f;
                else y1 = 34f;
                z1 = 0;
                image.sprite = FULLHP[3];
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
