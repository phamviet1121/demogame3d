using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class RotetiByMouse : MonoBehaviour
{
    public float anglePerSecondY;
    public float anglePerSecondX;
    public Transform camaraholder;
    public float minPitch;
    public float maxPitch;
    private float Pitch;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        UpdatePitch();
        Updateyaw();
    }

    void UpdatePitch()
    {
        float mouseY = Input.GetAxis("Mouse Y");
        float deltaPitch = -mouseY * anglePerSecondY;
        Pitch= Mathf.Clamp(Pitch+deltaPitch, minPitch, maxPitch);
        camaraholder.localEulerAngles=new Vector3 (Pitch, 0f,0f);
    }
      void Updateyaw()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float yaw = mouseX * anglePerSecondX * Time.deltaTime;
        transform.Rotate(0f, yaw, 0f);
    }
}
