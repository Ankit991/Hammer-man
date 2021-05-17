 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hamcontroll : MonoBehaviour
{
    public GameObject DrumBlast;
    public GameObject hitparticle;
   
   
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != "Drum"&&collision.gameObject.tag!="Player"&&collision.gameObject.tag!="Ground")
        {
            GameObject hamhit = Instantiate(hitparticle, collision.transform.position, Quaternion.identity);
            Destroy(hamhit, 3f);
        }
        if (collision.gameObject.tag == "Drum")
        {
            Vector3 drumpos = collision.gameObject.transform.position;
            Destroy(collision.gameObject);
           GameObject Blast= Instantiate(DrumBlast, drumpos, Quaternion.Euler(0, 0, 0));
            Destroy(Blast, 4f);
        }
         //  GetComponent<Rigidbody>().isKinematic = true;
        

    }
}
