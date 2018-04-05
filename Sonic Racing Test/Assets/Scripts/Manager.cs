using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public GameObject player;
    private CharacterControl controlScript;

    public bool countedDown = false;

    AudioSource ready;
    AudioSource set;
    AudioSource go;
    AudioSource bgm;
    // Use this for initialization
    void Start ()
    {
        controlScript = player.GetComponent<CharacterControl>();
        controlScript.enabled = false;

        AudioSource[] sounds = GetComponents<AudioSource>();
        ready = sounds[0];
        set = sounds[1];
        go = sounds[2];
        bgm = sounds[3];

        ready.Play();
        set.PlayDelayed(1f);
        go.PlayDelayed(2f);
        bgm.PlayDelayed(3f);
    }
	
	// Update is called once per frame
	void Update ()
    {
		if (countedDown)
        {
            controlScript.enabled = true;
            
        }
	}
}
