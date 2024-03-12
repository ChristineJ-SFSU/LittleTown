using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lookat : MonoBehaviour
{
    // Start is called before the first frame update
    public Camera camera;
    void Start()
    {
        if(!camera){
           camera = Camera.main;
        }
       // transform.Rotate(90,0,0);
    }

    // Update is called once per frame
    void Update()
    {
        
       
    }
}
