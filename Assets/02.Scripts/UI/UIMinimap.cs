using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class UIMinimap : MonoBehaviour
{
    [SerializeField] private Camera _minimapCamera;
    [SerializeField] private float _zoomMin = 1;
    [SerializeField] private float _zoomMax = 30;
    [SerializeField] private float _zoomOneStep = 1; // 1회 줌 할 때 증가/감소되는 수치
    [SerializeField] TextMeshProUGUI _textMapName;

    private void Awake()
    {
        _textMapName.text = SceneManager.GetActiveScene().name;
    }

    public void SetMinimapCamera(Camera cam)
    {
        _minimapCamera = cam;
    }

    
public void ZoomIn()
    {
        // 카메라의 orthographicSize를 감소시켜 카메라에 보이는 사물 크기 확대
        _minimapCamera.orthographicSize = Mathf.Max(_minimapCamera.orthographicSize - _zoomOneStep, _zoomMin);
    }

    public void ZoomOut()
    {
        _minimapCamera.orthographicSize = Mathf.Min(_minimapCamera.orthographicSize + _zoomOneStep, _zoomMax);
    }
}
