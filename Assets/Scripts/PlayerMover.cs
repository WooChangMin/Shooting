using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class PlayerMover : MonoBehaviour
{
    //�������� ���� �ӵ�
    [SerializeField] float runSpeed;
    [SerializeField] float walkSpeed;
    [SerializeField] float jumpSpeed;

    // CharacterController ������Ʈ�� ��������
    private CharacterController controller;
    
    //input���� �ޱ����� ����
    private Vector3 moveDir;
    private float moveSpeed;
    //������ ��� ���� y�ӵ��� �ʿ���
    private float ySpeed = 0;

    // �ִϸ����� ��������
    private Animator anim;

    private bool isWalking;

    private void Awake()
    {
        //rigidbody�� transform�� �ƴ� ���ο� ���۳�Ʈ ���� 
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();   
    }

    private void Update()
    {
        //������ ����
        Move();

        //�÷��̾ ���� �굵�� ������� + ��������. rigidbody�� �����ϴ°� ��ü�� ����� ������ ���� ����� �ƴ�
        Jump();
    }

    private void Move()
    {
        //�ӷ��� ����� ������ �ӵ���ġ * ��ŸŸ��
        // controller.Move(moveDir * moveSpeed * Time.deltaTime);  => ������� ������
        if (moveDir.magnitude <= 0)          //�ȿ�����
        {
            moveSpeed = Mathf.Lerp(moveSpeed, 0, 0.5f);  // �������Ⱦ��� =0 ���� �������ָ� �ٷθ��߰�, Mathf�� Lerp�� ���ٰ�� ��������.
        }
        else if (isWalking)
        {
            moveSpeed = Mathf.Lerp(moveSpeed, walkSpeed, 0.5f);
        }
        else
        {
            moveSpeed = Mathf.Lerp(moveSpeed, runSpeed, 0.5f);
        }
        // ���ñ��� ������
        controller.Move(transform.forward * moveDir.z * moveSpeed * Time.deltaTime);
        controller.Move(transform.right * moveDir.x * moveSpeed * Time.deltaTime);

        // Mathf.Lerp ������ ���°�쵵 �ִ�

        anim.SetFloat("XSpeed", moveDir.x, 0.1f, Time.deltaTime);
        anim.SetFloat("ZSpeed", moveDir.z, 0.1f, Time.deltaTime);
        anim.SetFloat("Speed", moveSpeed);
        //anim.SetFloat("ZSpeed", moveSpeed);
    }

    private void OnMove(InputValue value)
    {
        //������ �Է¹޴� ���� ���ӿ����� ���� �ٸ��Ƿ� x, z������ �ٲٴ� ������ �ʿ���
        Vector2 input = value.Get<Vector2>();
        moveDir = new Vector3(input.x, 0, input.y);
    }

    private void Jump()
    {
        // �߷¹������� ���� ����. ���ӵ� * �ð� -> �ӵ�
        ySpeed += Physics.gravity.y * Time.deltaTime;

        if (GroundCheck() && ySpeed < 0)
            ySpeed = 0;
        // ���� �ӷ¸�ŭ y������ �̵������ָ� ���� ��������
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
