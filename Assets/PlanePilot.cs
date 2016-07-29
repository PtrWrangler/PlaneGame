using UnityEngine;
using System.Collections;

public class PlanePilot : MonoBehaviour {
    public float speed = 50.0f;

    //use this for dropping lines behind plane
    public int frameGap = 0;
    public Vector3 lastPos = Camera.main.transform.position;

    //table to keep locations of all lines for hit testing (y is key)
    

	// Use this for initialization
	void Start () {
        Debug.Log("Plane Pilot Scrpt added to: " + gameObject.name);
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 moveCamTo = transform.position - transform.forward * 20.0f + Vector3.up * 5.0f;
        float bias = 0.96f;
        Camera.main.transform.position = Camera.main.transform.position * bias + 
                                         moveCamTo * (1.0f-bias);
        Camera.main.transform.LookAt(transform.position + transform.forward * 30.0f);

        transform.position += transform.forward * Time.deltaTime * speed;
        speed -= transform.forward.y * Time.deltaTime * 50.0f;

        if (speed <= 15.0f)
            speed = 15.0f;
        
        else if (speed >= 150.0f)
            speed = 150.0f;
        
        

        transform.Rotate(Input.GetAxis("Vertical") * 2, 0.0f, -Input.GetAxis("Horizontal") * 2);

        float terrainHeightWhereWeAre = Terrain.activeTerrain.SampleHeight(transform.position);
        if (terrainHeightWhereWeAre > transform.position.y)
        {
            transform.position = new Vector3(transform.position.x,
                                             terrainHeightWhereWeAre,
                                             transform.position.z);
        }

        if (frameGap == 10) {
            DrawLine(lastPos, transform.position, Color.red, 100.2f);
            lastPos = transform.position;
            frameGap = 0;
        }
        else
        {
            frameGap++;
        }
    }

    void DrawLine(Vector3 start, Vector3 end, Color color, float duration)
    {
        GameObject myLine = new GameObject();
        myLine.transform.position = start;
        myLine.AddComponent<LineRenderer>();

        /*BoxCollider _bc = (BoxCollider)myLine.gameObject.AddComponent(typeof(BoxCollider));
        _bc.center = Vector3.zero;
        Rigidbody gameObjectsRigidBody = myLine.AddComponent<Rigidbody>(); // Add the rigidbody.
        gameObjectsRigidBody.mass = 5; // Set the GO's mass to 5 via the Rigidbody.*/

        LineRenderer lr = myLine.GetComponent<LineRenderer>();
        lr.material = new Material(Shader.Find("Particles/Alpha Blended Premultiply"));
        lr.SetColors(color, color);
        lr.SetWidth(0.5f, 0.5f);
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
        GameObject.Destroy(myLine, duration);
    }
}
