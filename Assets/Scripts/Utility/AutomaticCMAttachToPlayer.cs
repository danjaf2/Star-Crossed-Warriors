using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AutomaticCMAttachToPlayer : MonoBehaviour
{
    // Start is called before the first frame update
    public CinemachineVirtualCamera virtualCamera; 
    void Start()
    {
       virtualCamera = GetComponent<CinemachineVirtualCamera>();
        try
        {
            //virtualCamera.Follow = GameObject.FindGameObjectWithTag("PlayerFollowCamera").transform;
            //virtualCamera.LookAt = GameObject.FindGameObjectWithTag("PlayerCockpit").transform;
        }
        catch (NullReferenceException ex)
        {
            Debug.Log("Player hasn't been spawned yet");
        }
    }

    // Update is called once per frame
    void Update()
    {
        try
        {
            //virtualCamera.Follow = GameObject.FindGameObjectWithTag("PlayerFollowCamera").transform;
            //virtualCamera.LookAt = GameObject.FindGameObjectWithTag("PlayerCockpit").transform;
        }catch(NullReferenceException ex)
        {
            Debug.Log("Player hasn't been spawned yet");
        }

        //if (virtualCamera.Follow != null)
        {
            //virtualCamera.transform.rotation = virtualCamera.Follow.rotation;
        }

        


        
        
        
    }
}
