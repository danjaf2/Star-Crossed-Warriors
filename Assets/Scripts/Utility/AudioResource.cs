using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioResource : MonoBehaviour
{
    [SerializeField] public AudioSource source;
    [SerializeField] public AudioCategory cat;
    [SerializeField] public int id;
    // Start is called before the first frame update
    void Start()
    {
        source = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
