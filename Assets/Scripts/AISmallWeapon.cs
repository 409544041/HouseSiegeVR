using UnityEngine;
using Random = UnityEngine.Random;

public class AISmallWeapon : MonoBehaviour
{
    [Header("Random Number Generator 1 = hit")]
    public float oddsHigh, oddsLow;

    public GameObject LevelManager;

    private void Start()
    {
        LevelManager = GameObject.FindGameObjectWithTag("LevelManager");
    }

        //Simple RayCastShooting Script with hopefully cover working;
        public void Shoot()
        {

                    AudioManager.instance.Play("Shot");
                    var i =  Random.Range(oddsLow, oddsHigh);
                    if (i >= 1)
                    { 
                        Debug.Log("I hit the player yay");
                        LevelManager.GetComponent<LevelManager>().LoseLife();
                    }
                    else
                    {
                        Debug.Log("Missed");
                    }
            
                    /*Ray ray = new Ray(transform.position, transform.forward);
                    RaycastHit hitData;
                    if (Physics.Raycast(ray, out hitData, 50f))
                    {
                        Debug.Log(hitData.transform.name);

                        if (hitData.transform.gameObject.CompareTag("Player"))
                        {
                            ani.SetBool("ReadyArm", true);
                            if (!(Time.time > NextShootTime))
                                return;
                            Weapon.Shoot();
                            NextShootTime = Time.time + Random.Range(fireRateMin, fireRateMax);
                        }
                    }*/
                    
        }
}
