using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;

public class TestRelay : MonoBehaviour
{
    // Start is called before the first frame update
    private async void Start()
    {
        await UnityServices.InitializeAsync();


        AuthenticationService.Instance.SignedIn += () =>
         {
             Debug.Log("Signed in "+ AuthenticationService.Instance.PlayerId);
         };
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }

    
    private async void CreateRelay()
    {
        try
        {
            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(1);

            string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
            Debug.Log(joinCode);
        }catch (RelayServiceException e)
        {
            Debug.Log(e);
        }
        
    }
    private async void JoinRelay(string joinCode)
    {
        try
        {
            Debug.Log("Joining Relay with " + joinCode);
            await RelayService.Instance.JoinAllocationAsync(joinCode);
        }catch(RelayServiceException e)
        {
            Debug.Log(e);
        }
       
    }
}
