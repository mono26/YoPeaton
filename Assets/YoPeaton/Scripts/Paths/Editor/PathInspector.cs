using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Path))]
public class PathInspector : Editor
{
    private Path path;
    private Transform handleTransform;
    private Quaternion handleRotation;

    private void OnSceneGUI () {
        path = target as Path;
        handleTransform = path.transform;
        handleRotation = Tools.pivotRotation == PivotRotation.Local ? handleTransform.rotation : Quaternion.identity;

        Vector3 p0 = ShowPoint(0);
        Vector3 p1 = ShowPoint(1);
        Vector3 p2 = ShowPoint(2);

        Handles.color = Color.white;
        Handles.DrawLine(p0, p1);
        Handles.DrawLine(p1, p2);
    }

    private Vector3 ShowPoint (int index) {
        Vector3 point = handleTransform.TransformPoint(path.points[index]);
        EditorGUI.BeginChangeCheck();
        point = Handles.DoPositionHandle(point, handleRotation);
        if (EditorGUI.EndChangeCheck()) {
            Undo.RecordObject(path, "Move Point");
            EditorUtility.SetDirty(path);
            path.points[index] = handleTransform.InverseTransformPoint(point);
        }
        return point;
    }
}
