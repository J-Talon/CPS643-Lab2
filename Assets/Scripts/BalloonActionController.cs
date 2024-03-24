
using UnityEngine;

public class BalloonActionController : MonoBehaviour
{
    public GameObject controller;  //extern: the controller
    
    private float sizeGain;
    private float size;
    private bool deflating;
    private Rigidbody body;
    private ConstantForce force;
    
    private const float HEIGHT_BOUND = 10;
    private const float DETACH_SIZE = 3;
    private const float MIN_SIZE = 0.4f;
    private const float EXPIRE_TIME = 5;

    private float deflatedTime;
    
    void Start()
    {
        sizeGain = 0.1f;
        deflatedTime = 0;
        size = MIN_SIZE;
        
      transform.parent = controller.transform;
      body = GetComponent<Rigidbody>();
      force = body.GetComponent<ConstantForce>();
      deflating = false;
    }


    public void deflate()
    {
        deflating = true;
        setSizeGain(-0.1f);
    }
    

   void updateSize()
    {
        
        if (sizeGain == 0)
            return;

        float gain = Mathf.Lerp(MIN_SIZE, DETACH_SIZE, sizeGain);
        gain *= Time.deltaTime;
        
        if (sizeGain < 0)
        {
            if (size < MIN_SIZE)
                return;
            
            size -= gain;
        }
        else
        {
            if (size >= DETACH_SIZE)
                return;
            
            size += gain;
        }
        
        transform.localScale = new Vector3(size, size, size);
  

    }
    
    void setSizeGain(float newGain)
    {
        sizeGain = newGain;
    }
    
    void Update()
    {
        updateSize();
        if (size > DETACH_SIZE)
        {
            transform.parent = null;
            setSizeGain(0);
        }
        
        if (transform.position.y > HEIGHT_BOUND || 
            transform.position.y < -HEIGHT_BOUND ||
            deflatedTime >= EXPIRE_TIME)
        {
            Destroy(gameObject);
        }


        if (size <= MIN_SIZE)
        {
            deflatedTime += Time.deltaTime;
        }
    }


    void FixedUpdate()
    {
        if (!(transform.parent == null))
            return;

        if (!deflating)
        {
            force.enabled = true;
        }
        else
        {
            
            if (size <= MIN_SIZE)
                body.useGravity = true;
            
            force.force = new Vector3(0, (-0.3f/size), 0);
            force.torque = new Vector3(20, 20, 20);
        }
    }
    
    
    
    
    
}
