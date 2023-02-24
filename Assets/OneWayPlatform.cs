using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlatformEffector2D))]
public class OneWayPlatform : MonoBehaviour
{
    public bool updatedLayer = false;
    public float waitForAction = 0.3f;
    private float actionTime = 0;
    public PlatformEffector2D effector2D;
    // Start is called before the first frame update
    void Start()
    {
        effector2D = GetComponent<PlatformEffector2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (actionTime > 0)
        {
            actionTime -= Time.deltaTime;
            return;
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            effector2D.colliderMask &= ~(1 << 9);
            actionTime = waitForAction;
            updatedLayer = true;
        }
        else if(updatedLayer)
        {
            effector2D.colliderMask |= (1 << 9);
            updatedLayer = false;
        }

    }
}
