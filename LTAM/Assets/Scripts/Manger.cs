using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.UIElements;
using UnityEngine.Windows.Speech;
using Image = UnityEngine.UI.Image;
using UnityEngine.UI;

public class Manger : MonoBehaviour
{
    public string[] keywords = new string[] { };
    public ConfidenceLevel confidence = ConfidenceLevel.Medium;
    public GameObject[] objTarget;
    public int index;
    public float timeCameraMoveRot;
    public float timeCameraMovePos;
    public float timeperFrame;

    bool check = true;
    public GameObject UI;
    protected PhraseRecognizer recognizer;
    public string word = "";
    public Image image;
    public Text[] answerText=new Text[3];
    public Text results;
    public List<Sprite> FULLHP;
    int[] d=new int[9];
    string[] keyWordsY = new string[] { "Door", "Curtain", "Bed", "Book", "Sofa", "Phone", "Tivi", "Table Work" };
    string[] keyWordsN = new string[] { "Money", "Dola", "Gold", "Silver", "Diamond", "Fridge","Lam Handsome",
        "Quan Handsome","Lien Handsome","Dog","Cat","Roasted Chicken"};

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
        answerText[0].text = "Door";answerText[1].text = keyWordsN[Random.Range(0, 10)]; answerText[2].text = keyWordsN[Random.Range(0, 10)];
    }

    private void Recognizer_OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        word = args.text;
        results.text = "You said: <b>" + word + "</b>";
    }

    void FixedUpdate()
    {
        switch (word)
        {
            case "Door":
                index = 0;
                StartCoroutine(SlowlyMovePos());
                //StartCoroutine(DisplayImage(1));
                if (check)
                {
                    UI.SetActive(false);
                    Randoms();
                    check = false;
                }
                StartCoroutine(SetActiveUI());
                break;
            case "Curtain":
                index = 1;
                StartCoroutine(SlowlyMovePos());
                StartCoroutine(DisplayImage(2));
                break;
            case "Bed":
                index = 2;
                StartCoroutine(SlowlyMovePos());
                StartCoroutine(DisplayImage(3));
                break;
            case "Book":
                index = 3;
                StartCoroutine(SlowlyMovePos());
                StartCoroutine(DisplayImage(4));
                break;
            case "Sofa":
                index = 4;
                StartCoroutine(SlowlyMovePos());
                StartCoroutine(DisplayImage(5));
                break;
            case "Phone":
                index = 5;
                StartCoroutine(SlowlyMovePos());
                StartCoroutine(DisplayImage(6));
                break;
            case "Tivi":
                print("log");
                index = 6;
                StartCoroutine(SlowlyMovePos());
                StartCoroutine(DisplayImage(7));
                break;
            case "Table Work":
                index = 7;
                StartCoroutine(SlowlyMovePos());
                StartCoroutine(DisplayImage(7));
                break;
        }
        
    }

    IEnumerator SlowlyMovePos()
    {
        float time = 0f;
        while(transform.position != objTarget[index].transform.position)
        {
            yield return new WaitForSeconds(timeperFrame);

            transform.position = Vector3.MoveTowards(transform.position, objTarget[index].transform.position, timeCameraMovePos);
            transform.rotation = Quaternion.Slerp(transform.rotation, objTarget[index].transform.rotation, time);
            time += timeCameraMoveRot;
        }
    }
    IEnumerator DisplayImage(int i)
    {
        yield return new WaitForSeconds(3f);
        image.sprite = FULLHP[i];
    }
    private void OnApplicationQuit()
    {
        if (recognizer != null && recognizer.IsRunning)
        {
            recognizer.OnPhraseRecognized -= Recognizer_OnPhraseRecognized;
            recognizer.Stop();
        }
    }
    public void Randoms()
    {
        int i = Random.Range(1, 7);
        if (d[i] == 1)
        {
            Randoms();
        }
        else
        {
            image.sprite = FULLHP[i];
            d[i] = 1;
        }
        
    }
    IEnumerator SetActiveUI()
    {
        yield return new WaitForSeconds(5f);
        UI.SetActive(true);
        //Randoms();
    }
}
