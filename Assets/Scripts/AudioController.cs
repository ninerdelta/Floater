﻿using UnityEngine;
using System.Collections;
using UnityEngine.Audio;

public class AudioController : MonoBehaviour {

    public AudioSource songSource;
    public GameObject grid;
    private States state;
    private float[] spectrumL = new float[512];
    private float[] spectrumR = new float[512];
    private int length = 324;

    private enum States
    {
        Stopped,
        Playing,
        Paused
    };


	// Use this for initialization
	void Start () {
        state = States.Stopped;
        grid = GameObject.Find("GridParent");
    }
	
	// Update is called once per frame
	void Update () {
        int i = 64;
        int blockIndex = 19;
        if(state == States.Playing)
        {           
            songSource.GetSpectrumData(spectrumL, 0, FFTWindow.BlackmanHarris);
            songSource.GetSpectrumData(spectrumR, 1, FFTWindow.BlackmanHarris);

            while (blockIndex < length - 1)
            {
                if(blockIndex > 18 && blockIndex < 305)
                {
                    if(blockIndex%18 !=0 && (blockIndex + 1)%18 != 0)
                    {
                        Transform block = grid.transform.GetChild((blockIndex));
                        float scale = Mathf.Abs(Mathf.Log((spectrumL[i-1] + spectrumR[i-1])/2,2.0f));
                        if (scale != Mathf.Infinity)
                        {
                            block.localScale = new Vector3(1.0f, 1.0f, scale);
                        }
                        i++;
                    }
                }
                blockIndex++;
            }
        }
	}

    private IEnumerator startupPause()
    {
        yield return new WaitForSeconds(1.0f);
        
    }

    public void playSong()
    {
        if(state == States.Stopped)
        {
            songSource.volume = 0.10f;
            songSource.Play();
            state = States.Playing;            
        }   
        else if(state == States.Playing)
        {
            songSource.Stop();
            state = States.Stopped;
        }
    }
}
