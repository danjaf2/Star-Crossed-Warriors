using Cinemachine;
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public enum ClassType { SCOUT, DEMO, HEAVY, NONE };
public class PlayerManager : NetworkBehaviour
{
   

    // Start is called before the first frame update
    [SerializeField] public GameObject vehicle;
    [SerializeField] public float currentEnergyPercentage = 100;

    [SerializeField] public ClassType type;
    [SerializeField] public PlayerShip playerClass;

    //[SerializeField] public List<GameObject> specialAbilityPrefabs;

    [SerializeField] public List<GameObject> vehicles;

    public CinemachineVirtualCamera virtualCamera;

    public GameObject dCamF;
    public GameObject dCamL;
    public GameObject sCamF;
    public GameObject sCamL;
    public GameObject hCamF;
    public GameObject hCamL;

    public override void OnNetworkSpawn()
    { // This is basically a Start method
        if (IsOwner)
        {
            virtualCamera.gameObject.SetActive(true);
            type = GameObject.FindObjectOfType<NetworkManagerUI>().type;
        }
        vehicle = GameObject.FindGameObjectWithTag("Plane");

        if (type == ClassType.DEMO)
        {
            if (IsOwner)
            {
                virtualCamera.LookAt = dCamL.transform;
                virtualCamera.Follow = dCamF.transform;
            }
            playerClass = new DemoShip();
        }
        else if (type == ClassType.SCOUT)
        {
            if (IsOwner)
            {
                virtualCamera.LookAt = sCamL.transform;
                virtualCamera.Follow = sCamF.transform;
            }
            playerClass = new ScoutShip();
        }
        else if (type == ClassType.HEAVY)
        {
            if (IsOwner)
            {
                virtualCamera.LookAt = hCamL.transform;
                virtualCamera.Follow = hCamF.transform;
            }
            playerClass = new HeavyShip();
        }
        else
        {
            playerClass = null;
        }

        vehicles[(int)type].SetActive(true); //Set correct vehicle to active

        playerClass = vehicles[(int)type].GetComponent<PlayerShip>();
        base.OnNetworkSpawn(); // Not sure if this is needed though, but good to have it.
    }
    void Start()
    { 

        


    }

    public void AdjustVehicleBasedOnType()
    {
        //Add code for speed adjustment here
    }

    // Update is called once per frame
    void Update()
    {
        //transform.position = vehicle.transform.position;
        //transform.rotation = vehicle.transform.rotation;
    }


    // Étienne: I don't think these will be necessary. They already exist on playeship class.
    public void Recharge(float amount)
    {
        currentEnergyPercentage += amount; 

        if(currentEnergyPercentage > 100)
        {
            currentEnergyPercentage = 100;
        }
    }

    public void Decharge(float amount)
    {
        currentEnergyPercentage -= amount;

        if (currentEnergyPercentage < 0)
        {
            currentEnergyPercentage = 0;
        }
    }
}
