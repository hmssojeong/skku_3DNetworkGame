using UnityEngine;

public class CopyPosition : MonoBehaviour
{
    [SerializeField] private bool x, y, z; // 이 값이 true이면 target의 좌표, false이면 현재 좌표를 그대로 사용
    [SerializeField] Transform target; // 쫒아가야할 대상 Transform

private void Update()
    {
        if (target == null)
        {
            foreach (var controller in FindObjectsOfType<PlayerController>())
            {
                if (controller.PhotonView.IsMine)
                {
                    target = controller.transform;
                    break;
                }
            }
            return;
        }

        transform.position = new Vector3(
            (x ? target.position.x : transform.position.x),
            (y ? target.position.y : transform.position.y),
            (z ? target.position.z : transform.position.z));
    }
}
