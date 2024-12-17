using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoverByKey : MonoBehaviour
{
    public CharacterController CharacterController;
    public float movingSpeed;
    public void OnValidate()
    {
        CharacterController=GetComponent<CharacterController>();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 diretion=transform.right*horizontal+transform.forward* vertical;
        CharacterController.SimpleMove(diretion * movingSpeed);
    }
}
