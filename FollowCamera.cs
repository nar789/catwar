using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform target; // 따라갈 오브젝트 (예: 플레이어)
    public float smoothSpeed = 0.125f; // 카메라 이동 속도 조절 변수
    public Vector3 offset; // 카메라와 목표 오브젝트 사이의 거리

    private Vector3 velocity = Vector3.zero; // SmoothDamp에서 사용할 속도 변수

    CameraShake cameraShake;

    private void Start()
    {
        cameraShake = GetComponent<CameraShake>();
    }

    void LateUpdate()
    {
        if (target == null)
        {
            return;
        }

       if(cameraShake.getIsShake())
        {
            return;
        }

        // 목표 위치 계산
        Vector3 desiredPosition = target.position + offset;
        // 부드러운 이동 효과 적용
        //Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        Vector3 smoothedPosition = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothSpeed); // 부드럽게 이동
        // 카메라 위치 업데이트
        transform.position = smoothedPosition;

        // 카메라 회전 유지 (원하는 경우)
        // transform.rotation = target.rotation; // 예시: 목표 오브젝트와 동일하게 회전
    }
}