using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using EZCameraShake;
using UnityEngine.SceneManagement;
public class Playercontroller : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform firepos;
    public GameObject Fireobj;
    public Transform hammer;
    public Quaternion hampos;
    public Vector3 Ham_startpos;
    Rigidbody hamrig;
    public LayerMask Ground;
           float dis;
    public Vector3 lastpos;//hammer last position;
    public bool Isground;
    public float firerate = 0.5f;
           float nextfire;
    Animator anim;
    public float Explosionrange,explosionradius;
    Vector3 explosionpos;
   
    void Awake()
    {
        hamrig = Fireobj.GetComponent<Rigidbody>();
        // hamrig.isKinematic = true;
        Ham_startpos = Fireobj.transform.position;
        hampos = hammer.rotation;
        anim = GetComponent<Animator>();
       
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Physics.CheckSphere(Fireobj.transform.position, .3f, Ground)){
            Isground = true;
        }
        if (Input.GetMouseButtonDown(0))
        {

            fire();

        }
        if (Input.GetMouseButtonUp(0))
        {
           
        }
        StartCoroutine(hammerback());
        if (!Isground&& Fireobj.transform.position.z>-2f)
        {
            hammer.transform.Rotate(Vector3.down * 10);
        }
        else
        {
            hammer.transform.rotation = hampos;
        }
       
    }
    public void fire()
    {
        if (Time.time > nextfire)
        {
            nextfire = firerate + Time.time;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (!Isground)
            {
                if (Physics.Raycast(ray, out hit,Ground))
                {
                    Fireobj.transform.LookAt(hit.point);
                    dis = Vector3.Distance(firepos.position, hit.point);
                    explosionpos = hit.point;        //Getting position for explosion;

                    Debug.DrawLine(Ham_startpos, explosionpos, Color.red, 1f);
                     // hamrig.velocity = Fireobj.transform.forward*dis * 10;
                     hamrig.AddRelativeForce(Fireobj.transform.forward * dis * 200, ForceMode.Acceleration);
                        anim.SetTrigger("throw");


                }
            }
        }
      
    }
    IEnumerator hammerback()
    {
        if (Isground)
        {
            Collider[] collider = Physics.OverlapSphere(explosionpos, explosionradius);
            foreach (Collider hit in collider)
            {
                Rigidbody rb = hit.GetComponent<Rigidbody>();

                if (rb != null)
                    rb.AddExplosionForce(Explosionrange, explosionpos, explosionradius, .1F,ForceMode.Impulse);
            }
            hamrig.isKinematic = true;
            yield return new WaitForSeconds(1f);
            Fireobj.transform.DOMove(Ham_startpos+new Vector3(0,.01f,0), .2f);
            Quaternion rot= Quaternion.Euler(Fireobj.transform.position.x, 0, Fireobj.transform.position.z);
            Fireobj.transform.rotation = rot;
           
           
            Isground = false;
            anim.SetFloat("Picking", 1);
            if (!Isground && Fireobj.transform.position.z < -2f)
            {
                anim.SetFloat("Picking", 0);
                hamrig.isKinematic = false;

            }

        }
    }
   public void Retart()
    {
        SceneManager.LoadScene(0);
    }
   
}
