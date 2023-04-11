using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using Unity.Netcode.Transports.UTP;
using QFSW.QC;
using Unity.Networking.Transport.Relay;
using Unity.Netcode;

public class TestRelay : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] public GameObject characterSelectPanel;
    private async void Start()
    {
        await UnityServices.InitializeAsync();


        AuthenticationService.Instance.SignedIn += () =>
         {
             Debug.Log("Signed in "+ AuthenticationService.Instance.PlayerId);
         };
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }

    [Command]
    private async void CreateRelay()
    {
        try
        {
            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(1);

            string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);


            Debug.Log("**********************************************JOIN CODE IS "+joinCode);

            RelayServerData relayServerData = new RelayServerData(allocation, "dtls");
            characterSelectPanel.SetActive(false);
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);
            NetworkManager.Singleton.StartHost();
        }catch (RelayServiceException e)
        {
            Debug.Log(e);
        }
        
    }
    [Command]
    private async void JoinRelay(string joinCode)
    {
        try
        {
            Debug.Log("Joining Relay with " + joinCode);
            JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(joinCode);
            RelayServerData relayServerData = new RelayServerData(joinAllocation, "dtls");

            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);
            characterSelectPanel.SetActive(false);
            NetworkManager.Singleton.StartClient();

            while (!NetworkManager.Singleton.IsConnectedClient)
            {
                //Wait
            }
        }
        catch(RelayServiceException e)
        {
            Debug.Log(e);
        }
       
    }
}
