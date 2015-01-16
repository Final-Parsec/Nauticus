using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using System.Linq;

[CustomEditor(typeof(Fragmentum))]
public class Fragmentum_Editor : Editor
{
    Fragmentum _target;
    Texture2D icon;
    Texture2D facebookIcon;
    Texture2D youtubeIcon;

    static bool bAbout;
    static bool bInfo;

    
    GUIStyle textStyle = new GUIStyle();

    Mesh mesh;

    enum V_FR_ERROR {Ok, InvalidRenderer, NoMaterials, MaterialNull, NotFractumMaterial, NoSkinnedMeshRendererSupport };
    V_FR_ERROR error;

    public virtual void OnEnable()
    {
        _target = (Fragmentum)target;

        
        if (_target.GetComponent<Renderer>() == null)
        {            
            error = V_FR_ERROR.InvalidRenderer;
        }

        if (_target.renderer.sharedMaterials.Length == 0)
        {
            error = V_FR_ERROR.NoMaterials;
        }

        if (_target.GetComponent<SkinnedMeshRenderer>())
        {
            error = V_FR_ERROR.NoSkinnedMeshRendererSupport;
        }
        else if (_target.GetComponent<MeshFilter>())
            mesh = _target.GetComponent<MeshFilter>().sharedMesh;
        else
            error = V_FR_ERROR.InvalidRenderer;



        icon = Resources.Load("Fragmentum_Icon_Editor") as Texture2D;

        facebookIcon = Resources.Load("facebook_icon") as Texture2D;
        youtubeIcon = Resources.Load("youtube_icon") as Texture2D;
               
        textStyle.fontStyle = FontStyle.Bold;
    }

    public override void OnInspectorGUI()
    {
        if (_target.enabled == false)
            return;

        if (error != V_FR_ERROR.Ok && error != V_FR_ERROR.NotFractumMaterial)
        {
            GUILayout.Space(5);

            textStyle.normal.textColor = Color.red;
            EditorGUILayout.LabelField("Warning: " + error.ToString(), textStyle);

            About();

            return;
        }


        EditorGUI.BeginDisabledGroup(Application.isPlaying);
        {
            GUILayout.Space(10);

            string fragmentumTag = "FragmentumTag";
            string tag = _target.renderer.sharedMaterial.GetTag(fragmentumTag, false, "nothing");

            if (tag.IndexOf("Fragmentum") == -1)
            {
                error = V_FR_ERROR.NotFractumMaterial;

                GUILayout.Space(5);
                textStyle.normal.textColor = Color.red;
                EditorGUILayout.LabelField("Warning: " + error.ToString(), textStyle);

                About();

                return;
            }
            
        EditorGUI.EndDisabledGroup();



            GUILayout.Space(5);
            //Activator
            EditorGUI.BeginDisabledGroup(Application.isPlaying);
            _target.fActivator = (Fragmentum.FRAGMENTUM_ACTIVATOR)EditorGUILayout.EnumPopup("Activator", _target.fActivator);
            EditorGUI.EndDisabledGroup();
            if (_target.fActivator == Fragmentum.FRAGMENTUM_ACTIVATOR.Plane)
            {
                if (!_target.planeObject)
                    GUI.backgroundColor = Color.red;
                _target.planeObject = (Transform)EditorGUILayout.ObjectField("   Activator Object", _target.planeObject, typeof(Transform), true);
                GUI.backgroundColor = Color.white;
            }

            if (_target.fActivator == Fragmentum.FRAGMENTUM_ACTIVATOR.Sphere)
            {
                if (!_target.sphereObject)
                    GUI.backgroundColor = Color.red;

                _target.sphereObject = (Transform)EditorGUILayout.ObjectField("   Activator Object", _target.sphereObject, typeof(Transform), true);
                GUI.backgroundColor = Color.white;

                if (_target.sphereObjectRadius == 0)
                    GUI.backgroundColor = Color.red;
                _target.sphereObjectRadius = EditorGUILayout.FloatField("   Activator Radius", _target.sphereObjectRadius);
                GUI.backgroundColor = Color.white;
            }
          
        }
        
        

        EditorGUI.BeginDisabledGroup(Application.isPlaying);
        {
            GUILayout.Space(5);

            //_target.bUseNoise = EditorGUILayout.Toggle("Use Noise", _target.bUseNoise);
            
            UpdateShader();
        }
        
        EditorGUI.EndDisabledGroup();

        GUILayout.Space(10);
        _target.useUpdateFunction = EditorGUILayout.Toggle("Use Update Function", _target.useUpdateFunction);

        //Info();

        About();

    }

    public virtual void OnDisable()
    {
        _target = null;
    }

