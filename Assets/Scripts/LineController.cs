using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineController : MonoBehaviour
{

    public Rigidbody2D rigidBody;
    // Start is called before the first frame update
    void Start()
    {
        rigidBody = gameObject.GetComponent<Rigidbody2D>();
        //rigidBody.centerOfMass = new Vector2(-100, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
