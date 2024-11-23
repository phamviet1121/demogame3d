using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoverBykey : MonoBehaviour
{

    public CharacterController characterController;
    public float movingSpeed;

    private void OnValidate()
    {
        characterController=GetComponent<CharacterController>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float HorizontalInput = Input.GetAxis("Horizontal");
        float VerticalInput= Input.GetAxis("Vertical");


        Vector3 direction = transform.right * HorizontalInput + transform.forward * VerticalInput;
        //Vector3 direction = new Vector3(HorizontalInput, 0, VerticalInput).normalized;

        characterController.SimpleMove(direction * movingSpeed );
    }
}
