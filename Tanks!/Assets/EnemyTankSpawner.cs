using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTankSpawner : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject EnemyTank;
    Transform SpawnPoint;


     void Start()
    {
        SpawnPoint = this.gameObject.transform;
        SpawnTank();
    }

    public void SpawnTank()
    {
        GameObject newEnemy = Instantiate(EnemyTank, SpawnPoint.position, SpawnPoint.rotation);
        newEnemy.GetComponent<EnemyTankReplacement>().enemyTankSpawner = this;
    }
}
