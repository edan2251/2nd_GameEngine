using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public float jumpPower = 5f;
    public float gravity = -9.81f;
    private CharacterController controller;
    private Vector3 velocity;
    public bool isGrounded;

    void Start()
    {
        controller = GetComponent<CharacterController>();    
    }

    void Update()
    {
        isGrounded = controller.isGrounded;
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        // 카메라 기준 방향 계산
        Transform cam = Camera.main.transform;

        Vector3 camForward = cam.forward;
        Vector3 camRight = cam.right;

        // y축 방향 제거 (수평 이동만)
        camForward.y = 0f;
        camRight.y = 0f;
        camForward.Normalize();
        camRight.Normalize();

        Vector3 move = camRight * x + camForward * z;
        controller.Move(move * speed * Time.deltaTime);

        //여기부터 점프
        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            velocity.y = jumpPower;
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }


    //교수님꺼
    //void Update()
    //{
    //    isGrounded = controller.isGrounded;
    //    float x = Input.GetAxis("Horizontal");
    //    float z = Input.GetAxis("Vertical");

    //    Vector3 move = new Vector3(x, 0, z);
    //    controller.Move(move * speed * Time.deltaTime);

    //    if (isGrounded && Input.GetKeyDown(KeyCode.Space))
    //    {
    //        velocity.y = jumpPower;
    //    }
    //    velocity.y += gravity * Time.deltaTime;
    //    controller.Move(velocity * Time.deltaTime);
    //}
}
