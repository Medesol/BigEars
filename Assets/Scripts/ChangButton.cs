using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangButton : MonoBehaviour
{
	public Sprite pencil;
	public Sprite erase;
	public int mode = 1; //1 for drawing; 0 for erasing;

    public AudioSource changeSound;//change sound;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void changeButton() {
	    Debug.Log("Change Button!");
        changeSound.Play();
        if (mode == 1) {
    		mode = 0;     
    		GetComponent<Image>().sprite = erase;
    	} else if (mode == 0) {
    		mode = 1;
    		GetComponent<Image>().sprite = pencil;
    		Debug.Log("change button to erase");
    	}
    }
}
