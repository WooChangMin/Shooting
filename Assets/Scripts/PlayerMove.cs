using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] float jumpSpeed;

    private CharacterController controller;
    private Vector3 moveDir;
    private float ySpeed;


    private void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        Move();
        Jump();
    }

    private void FixedUpdate()
    {
        GroundChecker();
    }

    private void Move()
    {
        // 월드기준 움직임 controller.Move(moveDir * moveSpeed * Time.deltaTime);
        controller.Move(transform.forward * moveDir.z * moveSpeed * Time.deltaTime);
        controller.Move(transform.right * moveDir.x * moveSpeed * Time.deltaTime);
    }

    private void OnMove(InputValue value)
    {
        Vector2 input = value.Get<Vector2>();
        moveDir = new Vector3(input.x, 0, input.y);
    }
    private void Jump()
    {
        ySpeed += Physics.gravity.y * Time.deltaTime;
        if (GroundChecker() && ySpeed < 0)
            ySpeed = 0;
        controller.Move(Vector3.up * ySpeed * Time.deltaTime);
    }

    private void OnJump(InputValue value)
    {
        if (GroundChecker())
            ySpeed = jumpSpeed;
    }

    private bool GroundChecker()
    {
        RaycastHit hit;
        return Physics.SphereCast(transform.position + Vector3.up * 1, 0.5f , Vector3.down, out hit, 0.7f);
    }
}
