using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwerveInput : MonoBehaviour
{
    public float moveAmountOnXAxis;

    private float firstFingerTouchPosition;
    
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            firstFingerTouchPosition = Input.mousePosition.x;
        }
        else if (Input.GetMouseButton(0))
        {
            moveAmountOnXAxis = Input.mousePosition.x - firstFingerTouchPosition;
            firstFingerTouchPosition = Input.mousePosition.x;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            moveAmountOnXAxis = 0;
        }
    }
}
