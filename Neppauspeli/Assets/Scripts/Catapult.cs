using UnityEngine;
using System.Collections;

public class Catapult : MonoBehaviour
{
    public float rotspeed = 2;

	void Update ()
    {
        catapultRotate();
	}
    void catapultRotate()
    {
        if(Input.GetButton("Jump"))
        {
            transform.Rotate(rotspeed, 0, 0);
        }
    }
}