    void OnSceneGUI()
    {
        if (_target == null || error != V_FR_ERROR.Ok)
            return;
        
                

        if (_target.fActivator == Fragmentum.FRAGMENTUM_ACTIVATOR.Plane && _target.planeObject != null)
        {
            if (_target.planeObject.gameObject.GetComponent<FragmentumHelper>() == null)
                _target.planeObject.gameObject.AddComponent<FragmentumHelper>();

            Handles.DrawLine(_target.renderer.bounds.center, _target.planeObject.position);
            Handles.Label(_target.planeObject.position, "  Plane helper", textStyle);
        }

        if (_target.fActivator == Fragmentum.FRAGMENTUM_ACTIVATOR.Sphere && _target.sphereObject != null)
        {
            if (_target.sphereObject.gameObject.GetComponent<FragmentumHelper>() == null)
                _target.sphereObject.gameObject.AddComponent<FragmentumHelper>();

            Handles.DrawLine(_target.renderer.bounds.center, _target.sphereObject.position);
            Handles.Label(_target.sphereObject.position, "  Sphere helper", textStyle);
            _target.sphereObjectRadius = Handles.RadiusHandle(Quaternion.identity, _target.sphereObject.position, _target.sphereObjectRadius);
        }
    }
    

    void UpdateShader()
    {
        if (error != V_FR_ERROR.Ok)
            return;

        string matName = _target.renderer.sharedMaterial.ToString();

        if (matName.IndexOf("null") != -1)
        {
            error = V_FR_ERROR.MaterialNull;
            return;
        }

        string fragmentumTag = "FragmentumTag";
        string tag = _target.renderer.sharedMaterial.GetTag(fragmentumTag, false, "nothing");

        if(tag.IndexOf("Fragmentum") == -1)
        {
            error = V_FR_ERROR.NotFractumMaterial;

            return;
        }

        
        ModifyKeyWords();
        
    }

    void Info()
    {
        if (error != V_FR_ERROR.Ok)
            return;

        GUILayout.Space(5);

        bInfo = EditorGUILayout.Foldout(bInfo, "Info");
        if (bInfo)
        {
            if (mesh != null)
            {
                textStyle.normal.textColor = Color.gray;
                EditorGUILayout.LabelField("  Current vertex count: " + mesh.vertexCount, textStyle);
                EditorGUILayout.LabelField("  Current triangle count: " + mesh.triangles.Length / 3, textStyle);

                int genVertexCount = mesh.triangles.Length;
                if (genVertexCount > 65000)
                    textStyle.normal.textColor = Color.red;

                int percent = (int)(100 * (genVertexCount / (float)mesh.vertexCount - 1));
                EditorGUILayout.LabelField("  Generated vertex count: " + genVertexCount + " (+" + percent + "%)", textStyle);
            }
        }

    }

    void About()
    {
        GUILayout.Space(5);

        bAbout = EditorGUILayout.Foldout(bAbout, "About");
        if (bAbout)
        {
            GUILayout.Space(10);
            GUILayout.Space(10);
            EditorGUILayout.BeginHorizontal();

            GUILayout.Space(10);
            GUILayout.Box(icon, new GUIStyle());

            EditorGUILayout.BeginVertical();
            EditorGUILayout.LabelField(Fragmentum.version);
            EditorGUILayout.LabelField("by Davit Naskidashvili");
            EditorGUILayout.LabelField("2013");
            EditorGUILayout.EndVertical();

            EditorGUILayout.EndHorizontal();


            GUILayout.Space(15);
            EditorGUILayout.BeginHorizontal();

            //if (GUILayout.Button("My assets", GUILayout.Width(100)))
            //{
            //    Application.OpenURL("https://www.assetstore.unity3d.com/#/publisher/1295");
            //}

            GUILayout.Space(10);
            if (GUILayout.Button("Fragmentum Forum", GUILayout.Width(130), GUILayout.Height(28)))
            {
                Application.OpenURL("http://forum.unity3d.com/threads/178616-Fragmentum-DirectX-11-contest-shader?p=1221534#post1221534");
            }

            GUILayout.Space(10);
            GUI.backgroundColor = new Color(0.275f, 0.424f, 0.690f);
            if (GUILayout.Button(facebookIcon, new GUIStyle(), GUILayout.Width(32), GUILayout.Height(32)))
            {
                Application.OpenURL("https://www.facebook.com/pages/Vacuum/645071998850267?ref=hl");
            }
            GUI.backgroundColor = Color.white;

            GUILayout.Space(10);
            if (GUILayout.Button(youtubeIcon, new GUIStyle(), GUILayout.Width(32), GUILayout.Height(32)))
            {
                Application.OpenURL("http://www.youtube.com/user/Arxivrag/videos");
            }

            EditorGUILayout.EndHorizontal();

            GUILayout.Space(10);
        }
    }

    void ModifyKeyWords()
    {
        if (Application.isPlaying == true)
            return;


        Material targetMat = _target.renderer.sharedMaterial;

        List<string> keywords = new List<string>();
                    

        switch (_target.fActivator)
        {
            case Fragmentum.FRAGMENTUM_ACTIVATOR.Plane:
                keywords.Add("V_FR_ACTIVATOR_PLANE");
                break;

            case Fragmentum.FRAGMENTUM_ACTIVATOR.Sphere:
                keywords.Add("V_FR_ACTIVATOR_SPHERE");
                break;

            default:
                keywords.Add("V_FR_ACTIVATOR_NONE");
                break;
        }




        keywords.Add("V_FR_EDITOR_ON");

        targetMat.shaderKeywords = keywords.ToArray();
        EditorUtility.SetDirty(targetMat);
    }
}
