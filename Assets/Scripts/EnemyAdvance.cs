using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAdvance : MonoBehaviour
{
    #region public method

    public Transform player;
    public Rigidbody Erb;
    public Material deadmaterial;
    public bool Dead;
    public bool stopmoving;
    public Collider Enmcollider;
    public Collider[] co;
    public Rigidbody[] rigit;
    public int MovementSpeed = 200;
  
    public GameObject[] followNode;
    public int nodeindex;
    // Start is called before the first frame update
    public Animator Eanim;
    #endregion

    //script reference
    UIManager uimanager;
    #region monobehaviour callback
    void Start()
    {
        turnOffRagdoll();
      //  transform.LookAt(player);
        uimanager = GameObject.Find("UImanager").GetComponent<UIManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, followNode[nodeindex].transform.position) < 2)
        {
            if (nodeindex < 1)
            {
                nodeindex++;
            }
        }
       
            transform.rotation = Quaternion.LookRotation(followNode[nodeindex].transform.position-transform.position);

        if (Vector3.Distance(transform.position, followNode[1].transform.position) < 1f)
        {
            Eanim.SetTrigger("Punch");
            stopmoving = true;
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
                if (!stopmoving)
                {
                    Erb.AddRelativeForce(Vector3.forward * MovementSpeed);

                }
            }
        }
        else
        {
            Eanim.SetBool("Over", true);
        }



    }
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
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Hammer"))
        {
            this.gameObject.GetComponentInChildren<SkinnedMeshRenderer>().material = deadmaterial;  //Assigning Dead material to Enemy
            Dead = true;
            uimanager.Deadpoint++;
            Eanim.SetFloat("walk", 0);
            turnonRagdoll();
        }
    }
    #endregion
}
