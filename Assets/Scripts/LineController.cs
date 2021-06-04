using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineController : MonoBehaviour
{

    public Rigidbody2D rigidBody;

    public EdgeCollider2D edgeCollider;
    // Start is called before the first frame update
    private void Start()
    {
        rigidBody = gameObject.GetComponent<Rigidbody2D>();
        edgeCollider = gameObject.GetComponent<EdgeCollider2D>();
        //rigidBody.centerOfMass = new Vector2(-100, 0);
    }

    // Update is called once per frame
    private void Update()
    {
        CalCenterOfMass();
    }

    private void CalCenterOfMass()
    {
        if (rigidBody.bodyType != RigidbodyType2D.Dynamic) return;
        Vector2[] points = edgeCollider.points;
        float sumX = 0, sumY = 0;
        int len = points.Length;
        foreach (var point in points)
        {
            sumX += point.x;
            sumY += point.y;
        }

        rigidBody.centerOfMass = new Vector2(sumX/len, sumY/len);
    }
}
