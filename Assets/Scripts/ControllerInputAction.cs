
using UnityEngine;



public class ControllerInputAction : MonoBehaviour
{
    //extern
    public GameObject balloonPrefab;
    public GameObject leftController;
    public GameObject rightController;
    public LineRenderer laser;
   
     
     
    private bool fireLaser;
    private static GameObject activeBalloon = null;
    private const float TRACE_DISTANCE = 30;


    void Start()
    {
        fireLaser = false;
    }
    
    void Update()
    {
        if (!fireLaser)
        {
            return;
        }

        
        Transform form = rightController.transform;
        Vector3 direction = form.forward;
        Vector3 startPos = form.position;
        Ray ray = new Ray(startPos, direction);

        RaycastHit hit;
        Vector3[] positions;
        if (Physics.Raycast(ray, out hit, TRACE_DISTANCE))
        {
            positions = new Vector3[] { startPos, hit.point };
            GameObject hitObject = hit.collider.gameObject;
            if (hitObject.Equals(activeBalloon))
            {
                hitObject.GetComponent<BalloonActionController>().deflate();
            }
        }
        else
        {
            Vector3 directionDist = new Vector3(direction.x * TRACE_DISTANCE, direction.y * TRACE_DISTANCE,
                direction.z * TRACE_DISTANCE);

            Vector3 end = new Vector3(startPos.x + directionDist.x, startPos.y + directionDist.y,
                startPos.z + directionDist.z);
            
            positions = new Vector3[] { startPos, end};

        }
        laser.SetPositions(positions);
    }

    public void onMenuButtonDown()
    {
        
        
        if (activeBalloon != null)
        {
            return;
        }
        
        balloonPrefab.transform.localScale = new Vector3(1, 1, 1);
        activeBalloon = Instantiate(balloonPrefab, leftController.transform.position, Quaternion.identity);
        activeBalloon.SetActive(true);
            
    }

    public void onRightButtonDown()
    {
        fireLaser = true;
        laser.enabled = true;
        
    }

    public void onRightButtonUp()
    {
        fireLaser = false;
        laser.enabled = false;
    }
    
}
