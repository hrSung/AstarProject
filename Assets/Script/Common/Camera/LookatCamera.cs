using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Camera))]
public class LookatCamera : MonoBehaviour
{
    private new Transform transform;
    public new Camera camera;

    [SerializeField]
    bool m_isEnableFocusing = false;
    [SerializeField]
    Transform m_tfFocusingTarget = null;
    [SerializeField]
    Vector3 m_vFocusingOffset = new Vector3(-1.12f, 1, -2);

    public bool EnableTargetFocusing { get { return m_isEnableFocusing; } set { m_isEnableFocusing = value; } }
    public Transform tfFocusTarget { get { return m_tfFocusingTarget; } set { m_tfFocusingTarget = value; m_isEnableFocusing = value != null; } }
    public Vector3 vFocusingPosition { get { return m_vFocusingOffset; } set { m_vFocusingOffset = value; } }

    bool m_isEnableZoom = false;
    [SerializeField]
    float m_fPinchZoomSpeed = 0.3f;
    private float m_fPrevPinchDist = 0f;
    private bool m_isZooming = false;

    public bool EnablePinchZoom { get { return m_isEnableZoom; } set { m_isEnableZoom = value; } }
    public bool IsZooming { get { return m_isZooming; } }

    private bool m_isCameraWork = false;
    public bool IsCamaeraWork { get { return m_isCameraWork; } }
    private Tween m_tween;
    public delegate void CameraWorkFinish();
    private CameraWorkFinish OnCameraWorkFinish;
    private float m_fCameraWorkTimeLimit;
    private float m_fCameraWorkTimeCurr;

    private void Awake()
    {
        transform = base.transform;
        camera = base.GetComponent<Camera>();

        //float[] distances = new float[32];
        //distances[10] = 50;
        //camera.layerCullDistances = distances;
    }

    private void LateUpdate()
    {
        if (m_isCameraWork)
        {
            int tick;
            if (null != m_tween && m_tween.tick < m_tween.totalTick)
            {
                tick = m_tween.tick;

                if (null != m_tween.fov)
                {
                    camera.fieldOfView = m_tween.fov[tick];
                }

                if (null != m_tween.x)
                {
                    camera.transform.localPosition = new Vector3(m_tween.x[tick], m_tween.y[tick], m_tween.z[tick]);
                }
            }

            if (0.0f < m_fCameraWorkTimeLimit)
            {
                float deltaTime = Time.deltaTime;
                m_fCameraWorkTimeCurr += deltaTime;

                tick = m_tween.tick;
                m_tween.tick = Mathf.CeilToInt((m_fCameraWorkTimeCurr / m_fCameraWorkTimeLimit) * (float)m_tween.totalTick);
                if (m_tween.tick == tick)
                {
                    m_tween.tick++;
                }

                if (m_tween.tick >= m_tween.totalTick)
                {
                    m_tween.tick = m_tween.totalTick;
                }
            }
            else
            {
                m_tween.tick++;
            }

            if (m_tween.tick >= m_tween.totalTick)
            {
                m_isCameraWork = false;
                m_tween = null;

                if (null != OnCameraWorkFinish)
                {
                    OnCameraWorkFinish();
                    OnCameraWorkFinish = null;
                }
            }
        }
        else
        {
            if (m_isEnableZoom)
                camera.fieldOfView = ZoomProcess(camera.fieldOfView);
        }

        if (EnableTargetFocusing && m_tfFocusingTarget != null)
        {
            Vector3 vLookAtPos = m_tfFocusingTarget.position + m_vFocusingOffset;
            transform.LookAt(vLookAtPos);
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (EnableTargetFocusing && m_tfFocusingTarget != null)
        {
            Vector3 vLookAtPos = m_tfFocusingTarget.position + m_vFocusingOffset;
            base.transform.LookAt(vLookAtPos);
        }
    }
#endif

    private float ZoomProcess(float fFov)
    {
        // TODO: Status UI 좌표 및 크기 값 갱신.
        // 유니티 에디터에선 휠 스크롤.
#if UNITY_EDITOR
        float fWheel = Input.GetAxis("Mouse ScrollWheel");

        if (fWheel == 0) return fFov;

        fFov = fFov + -fWheel * 10f;

        // 모바일 기기에선 핀치.
#elif UNITY_ANDROID || UNITY_IPHONE
        if (Input.touchCount < 2)
        {
            m_fPrevPinchDist = 0f;
            m_isZooming = false;
            return fFov;
        }

        float fDistance = Vector2.Distance(Input.GetTouch(0).position, Input.GetTouch(1).position);

        if (m_fPrevPinchDist != 0f)
        {
            fFov = fFov - (fDistance - m_fPrevPinchDist) * m_fPinchZoomSpeed;
        }

        m_fPrevPinchDist = fDistance;
        m_isZooming = true;
#endif

        return fFov;
    }


    public void AddCullingLayer(int nLayer)
    {
        int nMaskedLayer = camera.cullingMask;

        nLayer = 1 << nLayer;

        nMaskedLayer |= nLayer;
        camera.cullingMask = nMaskedLayer;
    }
    public void EraseCullingLayer(int nLayer)
    {
        int nMaskedLayer = camera.cullingMask;

        nLayer = 1 << nLayer;

        nMaskedLayer ^= nLayer;
        camera.cullingMask = nMaskedLayer;
    }

    public void CameraWork(int _nTweenLength, Tween _tween, CameraWorkFinish _CameraWorkFinish = null)
    {
        m_isCameraWork = true;
        m_tween = _tween;
        OnCameraWorkFinish = _CameraWorkFinish;

        m_fCameraWorkTimeLimit = 0.0f;
    }

    public void CameraWork(int _nTweenLength, Tween _tween, float _time, CameraWorkFinish _CameraWorkFinish = null)
    {
        m_isCameraWork = true;
        m_tween = _tween;
        m_fCameraWorkTimeLimit = _time;
        m_fCameraWorkTimeCurr = 0.0f;
        OnCameraWorkFinish = _CameraWorkFinish;
    }
}
