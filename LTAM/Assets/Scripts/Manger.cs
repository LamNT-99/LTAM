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

    bool[] check = new bool[9] ;
    public GameObject UI;
    protected PhraseRecognizer recognizer;
    public string word = "";
    public Image image;
    public Text[] answerText=new Text[3];
    public Text results;
    public List<Sprite> FULLHP;
    int[] d=new int[10];
    bool ck = false;
    string[] keyWordsY = new string[] { "Door", "Curtain", "Bed", "Book", "Sofa", "Phone", "Tivi", "Table Work" };
    string[] keyWordsN = new string[] { "Money", "Dola", "Gold", "Silver", "Diamond", "Fridge","Lam Handsome",
        "Quan Handsome","Lien Handsome","Dog","Cat","Roasted Chicken"};

    private void Start()
    {
        for(int i = 0; i < 9; i++)
        {
            check[i] = true;
        }
        if (keywords != null)
        {
            recognizer = new KeywordRecognizer(keywords, confidence);
            recognizer.OnPhraseRecognized += Recognizer_OnPhraseRecognized;
            recognizer.Start();
            //Debug.Log(recognizer.IsRunning);
        }

        //foreach (var device in Microphone.devices)
        //{
        //    Debug.Log("Name: " + device);
        //}
        Randoms();
    }

    private void Recognizer_OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        word = args.text;
        results.text = "You said: <b>" + word + "</b>";
    }

    void FixedUpdate()
    {
        for(int i = 0; i < 8; i++)
        {
            if (word == keyWordsY[i])
            {
                index = i;
                StartCoroutine(SlowlyMovePos());
                if (check[i])
                {
                    UI.SetActive(false);
                    StartCoroutine(SetActiveUI());
                    Randoms();
                    check[i] = false;
                }
            }
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
        int i = Random.Range(0 , 8);
        ck = false;
        if (d[i] == 1)
        {
            for(int j = 0; j < 8; j++)
            {
                if (d[j] != 1) ck = true;
            }
            if (ck)
            {
                Randoms();
            }
            else
            {
                
            }
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
