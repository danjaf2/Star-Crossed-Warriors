using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerManager : MonoBehaviour
{
    public enum ClassType { SCOUT, DEMO, HEAVY, NONE };

    // Start is called before the first frame update
    [SerializeField] public GameObject vehicle;
    [SerializeField] public float currentEnergyPercentage = 100;

    [SerializeField] public ClassType type;
    [SerializeField] public PlayerShip playerClass;

    //[SerializeField] public List<GameObject> specialAbilityPrefabs;

    [SerializeField] public List<GameObject> vehicles;

    void Start()
    {
        vehicle = GameObject.FindGameObjectWithTag("Plane");

        if(type == ClassType.DEMO)
        {
            playerClass = new DemoShip(); 
        }
        else if (type == ClassType.SCOUT)
        {
            playerClass = new ScoutShip();
        }
        else if (type == ClassType.HEAVY)
        {
            playerClass = new HeavyShip();
        }
        else
        {
            playerClass = null; 
        }

        vehicles[(int)type].SetActive(true); //Set correct vehicle to active
        
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
