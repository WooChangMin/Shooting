using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class PlayerMover : MonoBehaviour
{
    //움직임을 위한 속도
    [SerializeField] float runSpeed;
    [SerializeField] float walkSpeed;
    [SerializeField] float jumpSpeed;

    // CharacterController 컴포넌트를 쓰기위함
    private CharacterController controller;
    
    //input값을 받기위한 방향
    private Vector3 moveDir;
    private float moveSpeed;
    //포물선 운동을 위한 y속도가 필요함
    private float ySpeed = 0;

    // 애니메이터 가져오기
    private Animator anim;

    private bool isWalking;

    private void Awake()
    {
        //rigidbody나 transform이 아닌 새로운 컴퍼넌트 받음 
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();   
    }

    private void Update()
    {
        //움직임 구현
        Move();

        //플레이어가 땅에 닿도록 만들어줌 + 점프구현. rigidbody로 구현하는건 강체가 생기기 때문에 좋은 방법이 아님
        Jump();
    }

    private void Move()
    {
        //속력을 만들고 싶을땐 속도수치 * 델타타임
        // controller.Move(moveDir * moveSpeed * Time.deltaTime);  => 월드기준 움직임
        if (moveDir.magnitude <= 0)          //안움직임
        {
            moveSpeed = Mathf.Lerp(moveSpeed, 0, 0.5f);  // 러프를안쓰고 =0 으로 대입해주면 바로멈추고, Mathf의 Lerp를 써줄경우 점차멈춤.
        }
        else if (isWalking)
        {
            moveSpeed = Mathf.Lerp(moveSpeed, walkSpeed, 0.5f);
        }
        else
        {
            moveSpeed = Mathf.Lerp(moveSpeed, runSpeed, 0.5f);
        }
        // 로컬기준 움직임
        controller.Move(transform.forward * moveDir.z * moveSpeed * Time.deltaTime);
        controller.Move(transform.right * moveDir.x * moveSpeed * Time.deltaTime);

        // Mathf.Lerp 러프를 쓰는경우도 있다

        anim.SetFloat("XSpeed", moveDir.x, 0.1f, Time.deltaTime);
        anim.SetFloat("ZSpeed", moveDir.z, 0.1f, Time.deltaTime);
        anim.SetFloat("Speed", moveSpeed);
        //anim.SetFloat("ZSpeed", moveSpeed);
    }

    private void OnMove(InputValue value)
    {
        //실제로 입력받는 값과 게임에서의 값이 다르므로 x, z값으로 바꾸는 과정이 필요함
        Vector2 input = value.Get<Vector2>();
        moveDir = new Vector3(input.x, 0, input.y);
    }

    private void Jump()
    {
        // 중력방향으로 당기기 위함. 가속도 * 시간 -> 속도
        ySpeed += Physics.gravity.y * Time.deltaTime;

        if (GroundCheck() && ySpeed < 0)
            ySpeed = 0;
        // 받은 속력만큼 y축으로 이동시켜주면 땅에 떨어진다
        controller.Move(Vector3.up * ySpeed * Time.deltaTime);
    }

    private void OnJump(InputValue value)
    {
        if(GroundCheck())
            ySpeed = jumpSpeed;
    }

    private bool GroundCheck()
    {
        RaycastHit hit;
        return Physics.SphereCast(transform.position + Vector3.up * 1 , 0.5f, Vector3.down, out hit, 0.7f);
    }

    private void OnWalk(InputValue value)
    {
        isWalking = value.isPressed;
    }
}
