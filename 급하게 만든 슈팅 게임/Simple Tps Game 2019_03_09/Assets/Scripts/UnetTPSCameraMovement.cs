using UnityEngine;
using System.Collections;

public class UnetTPSCameraMovement : MonoBehaviour
{

    //public Transform GunTarget;
    private float x = 0.0f;
    private float y = 0.0f;

    [System.Serializable]
    public class CameraTarget
    {
        public Transform target;                //타겟
        public float cameraTargetHeight = 1.0f; //타겟의 높이
        public Transform weapon;
    }

    [SerializeField]
    public CameraTarget cameraTarget;

    [System.Serializable]
    public class User
    {
        [Header("-MouseSensitivity-")]
        public int mouseXSensitivity = 5;
        public int mouseYSensitivity = 5;
    }
    [SerializeField]
    public User user;

    [System.Serializable]
    public class CameraSettings
    {
        [Header("-Rate-")]
        public int ZoomRate = 20;
        public int lerpRate = 5;

        [Header("-Angle-")]
        public float MinAngle = -15f;
        public float MaxAngle = 25f;

        [Header("-Distance-")]
        public float dis = 5f;
        public float zoomViewDis = 3f;
        public float viewDis;
        public float correctedDis;
        public float currentDis;
    }
    [SerializeField]
    public CameraSettings cameraSettings;

    // Use this for initialization
    void Start()
    {
        //변수 초기화
        Vector3 Angles = transform.eulerAngles;
        x = Angles.x;
        y = Angles.y;
        cameraSettings.currentDis = cameraSettings.dis;
        cameraSettings.viewDis = cameraSettings.dis;
        cameraSettings.correctedDis = cameraSettings.dis;

        cameraTarget.target = FindObjectOfType<UnetPlayerMoveMent>().transform;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        //마우스 회전
        x += Input.GetAxis("Mouse X") * user.mouseXSensitivity;
        y += Input.GetAxis("Mouse Y") * user.mouseYSensitivity;

        //y가 범위를 넘지 않도록 도움
        y = Mathf.Clamp(y, cameraSettings.MinAngle, cameraSettings.MaxAngle);

        Quaternion rot = Quaternion.Euler(y, x, 0);

        //플레이어가 마우스 오른쪽 버튼을 눌렀을 때 줌, 아니면 일반 거리
        cameraSettings.viewDis = Input.GetMouseButton(1) ? Mathf.Lerp(cameraSettings.viewDis, cameraSettings.zoomViewDis, Time.deltaTime * cameraSettings.ZoomRate) : cameraSettings.dis;

        //조정된 시야가 범위를 넘지 않도록 도움
        cameraSettings.correctedDis = cameraSettings.viewDis;

        //오브젝트와 충돌 감지--------
        Vector3 pos = cameraTarget.target.position - (rot * Vector3.forward * cameraSettings.viewDis);

        RaycastHit hit;
        Vector3 cameraTargetPosition = new Vector3(cameraTarget.target.position.x, cameraTarget.target.position.y + cameraTarget.cameraTargetHeight, cameraTarget.target.position.z);

        bool isCorrected = false;

        if (Physics.Linecast(cameraTargetPosition, pos, out hit))
        {
            pos = hit.point;
            //카메라 타겟의 포지션과 hit point의 사이의 거리 구함
            cameraSettings.correctedDis = Vector3.Distance(cameraTargetPosition, pos);
            //거리가 보정된 것을 알려줌
            isCorrected = true;
        }
        //----------------------------

        //보정되거나 보정된 거리가 currentDistance보다 크면 Mathf.Lerp로 아니면 correctedDistance로
        cameraSettings.currentDis = !isCorrected || cameraSettings.correctedDis > cameraSettings.currentDis ? Mathf.Lerp(cameraSettings.currentDis, cameraSettings.correctedDis, Time.deltaTime * cameraSettings.ZoomRate) : cameraSettings.correctedDis;

        //최종 포지션
        pos = cameraTarget.target.position - (rot * Vector3.forward * cameraSettings.currentDis + new Vector3(0, -cameraTarget.cameraTargetHeight, 0));

        //포지션 값, 회전 값 변경
        transform.rotation = rot;
        transform.position = new Vector3(Mathf.Lerp(transform.position.x, pos.x, 5.0f), Mathf.Lerp(transform.position.y, pos.y, 5.0f), Mathf.Lerp(transform.position.z, pos.z, 5.0f));

        //LeftAlt = 자유 시점
        if (!Input.GetKey(KeyCode.LeftAlt))
        {
            //카메라의 x축과 타겟의 x축 맞춰줌
            float cameraX = transform.rotation.x;
            float cameraY = transform.rotation.y;
            float targetX = cameraTarget.target.rotation.x;
            cameraTarget.target.eulerAngles = new Vector3(cameraX, transform.eulerAngles.y, transform.eulerAngles.z);
        }
    }
}