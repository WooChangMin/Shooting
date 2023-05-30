using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TPSCameraController : MonoBehaviour
{
    [SerializeField] Transform cameraRoot;
    [SerializeField] float mouseSensitivity;

    //카메라가 바라보고 잇는 방향으로 플레이어도 바라보게 만듬
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
        //카메라는 Update에 구현을 하면안됨. 카메라의이동 -> 플레이어의이동 ->카메라의 이동 (연쇄적으로 발생해서 계속 움직일 수도 있음) LateUpdate->후처리(카메라 움직임을 위해 존재한다고 봐도 됨)
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
        // 좌우방향은 y축 기준으로 회전
        yRotation += lookDelta.x * mouseSensitivity * Time.deltaTime;

        // 위아래 방향은 x축 기준으로 회전
        xRotation -= lookDelta.y * mouseSensitivity * Time.deltaTime;

        // 플레이어가 위아래 기준으로 최대 최솟값을 지정해줘야함 -> 뒷 통수 까지 볼수있음
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);

        // 위아래의 경우 플레리어는 움직임 없이 카메라만 이동
        cameraRoot.rotation = Quaternion.Euler(xRotation, yRotation, 0);

        // 좌우는  회전한 방향만큼 플레이어도 회전
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
