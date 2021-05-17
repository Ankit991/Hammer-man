using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Cam : MonoBehaviour
{
    // Start is called before the first frame update
    public float turnspeed = 15;
    Camera mcamera;
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        mcamera = Camera.main;
    }

    // Update is called once per frame
   
        private void FixedUpdate()
        {

        float yawcamera = mcamera.transform.rotation.eulerAngles.y;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, yawcamera, 0), turnspeed * Time.deltaTime);

    }

}
