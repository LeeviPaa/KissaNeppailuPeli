using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour {

    public float horizontalSpeed = 1;
    public float verticalSpeed = 1;
    public float Yspeed = 1;
    public float rotspeed = 1;
    public float scrollSpeed = 1f;
    private float currScroll = 0.4f;
    private GameObject player;
    public Transform cameraClose;
    public Transform cameraFar;
    public Transform transformCamera;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

	void Update ()
    {
        movement();
        float delta = Input.GetAxis("Mouse ScrollWheel") - Input.GetAxis("Vertical2")*Time.deltaTime*5;
        if (delta != 0 && cameraClose != null && cameraFar != null)
        {
            currScroll += delta * Time.deltaTime * scrollSpeed;
            currScroll = Mathf.Clamp(currScroll, 0, 1);
        transformCamera.position = Vector3.Lerp(cameraClose.position, cameraFar.position, currScroll);
        }

    }
    void movement()
    {
        transform.position = player.transform.position;
        transform.Rotate(0, (Input.GetAxis("Horizontal2"))*rotspeed*Time.deltaTime, 0);
    }
}
