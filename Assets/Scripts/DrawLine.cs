using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawLine : MonoBehaviour
{
    public GameObject linePrefab;
    // hold our current line object
    public GameObject currentLine;

    public LineRenderer lineRenderer;
    public EdgeCollider2D edgeCollider;
    public Rigidbody2D rigidBody;
    
    // receive input from the user
    public List<Vector2> fingerPositions;
    public List<GameObject> lines;
    public bool erase = false;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // left click
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 screenMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //GameObject g = Utils.Raycast(Camera.main, screenMousePosition, 1 << 8); 
            int i = 0;
            while(i < lines.Count) {
                lineRenderer = lines[i].GetComponent<LineRenderer>();
                for(int j = 0; j < lineRenderer.positionCount; j++) {
                    if (Vector2.Distance(screenMousePosition, lines[i].transform.TransformPoint(lineRenderer.GetPosition(j))) < 0.1f) {
                        //Debug.Log(screenMousePosition);
                        //Debug.Log(lineRenderer.GetPosition(j));
                        erase = true;
                        GameObject temp = lines[i];
                        lines.Remove(temp);
                        Destroy(temp);
                        i--;
                        break;
                    }
                }
                i++;
            }
            if (!erase)  CreateLine();
        }

        if (!erase && Input.GetMouseButton(0))
        {
            Vector2 tempFingerPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (Vector2.Distance(tempFingerPos, fingerPositions[fingerPositions.Count - 1]) > .1f)
            {
                UpdateLine(tempFingerPos);
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (erase) erase = false;
            else FinishLine();
        }
    }

    void CreateLine()
    {
        currentLine = Instantiate(linePrefab, Vector3.zero, Quaternion.identity);
        currentLine.layer = 1 << 3;
        lineRenderer = currentLine.GetComponent<LineRenderer>();
        edgeCollider = currentLine.GetComponent<EdgeCollider2D>();
        rigidBody = currentLine.GetComponent<Rigidbody2D>();
        // Create a new line
        fingerPositions.Clear();
        // The line needs at least two points to be drown
        fingerPositions.Add(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        fingerPositions.Add(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        lineRenderer.SetPosition(0, fingerPositions[0]);
        lineRenderer.SetPosition(1, fingerPositions[1]);
        
        // update the edge collider
        edgeCollider.points = fingerPositions.ToArray();

        rigidBody.bodyType = RigidbodyType2D.Static;
    }

    void UpdateLine(Vector2 newFingerPos)
    {
        fingerPositions.Add(newFingerPos);
        lineRenderer.positionCount++;
        lineRenderer.SetPosition(lineRenderer.positionCount-1, newFingerPos);
        edgeCollider.points = fingerPositions.ToArray();
    }

    private void FinishLine()
    {
        rigidBody.bodyType = RigidbodyType2D.Dynamic;
        rigidBody.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        lines.Add(currentLine);
    }
}
