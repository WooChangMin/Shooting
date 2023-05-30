using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FPSCameraController : MonoBehaviour
{
    [SerializeField] Transform cameraRoot;
    [SerializeField] float mouseSensitivity;

    private Vector2 lookDelta;
    private float xRotation;
    private float yRotation;

    private void OnEnable()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void OnDisable()
    {
        Cursor.lockState = CursorLockMode.None;
    }

    private void LateUpdate()
    {
        //ī�޶�� Update�� ������ �ϸ�ȵ�. ī�޶����̵� -> �÷��̾����̵� ->ī�޶��� �̵� (���������� �߻��ؼ� ��� ������ ���� ����) LateUpdate->��ó��(ī�޶� �������� ���� �����Ѵٰ� ���� ��)
        Look();
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
        cameraRoot.localRotation = Quaternion.Euler(xRotation, 0 ,0);

        // �¿��  ȸ���� ���⸸ŭ �÷��̾ ȸ��
        transform.localRotation = Quaternion.Euler(0, yRotation, 0);
    }
    private void OnLook(InputValue value)
    {
        lookDelta = value.Get<Vector2>();
    }
}
