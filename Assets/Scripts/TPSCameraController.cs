using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TPSCameraController : MonoBehaviour
{
    [SerializeField] Transform cameraRoot;
    [SerializeField] float mouseSensitivity;

    //ī�޶� �ٶ󺸰� �մ� �������� �÷��̾ �ٶ󺸰� ����
    [SerializeField] float lookDistance;


    private Vector2 lookDelta;
    private float xRotation;
    private float yRotation;


    private void Update()
    {
        Rotate();
    }
    private void LateUpdate()
    {
        //ī�޶�� Update�� ������ �ϸ�ȵ�. ī�޶����̵� -> �÷��̾����̵� ->ī�޶��� �̵� (���������� �߻��ؼ� ��� ������ ���� ����) LateUpdate->��ó��(ī�޶� �������� ���� �����Ѵٰ� ���� ��)
        Look();
    }

    private void Rotate()
    {
        Vector3 lookPoint = Camera.main.transform.position + Camera.main.transform.forward * lookDistance;
        lookPoint.y = transform.position.y;
        transform.LookAt(lookPoint);
    }
    private void Look()
    {
        // �¿������ y�� �������� ȸ��
        yRotation += lookDelta.x * mouseSensitivity * Time.deltaTime;

        // ���Ʒ� ������ x�� �������� ȸ��
        xRotation -= lookDelta.y * mouseSensitivity * Time.deltaTime;

        // �÷��̾ ���Ʒ� �������� �ִ� �ּڰ��� ����������� -> �� ��� ���� ��������
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);

        // ���Ʒ��� ��� �÷������ ������ ���� ī�޶� �̵�
        cameraRoot.rotation = Quaternion.Euler(xRotation, yRotation, 0);

        // �¿��  ȸ���� ���⸸ŭ �÷��̾ ȸ��
        // transform.localRotation = Quaternion.Euler(0, yRotation, 0);
    }
    private void OnEnable()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void OnDisable()
    {
        Cursor.lockState = CursorLockMode.None;
    }

    private void OnLook(InputValue value)
    {
        lookDelta = value.Get<Vector2>();
    }
}
