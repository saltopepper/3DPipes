using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    //The moving boundaries in the square.
    //Needs to Scale in Z direction
    public int boundariesXYZ = 50;
    //Boundary Shift in Z Axis, so pipes are not directly in front of the camera.
    public int shiftZ = 10;

    private Camera main;
    public GameObject Pipe;
    public GameObject Intersection;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        main = Camera.main;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            createNewPipeline();
        }
    }

    //Checks if a vector is in our boundary field
    public bool checkInBoundary(Vector3 position)
    {
        int border = Mathf.RoundToInt(boundariesXYZ / 2f);
        if (position.z >= shiftZ && position.z <= shiftZ + boundariesXYZ)
        {
            if (position.y >= -border && position.y <= border)
            {
                if (position.x >= -border && position.x <= border)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public bool checkAtBoundary(Vector3 position)
    {
        if (position.z == shiftZ || position.z == shiftZ + boundariesXYZ || position.y == -(boundariesXYZ / 2f) || position.y == boundariesXYZ / 2f || position.x == -(boundariesXYZ / 2f) || position.x == boundariesXYZ / 2f)
        {
            return true;
        }


        return false;
    }

    //finds a new vector in boundary field
    //just to be failsafe, dont spawn at the edge.
    Vector3 newVectorInBoundary()
    {
        Vector3 vec = new Vector3(Random.Range(-boundariesXYZ / 2 + 1, boundariesXYZ / 2), Random.Range(-boundariesXYZ / 2 + 1, boundariesXYZ / 2), shiftZ + 1 + Random.Range(0, boundariesXYZ));


        return vec;
    }

    //Creates new Pipe
    public void createNewPipeline()
    {
        Debug.Log("Creating new Pipeline");
        //create new Pipe within boundary
        bool foundNewLocation = false;
        Vector3 newLocation = newVectorInBoundary();
        while (!foundNewLocation)
        {
            if (Physics.CheckBox(newLocation, new Vector3(0.4f, 0.4f, 0.4f)))
            {
                //Something is in that area
                //try out new location
                newLocation = newVectorInBoundary();
            }
            else
            {
                //Nothing is in that area
                foundNewLocation = true;
            }
        }
        GameObject newPipeline = new GameObject("Pipeline");
        newPipeline.AddComponent<Pipeline>();
        newPipeline.GetComponent<Pipeline>().pipelinecolor = new Color(Random.Range(0, 1f), Random.Range(0, 1f), Random.Range(0, 1f));
        GameObject go = Instantiate(Pipe, newLocation, Quaternion.identity);
        go.transform.parent = newPipeline.transform;
        go.GetComponent<PipeMovement>().pipelineColor = go.transform.parent.GetComponent<Pipeline>().pipelinecolor;
        go.GetComponent<PipeMovement>().createIntersectionPoint();

        //rotate object in random around x and y
        go.transform.Rotate(Random.Range(-1, 3) * 90, Random.Range(-1, 3) * 90, 0);

    }
}
