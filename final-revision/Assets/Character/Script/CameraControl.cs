using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;
    [SerializeField]
    private float mouse_sensitivity;
    float mouseX, mouseY;

    Vector3 object_rotation;
    public Transform character_body;

    CharacterControl health_check;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        health_check = target.GetComponent<CharacterControl>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LateUpdate()
    {
        if (health_check.is_alive() == true)
        {
            this.transform.position = Vector3.Lerp(this.transform.position, target.position + offset, Time.deltaTime * 10);
            mouseX += Input.GetAxis("Mouse X") * mouse_sensitivity;
            mouseY += Input.GetAxis("Mouse Y") * mouse_sensitivity;
            if (mouseY >= 40)
            {
                mouseY = 40;
            }
            if (mouseY <= -25)
            {
                mouseY = -25;
            }
            this.transform.eulerAngles = new Vector3(-mouseY, mouseX, 0);
            target.transform.eulerAngles = new Vector3(0, mouseX, 0);

            Vector3 object_rotation = this.transform.localEulerAngles;
            object_rotation.x += 10;
            object_rotation.z = 0;

            character_body.transform.eulerAngles = object_rotation;
        }
    }
}
