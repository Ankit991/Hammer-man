using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    #region public field
    public GameObject[] nodefollow;
    public Transform player;
    public Rigidbody Erb;
    public Animator Eanim;
    public Rigidbody[] rigit;
    public Collider Enmcollider;
    public Collider[] co;
    public Material deadmaterial;
    //int ,float,bool,vector etc;
    public int nodeindex;
    public int MovementSpeed = 200;
    public bool Dead;
    public bool stopmoving;
    #endregion

    #region private field

    bool Iswalk;
    #endregion

   // srcipt reference
    UIManager uimanager;

    #region monobehaviour callback
    // Start is called before the first frame update


    void Start()
    {
        turnOffRagdoll();
      
        uimanager = GameObject.Find("UImanager").GetComponent<UIManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, nodefollow[nodeindex].transform.position) < 2)
        {
            if (nodeindex < 5)
            {
                nodeindex++;
            }
           
        }
            transform.rotation = Quaternion.LookRotation(nodefollow[nodeindex].transform.position - transform.position);
        if (Vector3.Distance(transform.position, nodefollow[5].transform.position) < 1f)
        {
            Eanim.SetTrigger("Punch");

            stopmoving = true;
            Iswalk = false;
            // Dead fuction calling;
            uimanager.Dead(true);
        }
        if (!uimanager.gameover)
        {
            if (!Dead)
            {
                if (Vector3.Distance(transform.position, player.position) > .5f)
                {
                    Eanim.SetFloat("walk", 1);

                }
                if (!stopmoving&&Iswalk)
                {
                    Erb.AddRelativeForce(Vector3.forward * MovementSpeed);

                }
            }
        }
        else
        {
            Eanim.SetBool("Over", true);
        }

        if (Input.GetMouseButtonDown(0))
        {
            Iswalk = true;
            Eanim.SetTrigger("Walk");
        }


    }
  
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Hammer"))
        {
            stopmoving = true;
            this.gameObject.GetComponentInChildren<SkinnedMeshRenderer>().material = deadmaterial;  //Assigning Dead material to Enemy
            Dead = true;

            uimanager.Deadpoint++;
            Eanim.SetFloat("walk", 0);
            turnonRagdoll();
            Destroy(this.gameObject, 4f);
        }
    }
    #endregion
    #region Ragdoll ON/Off
    public void turnOffRagdoll()
    {
       
        for (int i = 0; i < co.Length; i++)
        {
            co[i].enabled = false;
        }
        for (int i = 0; i < rigit.Length; i++)
        {
            rigit[i].useGravity = false;
        }
    }
    public void turnonRagdoll()
    {
        Eanim.enabled = false;
        Enmcollider.enabled = false;
        Erb.useGravity = false;
        Erb.isKinematic = true;
        for (int i = 0; i < co.Length; i++)
        {
            co[i].enabled = true;
        }
        for (int i = 0; i < rigit.Length; i++)
        {
           rigit[i].useGravity = true;
        }
    }
    #endregion
   

}
