using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickObject : MonoBehaviour
{
    // Start is called before the first frame update
    Manger m;
    private AudioSource obj;
    public int number;
    void Start()
    {
        obj = GetComponent<AudioSource>();
        m = GameObject.Find("Main Camera").GetComponent<Manger>();
    }
    public void OnMouseDown()
    {
        if(m.checkClickOBJ)
            obj.PlayOneShot(m.audioClips[number], 1f);
    }
    
}
