using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PipeMovement : MonoBehaviour
{
    public Color pipelineColor;
    public float speed = 10;
    public float turnRate = 0.05f;
    // Start is called before the first frame update
    void Start()
    {
        speed = 50;
        pipelineColor = transform.parent.GetComponent<Pipeline>().pipelinecolor;
        GetComponentInChildren<MeshRenderer>().material.color = pipelineColor;
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(transform.localScale.z);
        stretchPipe();
        reachUniform();
    }

    // stretches the pipe / move the pipe
    void stretchPipe()
    {
        transform.localScale += new Vector3(0,0,Time.deltaTime * speed);
    }

    // turn corner
    void turnCorner()
    {
        createIntersectionPointEnd();
        createPipeSection();
        this.enabled = false;
    }

    // checks if scale is close to next integer
    void closeEnough()
    {
        float dist = Mathf.CeilToInt(transform.localScale.z) - transform.localScale.z;
        if (dist <= 0.25f)
        {
            transform.localScale = new Vector3(1,1,Mathf.RoundToInt(transform.localScale.z));
        }
    }

    //if reached scale.z % 2 == 0 (that means, reached a new grid field)
    void reachUniform()
    {
        closeEnough();
        //if (transform.localScale.z == (int)transform.localScale.z)
        if(transform.localScale.z % 2 == 0)
        {
            if(checkAreaInFront())
            {
                //something in front
                //you need to turn
                turnCorner();
            }
            else
            {
                //nothing in front.. but maybe still turn?
                if(Random.Range(0,1f) <= turnRate)
                {
                    turnCorner();
                }
            }
        }
    }

    //true if something is in front
    bool checkAreaInFront()
    {
        Vector3 newLocation = transform.position + transform.forward * (1 + transform.localScale.z / 2);
        if (Physics.CheckBox(newLocation, new Vector3(0.4f, 0.4f, 0.4f)) || !GameManager.instance.checkInBoundary(newLocation))
        {
            return true;
        }
        return false;
    }

    public void createIntersectionPoint()
    {
        GameObject Intersection = Instantiate(GameManager.instance.Intersection);
        Intersection.transform.position = transform.position;
        Intersection.transform.parent = transform.parent;
        Intersection.GetComponent<MeshRenderer>().material.color = pipelineColor;
    }

    void createIntersectionPointEnd()
    {
        GameObject Intersection = Instantiate(GameManager.instance.Intersection);
        Intersection.transform.position = transform.position + transform.forward * (transform.localScale.z / 2);
        Intersection.transform.parent = transform.parent;
        Intersection.GetComponent<MeshRenderer>().material.color = pipelineColor;
    }

    //create new pipe section and find new rotation/direction
    void createPipeSection()
    {
        Vector3 vec = FindNewDirection();

        //end and create new pipeline
        if(vec == Vector3.zero)
        {
            GameManager.instance.createNewPipeline();
            return;
        }

        GameObject Pipe = Instantiate(GameManager.instance.Pipe);
        Pipe.transform.position = transform.position + transform.forward * (transform.localScale.z / 2);
        Pipe.transform.parent = transform.parent;
        Pipe.GetComponentInChildren<MeshRenderer>().material.color = pipelineColor;

        
        Pipe.transform.LookAt(vec);
        //rotate object in random around x and y
        //go.transform.Rotate(Random.Range(-1, 3) * 90, Random.Range(-1, 3) * 90, 0);
    }

    Vector3 FindNewDirection()
    {
        Vector3 vector = Vector3.zero;

        //current location
        Vector3 newLocation = transform.position + transform.forward * (transform.localScale.z / 2);

        List<Vector3> list = new List<Vector3>();

        //check all 4 directions in x and y
        Vector3 temp = newLocation + transform.up;
        if (!Physics.CheckBox(temp, new Vector3(0.4f, 0.4f, 0.4f)) && GameManager.instance.checkInBoundary(temp))
        {
            list.Add(temp);
        }

        temp = newLocation - transform.up;
        if (!Physics.CheckBox(temp, new Vector3(0.4f, 0.4f, 0.4f)) && GameManager.instance.checkInBoundary(temp))
        {
            list.Add(temp);
        }

        temp = newLocation + transform.right;
        if (!Physics.CheckBox(temp, new Vector3(0.4f, 0.4f, 0.4f)) && GameManager.instance.checkInBoundary(temp))
        {
            list.Add(temp);
        }

        temp = newLocation - transform.right;
        if (!Physics.CheckBox(temp, new Vector3(0.4f, 0.4f, 0.4f)) && GameManager.instance.checkInBoundary(temp))
        {
            list.Add(temp);
        }

        if(list.Count != 0)
        {
            vector = list[Random.Range(0, list.Count)];
        }
        else
        {
           
        }

        return vector;
    }


}
