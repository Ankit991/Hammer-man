using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public  AudioSource Playsound;
    public  AudioClip[] soundclips;
    // Start is called before the first frame update
   
    public  void Sound(int clipindex)
    {
       Playsound.clip = soundclips[clipindex];
        if (!Playsound.isPlaying)
        {
            Playsound.Play();
        }
       
    }
}
