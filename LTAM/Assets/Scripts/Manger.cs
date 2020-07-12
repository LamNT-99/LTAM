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
    private int index;
    private float timeCameraMoveRot= 0.05f;
    private float timeCameraMovePos= 0.5f;
    private float timeperFrame= 0.05f;
    private float speed = 2f;

    //source
    private AudioSource audioSource;
    public List<Sprite> FULLHP;
    public List<AudioClip> audioClips;

    //UI
    public GameObject UI, UiEnd, UIStart, UIHand, UIStatus, UIHelp;
    public Text pointText, pointEnd, textWorL, timeText, results, textEnd ,noteText;
    public Text[] answerText = new Text[3];
    public Image image;

    //Check
    private float point = 0, time = 100f, timeMore;
    bool start = false, checkRandom = false, rotateCamera = false, closeUILoss = false;
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

    //Update
    void Update()
    {
        if (rotateCamera)
        {
            RotateCamera();
        }
        
    }

    private void RotateCamera()
    {
        transform.Rotate(Vector3.up, Input.GetAxis("Mouse X") * speed);
        transform.Rotate(Vector3.right, -Input.GetAxis("Mouse Y") * speed);

        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0);
        transform.Translate(Vector3.right * Input.GetAxis("Horizontal") * speed * Time.deltaTime * 2f);
        transform.Translate(Vector3.forward * Input.GetAxis("Vertical") * speed * Time.deltaTime * 2f);
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        if (transform.position.z >= 18f) transform.position = new Vector3(transform.position.x, transform.position.y, 18f);
        if (transform.position.z <= 5f) transform.position = new Vector3(transform.position.x, transform.position.y, 5f);
        if (transform.position.x >= 23f) transform.position = new Vector3(23f, transform.position.y, transform.position.z);
        if (transform.position.x <= 8f) transform.position = new Vector3(8f, transform.position.y, transform.position.z);
    }

    //Update 
    void FixedUpdate()
    {
        if (!closeUILoss) GameLoss();
        else UI.SetActive(false);
        Handle();
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
            audioSource.PlayOneShot(audioClips[7], 1f);
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
                        StartCoroutine(SetActiveUI(5f, UI, true));
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
            UiEnd.SetActive(true);
            audioSource.PlayOneShot(audioClips[3], 1f);
            StartCoroutine(wait(4, 2));
            textWorL.text = "LOSE";
            textEnd.text = "Hết thời gian !!!" + "\n => Bạn ngu VC";
            pointEnd.text = "Point : " + point;
            pointEnd.color = Color.blue;
            start = false;
            UI.SetActive(false);
            rotateCamera = true;
            StartCoroutine(SetActiveUI(7, UiEnd, false));
        }
    }

    //Game Winner
    public void GameWinner()
    {
        UiEnd.SetActive(true);
        start = false;
        rotateCamera = true;
        closeUILoss = true;
        pointEnd.text = "";
        if (Time.time <= 100f)
        {
            textWorL.text = "WINNER!!!";
            textEnd.text = "Tổng thời gian trả lời của bạn : " + Time.time + "s" + "\n => Bạn giỏi VC";
            pointEnd.text = "Point : " + point;
            pointEnd.color = Color.blue;
            StartCoroutine(SetActiveUI(7, UiEnd, false));
        }
        noteText.text = "You can view rooms by pressing key A, D, W, S to move";
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

    //display UI before time
    IEnumerator SetActiveUI(float time,GameObject UI ,bool check)
    {
        yield return new WaitForSeconds(time);
        UI.SetActive(check);
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
