using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(FragmentumHelper))]
public class FragmentumHelper_Editor : Editor
{

    FragmentumHelper _target;

    public virtual void OnEnable()
    {
        _target = (FragmentumHelper)target;
    }

    void OnSceneGUI()
    {        
        if (_target == null)
            return;

        float size = HandleUtility.GetHandleSize(_target.transform.position) * 0.75f;

        Handles.color = new Color(1, 1, 1, 0.1f);
        Handles.DrawSolidDisc(_target.transform.position, _target.transform.up, size);

        Handles.color = new Color(1, 1, 1, 0.7f);
        Handles.ArrowCap(0, _target.transform.position, Quaternion.LookRotation(_target.transform.up), size);
    }	
}
