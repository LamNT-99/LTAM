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
    //API
    public string[] keywords = new string[] { };
    public ConfidenceLevel confidence = ConfidenceLevel.Medium;
    PhraseRecognizer recognizer;
    public string word = "";

    //pos
    public GameObject[] objTarget;
    private int index, indexAuto;
    private float timeCameraMoveRot= 0.05f;
    private float timeCameraMovePos= 0.5f;
    private float timeperFrame= 0.05f;
    private float speed = 2f;

    //source
    private AudioSource audioSource;
    public List<AudioClip> audioClips;

    //UI
    public GameObject UI, UiEnd, UIStart, UIHand, UIStatus, UIHelp ,UIChoose, UIAdd, UIConver;
    public Text pointText, pointEnd, textWorL, timeText, results, textEnd ,noteText;
    public Text[] answerText = new Text[3];
    public Image image;
    public List<Sprite> FULLHP;

    //Check
    private float point = 0, time = 100f, timeMore, exit = 0;
    bool start = false, checkRandom = false, rotateCamera = false, closeUILoss = false, autoMove = false, autoFinish = false, closeHand = false;
    int[] checkQuestion = new int[10];
    public bool checkClickOBJ = false;

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
        pointText.text = " 0 ";
        UiEnd.SetActive(false);
        UIStatus.SetActive(false);
        UI.SetActive(false);
        UIChoose.SetActive(false);
        UIAdd.SetActive(false);
        UIConver.SetActive(false);
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
        if (transform.position.x <= 5f) transform.position = new Vector3(5f, transform.position.y, transform.position.z);
    }

    //Update 
    void FixedUpdate()
    {
        if (!closeUILoss)
            GameLoss();
        else
        {
            UI.SetActive(false);
            StartCoroutine(SetActiveUI(7, UiEnd, false));
        }
        Handle();
        TimeEndGame();
        PeOrRo();

        // exit Game
        if (word == "Quit")
        {
            CloseGame();
        }
        pointText.text = " " + point;
        AutoMove();
        CheckAutoAW();
    }

    //Check is people or robot
    public void PeOrRo()
    {
        if (word == "Super Handsome" && !closeHand)
        {
            UIHand.SetActive(false);
            UIChoose.SetActive(true);
            //UIStatus.SetActive(true);
            //timeMore = Time.time;
            //StartCoroutine(wait(2, 1f));
            //start = true;
            StartCoroutine(wait(8, 1f));
            closeHand = true;
            word = "";
        }
        if ((word == "Handsome" || word == "Yes") && !closeHand)
        {
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
                        StartCoroutine(SlowlyMovePos(index));
                        if (!closeUILoss)
                        {
                            StartCoroutine(wait(5, 0));
                            StartCoroutine(wait(2, 6));
                        }
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
        if (Time.time == (100f + Mathf.Round(timeMore)) && start == true)
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
            //rotateCamera = true;
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
        textWorL.text = "WINNER!!!";
        textEnd.text = "Tổng thời gian trả lời của bạn : " + (Time.time-timeMore) + "s" + "\n => Bạn giỏi VC";
        pointEnd.text = "Point : " + point;
        pointEnd.color = Color.blue;
        StartCoroutine(SetActiveUI(7, UiEnd, false));
        noteText.text = "You can view rooms by pressing key A, D, W, S to move";
        StopAllCoroutines();
        checkClickOBJ = true;
    }

    //Time end game
    public void TimeEndGame()
    {
        if (time >= 0 && start == true)
        {
            time -= 0.02f;
            timeText.text = " " + Mathf.Round(time) + " s";
        }
        else timeText.text = " 0 s ";
    }

    //Move the camera to the object
    IEnumerator SlowlyMovePos(int index)
    {
        float time = 0f;
        while (transform.position != objTarget[index].transform.position || transform.rotation != objTarget[index].transform.rotation)
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
    IEnumerator wait(int audio, float time)
    {
        yield return new WaitForSeconds(time);
        audioSource.PlayOneShot(audioClips[audio], 1f);
    }

    //Question mode selection
    public void ButtonQuestion()
    {
        start = true;
        UI.SetActive(true);
        UIStatus.SetActive(true);
        UIAdd.SetActive(true);
        UIChoose.SetActive(false);
        timeMore = Time.time;
        StartCoroutine(wait(2, 1f));
        noteText.text = "Your task is to answer questions to move to objects!";
    }

    //Auto mode selection
    public void ButtonAuto()
    {
        StopAllCoroutines();
        UIChoose.SetActive(false);
        UI.SetActive(false);
        autoMove = true;
        start = false;
        UIAdd.SetActive(true);
        UIConver.SetActive(false);
        UIStatus.SetActive(false);
        indexAuto = 0;
    } 

    IEnumerator Auto(float time)
    {
        yield return new WaitForSeconds(time);
        indexAuto++;
        autoMove = true;
    }

    // auto move camera to object.
    public void AutoMove()
    {
        if (autoMove)
        {
            if (indexAuto >= 8)
            {
                rotateCamera = true;
                autoMove = false;
                StopAllCoroutines();
                autoFinish = true;
                checkClickOBJ = true;
                noteText.text = "You can say the name of the item or press the key A,D,W,S to move to the object";
            }
            else
            {
                StartCoroutine(SlowlyMovePos(indexAuto));
                StartCoroutine(wait(indexAuto+9, 1f));
                StartCoroutine(Auto(6f));
                autoMove = false;
            }
            
        }
    }

    //voice control after auto mode
    public void CheckAutoAW()
    {
        if (autoFinish)
        {
            for(int i = 0; i < 8; i++)
            {
                if (word == keyWordsY[i])
                {
                    StartCoroutine(SlowlyMovePos(i));
                    StartCoroutine(wait(i + 9, 1f));
                    word = "";
                }
            }
        }
        
    }

    public void clickAdd()
    {
        UIConver.SetActive(true);
    }
    public void CloseAdd()
    {
        UIConver.SetActive(false);
    }
}
