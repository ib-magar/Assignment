using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SlicedPiece : MonoBehaviour
{
    public float destroyTime;
    public GameObject destroyEffect;
    private IEnumerator Start()
    {
        destroyEffect = Resources.Load("ParticleEffect") as GameObject ;
      //  if(!transform.CompareTag("MainObject"))
        {
            GetComponent<Rigidbody2D>().isKinematic = false;
            yield return new WaitForSeconds(destroyTime > 0 ? destroyTime : 1.5f);
            //Instantiate(destroyEffect,transform.position, Quaternion.identity); 
            Destroy(gameObject);

        }
      /*  else
        {
            GetComponent<Rigidbody2D>().isKinematic=true;
        }*/
    }

}
