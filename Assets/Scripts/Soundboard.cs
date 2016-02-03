using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Soundboard : MonoBehaviour {
    
    public static Soundboard s;    
    Dictionary<string, AudioSource> sources;
    
    void Start(){
        s = this;
        sources = new Dictionary<string, AudioSource>();
        for(int i = 0; i < transform.childCount; i++){            
            AudioSource ass = transform.GetChild(i).GetComponent<AudioSource>();             
            sources.Add(ass.name, ass);
        }
    }
    
    public void PlaySound(string assname){
        PlayPitchedSound(assname, 1,1);
    }
    
    public void PlayPitchedSound(string assname, float minPitch, float maxPitch){
        AudioSource ass = new AudioSource();
        sources.TryGetValue(assname, out ass);
        if(ass!=null){
            ass.Play();            
        } else {
            Debug.LogError("invalid assname");
        }
    }

    
}
