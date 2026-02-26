using UnityEngine;
using UnityEditor;

public class RectPivotTool : EditorWindow
{
    [MenuItem("UI Utilities/Set Pivot to Center %g")] // Ctrl+G 단축키
    static void SetPivotCenter() => SetPivot(new Vector2(0.5f, 0.5f));

    [MenuItem("UI Utilities/Set Pivot to Top Left")]
    static void SetPivotTopLeft() => SetPivot(new Vector2(0f, 1f));

    static void SetPivot(Vector2 pivot)
    {
        GameObject[] selectedObjects = Selection.gameObjects;

        foreach (GameObject go in selectedObjects)
        {
            RectTransform rectTransform = go.GetComponent<RectTransform>();
            if (rectTransform == null) continue;

            // 변경 전의 위치 정보를 저장 (Undo 가능하게 설정)
            Undo.RecordObject(rectTransform, "Change Pivot");

            // 핵심 로직: 피벗 변경으로 인한 위치 변화 보정
            Vector2 size = rectTransform.rect.size;
            Vector3 deltaPivot = rectTransform.pivot - pivot;
            Vector3 deltaPosition = new Vector3(deltaPivot.x * size.x, deltaPivot.y * size.y, 0);

            // 로컬 스케일을 고려하여 위치 보정
            deltaPosition.x *= rectTransform.localScale.x;
            deltaPosition.y *= rectTransform.localScale.y;

            rectTransform.pivot = pivot;
            rectTransform.localPosition -= deltaPosition;
        }
    }
}