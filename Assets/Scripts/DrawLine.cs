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
    public LineController lineControl;
    
    // receive input from the user
    public List<Vector2> fingerPositions;
    public List<GameObject> lines;
    public bool erase = false;
    public LayerMask layerMask;
    private float castLength = 0.2f;
    private float distance = 0.0f;
    private float totalDistance = 0.0f;
    private float maxLength = 15.0f;
    private float totalLength = 150.0f;
    private Dictionary<LineRenderer, float> distanceInfo = new Dictionary<LineRenderer, float>();

    float GetLength(GameObject line) {
    	lineRenderer = line.GetComponent<LineRenderer>();
    	float result = 0;
    	for(int i = 0; i < lineRenderer.positionCount - 1; i++) result += Vector2.Distance(line.transform.TransformPoint(lineRenderer.GetPosition(i)), line.transform.TransformPoint(lineRenderer.GetPosition(i + 1)));
    	return result; 
    }

    // Start is called before the first frame update
    void Start()
    {
     	   
    }

    // Update is called once per frame
    void Update()
    {
    	if (totalDistance < totalLength) {
	        // left click
	        if (Input.GetMouseButtonDown(0))
	        {
	            /*Vector2 screenMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
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
	            if (!erase)  CreateLine();*/
	            CreateLine();
	        }

	        if (Input.GetMouseButton(0))
	        {
	        	if (distance < maxLength && totalDistance < totalLength) {
		            Vector2 tempFingerPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		            float tempDistance = Vector2.Distance(tempFingerPos, fingerPositions[fingerPositions.Count - 1]);
		            if (tempDistance > .1f)
		            {
		            	//distance += tempDistance;
		            	//totalDistance += tempDistance;
		                UpdateLine(tempFingerPos);
		            }
	        	} else {
	        		Debug.Log("not enough ink");
	        		FinishLine();
	        		//if (!distanceInfo.ContainsKey(lineRenderer)) distanceInfo.Add(lineRenderer, distance);
	        		//distance = 0.0f;
	        	}
	        }

	        if (Input.GetMouseButtonUp(0))
	        {
	            FinishLine();
	            Debug.Log(distance);
	            //if (!distanceInfo.ContainsKey(lineRenderer)) distanceInfo.Add(lineRenderer, distance);
	            //distance = 0.0f;
	            Debug.Log(totalDistance);
	        }
	    } else {
	    	Debug.Log("not enough ink");
	        FinishLine();
	        //if (!distanceInfo.ContainsKey(lineRenderer)) distanceInfo.Add(lineRenderer, distance);
	        //distance = 0.0f;
	    }

        if (Input.GetMouseButtonDown(1)) {
        	RemoveLine();
        }

        if (Input.GetMouseButton(1)) {
        	RemoveLine();
        }

    }

    void RemoveLine() {
       	Vector2 screenMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var hitLeft = Physics2D.Raycast(screenMousePosition, transform.TransformDirection(Vector2.left), castLength, layerMask);
        if (hitLeft && !distanceInfo.ContainsKey(hitLeft.collider.gameObject.GetComponent<LineRenderer>())) {
        	Debug.Log("left");
        	//Debug.Log(totalDistance);
        	//totalDistance -= GetLength(hitLeft.collider.gameObject);
        	//totalDistance -= distanceInfo[hitLeft.collider.gameObject.GetComponent<LineRenderer>()];
        	//distanceInfo.Remove(hitLeft.collider.gameObject.GetComponent<LineRenderer>());
        	//Debug.Log(totalDistance);
        	Destroy(hitLeft.collider.gameObject);
        }
        var hitRight = Physics2D.Raycast(screenMousePosition, transform.TransformDirection(Vector2.right), castLength, layerMask);
        if (hitRight && !distanceInfo.ContainsKey(hitRight.collider.gameObject.GetComponent<LineRenderer>())) {
        	Debug.Log("right");
        	//Debug.Log(totalDistance);
        	//totalDistance -= GetLength(hitRight.collider.gameObject);
        	//totalDistance -= distanceInfo[hitRight.collider.gameObject.GetComponent<LineRenderer>()];
        	//distanceInfo.Remove(hitRight.collider.gameObject.GetComponent<LineRenderer>());
        	//Debug.Log(totalDistance);
        	Destroy(hitRight.collider.gameObject);
        }
		var hitUp = Physics2D.Raycast(screenMousePosition, transform.TransformDirection(Vector2.up), castLength, layerMask);
        if (hitUp && !distanceInfo.ContainsKey(hitUp.collider.gameObject.GetComponent<LineRenderer>())) {
        	Debug.Log("up");
        	//Debug.Log(totalDistance);
        	//totalDistance -= GetLength(hitUp.collider.gameObject);
        	//totalDistance -= distanceInfo[hitUp.collider.gameObject.GetComponent<LineRenderer>()];
        	//distanceInfo.Remove(hitUp.collider.gameObject.GetComponent<LineRenderer>());
        	//Debug.Log(totalDistance);
        	Destroy(hitUp.collider.gameObject);
        }
        var hitDown = Physics2D.Raycast(screenMousePosition, transform.TransformDirection(Vector2.down), castLength, layerMask);
    	if (hitDown && !distanceInfo.ContainsKey(hitDown.collider.gameObject.GetComponent<LineRenderer>())) {
        	Debug.Log("down");
        	//Debug.Log(totalDistance);
        	//totalDistance -= GetLength(hitDown.collider.gameObject);
        	//totalDistance -= distanceInfo[hitDown.collider.gameObject.GetComponent<LineRenderer>()];
        	//distanceInfo.Remove(hitDown.collider.gameObject.GetComponent<LineRenderer>());
        	//Debug.Log(totalDistance);
        	Destroy(hitDown.collider.gameObject);
        }
    }

    void CreateLine()
    {
        currentLine = Instantiate(linePrefab, Vector3.zero, Quaternion.identity);
        lineRenderer = currentLine.GetComponent<LineRenderer>();
        edgeCollider = currentLine.GetComponent<EdgeCollider2D>();
        rigidBody = currentLine.GetComponent<Rigidbody2D>();
        lineControl = currentLine.GetComponent<LineController>();
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

        distance = 0.0f;
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
        lineControl.onFinished();
    }
}
