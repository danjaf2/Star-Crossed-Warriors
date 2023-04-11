using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.UIElements.ToolbarMenu;

public class MineField : Entity
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Entity>(out var entity))
        {
            gameObject.GetComponent<Bullet>().PerformActionOnCollision(BulletVariant.MINE); 
        }

        // Destroy the bullet since it hit something.
        if (other.gameObject.layer == 3)
        {
            gameObject.GetComponent<Bullet>().PerformActionOnCollision(BulletVariant.MINE);
            //Destroy(this.gameObject);
        }
    }

    protected override void OnDeath()
    {
        gameObject.GetComponent<Bullet>().Explode();

        Destroy(this.gameObject, 0.01f);



    
    }


}
