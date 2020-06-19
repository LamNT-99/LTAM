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
    protected string word = "Door";
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
        //FULLHP = Resources.Load<Sprite>("b");
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
                if (z <= 13) z += 0.1f;
                print(FULLHP[0].name);
                image.sprite = FULLHP[0];
                print(z);
                break;
            case "b"://curtains
                if (z <= 15.5f) z += 0.1f;
                if (x >= 11.3f) x -= 0.1f;
                if (y1 <= 275) y1 += 4f;
                print(y1);
                break;
            case "Bed":
                if (x >= 9) x -= 0.1f;
                if (y <= 5) y += 0.1f;
                if (z <= 16.5f) z += 0.1f;
                if (x1 <= 20) x1 += 2f;
                if (y1 >= 185) y1 -= 3f;
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
