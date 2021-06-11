using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveObstacles : MonoBehaviour
{
    private int layerMask = 1 << 9;

    private int castLength = 30;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log("Hitting right button");
            Vector2 screenMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var hitLeft = Physics2D.Raycast(screenMousePosition,
                transform.TransformDirection(Vector2.left), castLength, layerMask);
            var hitRight = Physics2D.Raycast(screenMousePosition,
                transform.TransformDirection(Vector2.right), castLength, layerMask);

            if (hitLeft && hitRight && hitLeft.collider == hitRight.collider)
            {
                Destroy(hitLeft.collider.gameObject);
            }
        }
    }
}
