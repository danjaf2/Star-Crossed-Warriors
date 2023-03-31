using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class Scout: PlayerShip
{
    public Scout()
    {
        maxHealth = 150;
        maxEnergy = 300;
        speed = 2;
        primaryFireRate = 7;
        lockOnRate = 100;
       
}
}

public class Demoman : PlayerShip
{
    public Demoman()
    {
        maxHealth = 225;
        maxEnergy = 300;
        speed = 1.0f;
        primaryFireRate = 5;
        lockOnRate = 100;
        
    }
}

public class Heavy : PlayerShip
{
    public Heavy()
    {
        maxHealth = 350;
        maxEnergy = 400;
        speed = 0.5f;
        primaryFireRate = 5;
        lockOnRate = 100;
        
    }
}

public enum ClassType { SCOUT, DEMO, HEAVY, NONE}; 
public class PlayerManager : MonoBehaviour
{
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
            playerClass = new Demoman(); 
        }
        else if (type == ClassType.SCOUT)
        {
            playerClass = new Scout();
        }
        else if (type == ClassType.HEAVY)
        {
            playerClass = new Heavy();
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
