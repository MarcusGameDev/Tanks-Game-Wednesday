using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyTankReplacement : MonoBehaviour
{
    public EnemyTankSpawner enemyTankSpawner;
    

    // Start is called before the first frame update
    void Start()
    {
      //  enemyTankSpawner = GameObject.FindGameObjectWithTag("EnemySpawner").GetComponent<EnemyTankSpawner>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            enemyTankSpawner.EnemyTank = this.gameObject;
            enemyTankSpawner.SpawnTank();
            Destroy(this.gameObject);
        }
    }
}
