using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMover : MonoBehaviour
{
    //�������� ���� �ӵ�
    [SerializeField] float moveSpeed;
    [SerializeField] private float jumpSpeed;

    // CharacterController ������Ʈ�� ��������
    private CharacterController controller;
    
    //input���� �ޱ����� ����
    private Vector3 moveDir;

    //������ ��� ���� y�ӵ��� �ʿ���
    private float ySpeed = 0;
    

    private void Awake()
    {
        //rigidbody�� transform�� �ƴ� ���ο� ���۳�Ʈ ���� 
        controller = GetComponent<CharacterController>();
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

        // ���ñ��� ������
        controller.Move(transform.forward * moveDir.z * moveSpeed * Time.deltaTime);
        controller.Move(transform.right * moveDir.x * moveSpeed * Time.deltaTime);

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
}
