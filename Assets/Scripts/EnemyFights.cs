using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFights : MonoBehaviour
{
    public List<GameObject> enemiesToFight;
    public List<GameObject> objectsToDisable;

    void Update()
    {
        if (EnemiesDead())
        {
            DisableGameObjects();
        }
    }

    private bool EnemiesDead()
    {
        for (int i = 0; i < enemiesToFight.Count; i++)
        {
            if (enemiesToFight[i].activeInHierarchy)
            {
                return false;
            }
        }

        return true;
    }

    private void DisableGameObjects()
    {
        for (int j = 0; j < objectsToDisable.Count; j++)
        {
            objectsToDisable[j].SetActive(false);
        }
    }
}
