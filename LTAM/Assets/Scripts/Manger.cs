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

    bool[] check = new bool[8] ;
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
        for(int i = 0; i < 8; i++)
        {
            check[i] = true;
        }
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
        keywords = new string[] { "Door" };
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
                if (check[0])
                {
                    UI.SetActive(false);
                    StartCoroutine(SetActiveUI());
                    Randoms();
                    check[0] = false;
                }
                break;
            case "Curtain":
                index = 1;
                StartCoroutine(SlowlyMovePos());
                if (check[1])
                {
                    UI.SetActive(false);
                    StartCoroutine(SetActiveUI());
                    Randoms();
                    check[1] = false;
                }
                break;
            case "Bed":
                index = 2;
                StartCoroutine(SlowlyMovePos());
                if (check[2])
                {
                    UI.SetActive(false);
                    StartCoroutine(SetActiveUI());
                    Randoms();
                    check[2] = false;
                }
                break;
            case "Book":
                index = 3;
                StartCoroutine(SlowlyMovePos());
                if (check[3])
                {
                    UI.SetActive(false);
                    StartCoroutine(SetActiveUI());
                    Randoms();
                    check[3] = false;
                }
                break;
            case "Sofa":
                index = 4;
                StartCoroutine(SlowlyMovePos());
                if (check[4])
                {
                    UI.SetActive(false);
                    StartCoroutine(SetActiveUI());
                    Randoms();   
                    check[4] = false;
                }
                break;
            case "Phone":
                index = 5;
                StartCoroutine(SlowlyMovePos());
                if (check[5])
                {
                    UI.SetActive(false);
                    StartCoroutine(SetActiveUI());
                    Randoms();                    
                    check[5] = false;
                }
                break;
            case "Tivi":
                index = 6;
                StartCoroutine(SlowlyMovePos());
                if (check[6])
                {
                    UI.SetActive(false);
                    StartCoroutine(SetActiveUI());
                    Randoms();
                    check[6] = false;
                }
                break;
            case "Table Work":
                index = 7;
                StartCoroutine(SlowlyMovePos());
                if (check[7])
                {
                    UI.SetActive(false);
                    StartCoroutine(SetActiveUI());
                    Randoms();
                    check[7] = false;
                }
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
            RandomAnswer(i);
            d[i] = 1;
        }
    }
    IEnumerator SetActiveUI()
    {
        yield return new WaitForSeconds(5f);
        UI.SetActive(true);
    }
    public void RandomAnswer(int i)
    {
        keywords = new string[] { keyWordsY[i] };
        int r = Random.Range(0, 2);
        answerText[r].text = keyWordsY[i];
        if (r == 0)
        {
            answerText[1].text = keyWordsN[Random.Range(0, 10)];
            answerText[2].text = keyWordsN[Random.Range(0, 10)];
        }
        else if (r == 1)
        {
            answerText[0].text = keyWordsN[Random.Range(0, 10)];
            answerText[2].text = keyWordsN[Random.Range(0, 10)];
        }
        else
        {
            answerText[0].text = keyWordsN[Random.Range(0, 10)];
            answerText[2].text = keyWordsN[Random.Range(0, 10)];
        }
    }
}
