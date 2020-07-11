using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;
using Image = UnityEngine.UI.Image;
using UnityEngine.UI;

public class Manger : MonoBehaviour
{
    //API
    public string[] keywords = new string[] { };
    public ConfidenceLevel confidence = ConfidenceLevel.Medium;
    PhraseRecognizer recognizer;
    public string word = "";

    //pos
    public GameObject[] objTarget;
    public int index;
    public float timeCameraMoveRot;
    public float timeCameraMovePos;
    public float timeperFrame;

    //source
    private AudioSource audioSource;
    public List<Sprite> FULLHP;
    public List<AudioClip> audioClips;

    //UI
    public GameObject UI, UiEnd, UIStart, UIHand, UIStatus, UIHelp;
    public Text pointText, pointEnd, textWorL, timeText, results, textEnd;
    public Text[] answerText = new Text[3];
    public Image image;

    //Check
    private float point = 0, time = 100f, timeMore;
    bool start = false, checkRandom = false;
    int[] checkQuestion = new int[10];

    //Answer Yes and No
    string[] keyWordsY = new string[] { "Door", "Curtain", "Bed", "Book", "Sofa", "Phone", "Tivi", "Table Work" };
    string[] keyWordsN = new string[] { "Money", "Dola", "Gold", "Silver", "Diamond", "Fridge","Laptop",
        "Pot","Tree","Dog","Cat","Roasted Chicken"};




    //API
    private void Recognizer_OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        word = args.text;
        results.text = "You said: <b>" + word + "</b>";
    }
    private void OnApplicationQuit()
    {
        if (recognizer != null && recognizer.IsRunning)
        {
            recognizer.OnPhraseRecognized -= Recognizer_OnPhraseRecognized;
            recognizer.Stop();
        }
    }

    //start 
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (keywords != null)
        {
            recognizer = new KeywordRecognizer(keywords, confidence);
            recognizer.OnPhraseRecognized += Recognizer_OnPhraseRecognized;
            recognizer.Start();
        }

        //UI start
        Randoms();
        pointText.text = "Point : 0";
        UiEnd.SetActive(false);
        UIStatus.SetActive(false);
    }

<<<<<<< HEAD
    //Update
    private void Update()
=======
    public Camera cameraObj;
    public float speed = 2f;

    void Update()
>>>>>>> 3c668d5abe298bafddfe18d5a172db7668f0d4de
    {
        RotateCamera();
    }

<<<<<<< HEAD
    private float speed = 2f;
    private void RotateCamera()
    {
        transform.Rotate(Vector3.up, Input.GetAxis("Mouse X") * speed);
        transform.Rotate(Vector3.right, -Input.GetAxis("Mouse Y") * speed);

        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0);
