using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField] public enum AudioCategory { SHOOT, HIT, LOWHP, DEATH, SUCCESS, AIMENGAGE, SPEEDUP, LOWMANA };
public class AudioManager : MonoBehaviour
{
    // Start is called before the first frame update

    
   public List<AudioResource> resources;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void playAudio(AudioCategory toPlay, int id)
    {
        foreach (AudioResource resource in resources)
        {
            if(resource.cat == toPlay && resource.id == id)
            {
                resource.source.Play();

                return; 
            }
        }
    }
}
