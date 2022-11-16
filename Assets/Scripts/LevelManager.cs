using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [Header("Basic Stats")]
    public int lifes = 3;
    public int level;
    public bool isDead;

    [Header("PlayerLifes")]
    //todo makes this more advanced looking
    public GameObject life1, life2, life3; 
    

    public void LoseLife()
    {
        if(lifes == 0)
            return;
        
        lifes -= 1;
        switch (lifes)
        {
            case 2:
                Destroy(life3);
                break;
            case 1:
                Destroy(life2);
                break;
            case 0:
                Destroy(life1);
                break;
        }
    }
}