=======
    void RotateCamera()
    {
        //if (Input.GetMouseButton(0))
        {
            cameraObj.transform.Rotate(
                                 Vector3.up,
                                            Input.GetAxis("Mouse X") * speed);
            cameraObj.transform.Rotate(
                                          Vector3.right,
                                            -Input.GetAxis("Mouse Y") * speed);

            cameraObj.transform.eulerAngles = new Vector3(cameraObj.transform.eulerAngles.x , cameraObj.transform.eulerAngles.y ,0); 
        }
>>>>>>> 3c668d5abe298bafddfe18d5a172db7668f0d4de
    }

    //Update 
    void FixedUpdate()
    {
        Handle();
        GameLoss();
        TimeEndGame();
        PeOrRo();

        // exit Game
        if (word == "Quit")
        {
            CloseGame();
        }
        pointText.text = "Point : " + point;
    }

    //Check is people or robot
    public void PeOrRo()
    {
        if (word == "Super Handsome")
        {
            UIHand.SetActive(false);
            UIStatus.SetActive(true);
            timeMore = Time.time;
            StartCoroutine(wait(2, 1f));
            start = true;
            word = "";
        }
        if (word == "Handsome" || word == "Yes")
        {
            int exit = 0;
            exit++;
            if (exit == 2)
            {
                CloseGame();
            }
            word = "";
        }
    }

    //Handle the main game
    public void Handle()
    {
        if (start)
        {
            for (int i = 0; i < 11; i++)
            {
                if (i < 8)
                {
                    if (word == keyWordsY[i])
                    {
                        index = i;
                        StartCoroutine(SlowlyMovePos());
                        StartCoroutine(wait(5, 0));
                        StartCoroutine(wait(2, 6));
                        UI.SetActive(false);
                        StartCoroutine(SetActiveUI());
                        Randoms();
                        if (Time.time <= 50f)
                        {
                            point += 5;
                        }
                        else point += 3;
                        word = "";
                    }
                }
                if (word == keyWordsN[i])
                {
                    StartCoroutine(wait(6, 0));
                    point -= 2;
                    word = "";
                }
            }
        }
    }

    //Game Loss
    public void GameLoss()
    {
        if (Time.time == (100f + Mathf.Round(timeMore)))
        {
            UI.SetActive(false);
            UiEnd.SetActive(true);
            audioSource.PlayOneShot(audioClips[3], 1f);
            StartCoroutine(wait(4, 2));
            textWorL.text = "LOSE";
            textEnd.text = "Hết thời gian !!!" + "\n => Bạn ngu VC";
            pointEnd.text = "Point : " + point;
            pointEnd.color = Color.blue;
            start = false;
        }
    }

    //Game Winner
    public void GameWinner()
    {
        UI.SetActive(false);
        UiEnd.SetActive(true);
        start = false;
        pointEnd.text = "";
        if (Time.time <= 100f)
        {
            textWorL.text = "WINNER!!!";
            textEnd.text = "Tổng thời gian trả lời của bạn : " + Time.time + "s" + "\n => Bạn giỏi VC";
            pointEnd.text = "Point : " + point;
            pointEnd.color = Color.blue;
        }
    }

    //Time end game
    public void TimeEndGame()
    {
        if (time >= 0 && start == true)
        {
            time -= 0.02f;
            timeText.text = "Time : " + Mathf.Round(time) + " s";
        }
        else timeText.text = "Time : 0 s";
    }

    //Move the camera to the object
    IEnumerator SlowlyMovePos()
    {
        float time = 0f;
        while (transform.position != objTarget[index].transform.position)
        {
            yield return new WaitForSeconds(timeperFrame);

            transform.position = Vector3.MoveTowards(transform.position, objTarget[index].transform.position, timeCameraMovePos);
            transform.rotation = Quaternion.Slerp(transform.rotation, objTarget[index].transform.rotation, time);
            time += timeCameraMoveRot;
        }
    }

    // random number question
    public void Randoms()
    {
        int i = Random.Range(0, 8);
        checkRandom = false;
        if (checkQuestion[i] == 1)
        {
            for (int j = 0; j < 8; j++)
            {
                if (checkQuestion[j] != 1) checkRandom = true;
            }
            if (checkRandom)
            {
                Randoms();
            }
            else
            {
                GameWinner();
            }
        }
        else
        {
            image.sprite = FULLHP[i];
            RandomAnswer(i);
            checkQuestion[i] = 1;
        }
    }

    //display UI before 5s
    IEnumerator SetActiveUI()
    {
        yield return new WaitForSeconds(5f);
        UI.SetActive(true);
    }

    //Random Answer
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

    //click button Play(UI start)
    public void buttonPlay()
    {
        UIStart.SetActive(false);
        UIHand.SetActive(true);
        audioSource.PlayOneShot(audioClips[0], 1f);
        StartCoroutine(wait(1, 2));
    }

    // click button Help(UI start)
    public void buttonHelp()
    {
        UIHelp.SetActive(true);
    }

    //exit UI Help
    public void closeHelp()
    {
        UIHelp.SetActive(false);
    }

    //Quit game
    public void CloseGame()
    {
        Application.Quit();
    }

    //wait play audio clip
    IEnumerator wait(int i, float j)
    {
        yield return new WaitForSeconds(j);
        audioSource.PlayOneShot(audioClips[i], 1f);
    }
}
