using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testPlayer : MonoBehaviour
{
    #region public field
    public GameObject Hammer_trail;
    public Transform target, curvepoint, fireobj;
    public Transform hammerParent;
    public Rigidbody hammer;
    public Animator anim;
    //int ,float,bool ,vector etc
    public int throwpower;
    [HideInInspector] public bool whenwin;
    [HideInInspector] public Vector3 HammerhitPoint;
    #endregion

    #region private field
    private float time;
    private float disBTw;
    public bool hReturning;

    bool checking = true;
    bool throwing = true;

    private Vector3 oldpos;
    #endregion



    #region monobehaviour callback
    void Update()
    {



        if (Input.GetMouseButtonDown(0))
        {

            if (throwing && !whenwin)
            {
                anim.SetTrigger("Throw");
                Castray();

                StartCoroutine(setTimeforReturningHammer());
                throwing = false;
            }


        }
        if (Input.GetMouseButtonDown(1))
        {
            ReturnHammer();
        }
        if (hReturning)
        {
            if (time < 1f)
            {
                hammer.position = curve(time, oldpos, curvepoint.position, target.position);
                hammer.rotation = Quaternion.Slerp(hammer.transform.rotation, target.rotation, 10 * Time.deltaTime);
                time += Time.deltaTime;
            }
            else
            {
                resethammer();
            }
        }
        //fuction calling
        TrailsofHammer();
    }
    #endregion




    #region private method


    void TrailsofHammer()
    {
        if (hammer.transform.parent == null)
        {
            Hammer_trail.SetActive(true);
        }
        else
        {
            Hammer_trail.SetActive(false);
        }
    }
    void Castray()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray,out hit))
        {
            HammerhitPoint = hit.point;
            fireobj.transform.LookAt(hit.point);
           
           transform.LookAt(hit.point);
            disBTw = Vector3.Distance(hammer.position, hit.point);
           
        }
    }
    void ThrowHammer()
    {

          
            checking = false;
            hReturning = false;
            hammer.transform.parent = null;
           
            hammer.isKinematic = false;
            hammer.velocity = fireobj.transform.forward  * throwpower;
            
            hammer.AddTorque(hammer.transform.TransformDirection(Vector3.up) * 100, ForceMode.Impulse);
       
    }
    void ReturnHammer()
    {
        if (!whenwin)
        {
            anim.SetBool("Pull", true);
        }
        checking = true;
        time = 00f;
        oldpos = hammer.position;
        hReturning = true;
        hammer.velocity = Vector3.zero;
        hammer.isKinematic = true;
       
    }
    void resethammer()
    {
        anim.SetBool("Pull", false);
        hReturning = false;
        hammer.transform.parent = hammerParent;
        hammer.position = target.position;
        hammer.rotation = target.rotation;
        throwing = true;
    }
    Vector3 curve(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        Vector3 p = (uu * p0) + (2 * u * t * p1) + (tt * p2);
        return p;
    }
    IEnumerator setTimeforReturningHammer()
    {
        yield return new WaitForSeconds(1.2f);
         ReturnHammer();
    }
    #endregion

    #region public method
    public void throwsound()
    {
        GameObject.Find("MusicManager").GetComponent<SoundManager>().Playsound.volume = .5f;
        GameObject.Find("MusicManager").GetComponent<SoundManager>().Sound(1);
    }
    #endregion
}
