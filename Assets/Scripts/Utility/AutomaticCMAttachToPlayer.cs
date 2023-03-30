using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutomaticCMAttachToPlayer : MonoBehaviour
{
    // Start is called before the first frame update
    public CinemachineVirtualCamera virtualCamera; 
    void Start()
    {
       virtualCamera = GetComponent<CinemachineVirtualCamera>();
        virtualCamera.Follow = GameObject.FindGameObjectWithTag("PlayerFollowCamera").transform;
        virtualCamera.LookAt = GameObject.FindGameObjectWithTag("PlayerCockpit").transform;
    }

    // Update is called once per frame
    void Update()
    {
        virtualCamera.transform.rotation = virtualCamera.Follow.rotation; 
        
    }
}
