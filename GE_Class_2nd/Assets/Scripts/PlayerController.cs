using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float currentSpeed;
    private float stopSpeed = 0f;
    private float walkSpeed = 5f;
    private float runSpeed = 12f;
    private float jumpPower = 5f;
    public float gravity = -9.81f;

    public CinemachineVirtualCamera virtualCam;
    public float rotationSpeed = 10f;
    private CinemachinePOV pov;

    private CharacterController controller;
    private Vector3 velocity;
    public bool isGrounded;
    public bool isRunning;

    public CinemachineSwitcher cinemachineSwitcher;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        pov = virtualCam.GetCinemachineComponent<CinemachinePOV>();
    }

    void Update()
    {
        //if (Cursor.lockState != CursorLockMode.Locked) return;

        //if (cinemachineSwitcher.usingFreeLook == true) return;

        if (cinemachineSwitcher.usingFreeLook == false)
        {
            if (Input.GetKey(KeyCode.LeftShift))                     //���� ����Ʈ�� ������ �޸���� ����
            {
                currentSpeed = runSpeed;
                isRunning = true;
            }
            else
            {
                currentSpeed = walkSpeed;
                isRunning = false;
            }
        }

        if (isRunning)
        {
            virtualCam.m_Lens.FieldOfView = Mathf.Lerp(virtualCam.m_Lens.FieldOfView, 65f, Time.deltaTime * 5f);
        }
        else
        {
            virtualCam.m_Lens.FieldOfView = Mathf.Lerp(virtualCam.m_Lens.FieldOfView, 40f, Time.deltaTime * 5f);
        }

        isGrounded = controller.isGrounded;
        if(isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        // ī�޶� ���� ���� ���
        Vector3 camForward = virtualCam.transform.forward;
        camForward.y = 0;
        camForward.Normalize();

        Vector3 camRight = virtualCam.transform.right;
        camRight.y = 0;
        camRight.Normalize();

        Vector3 move = (camForward * z + camRight * x).normalized;  //�̵� ���� = ī�޶� forward/right���
        controller.Move(move * currentSpeed * Time.deltaTime);

        if (cinemachineSwitcher.usingFreeLook == true)
        {
            currentSpeed = stopSpeed;
            jumpPower = 0f;
        }
        else
        {
            currentSpeed = walkSpeed;
            jumpPower = 5f;
        }

            float cameraYaw = pov.m_HorizontalAxis.Value;   //���콺 �¿� ȸ����
        Quaternion targetRot = Quaternion.Euler(0f, cameraYaw, 0f);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotationSpeed * Time.deltaTime);

        //������� ����
        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            velocity.y = jumpPower;
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

}
