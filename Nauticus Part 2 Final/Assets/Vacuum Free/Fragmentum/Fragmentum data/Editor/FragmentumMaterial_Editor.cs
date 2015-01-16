using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Linq;

public class FragmentumMaterial_Editor : MaterialEditor
{
    Material targetMaterial;    
    string[] keyWords;


    static bool bFParameters = true;
    static bool bVParameters = true;



    bool bIsExtrude;


    MaterialProperty _FragTex;
    MaterialProperty _FragTexStrength;
    MaterialProperty _FragPow;

    MaterialProperty _DisAmount;
    MaterialProperty _FragmentScale;
    MaterialProperty _DistanceToPlane;


    MaterialProperty _Color;
    MaterialProperty _MainTex;
    MaterialProperty _Emission;


    public override void OnEnable()
    {
        base.OnEnable();


        targetMaterial = target as Material;
        keyWords = targetMaterial.shaderKeywords;
        

        Material[] mats = new Material[] { targetMaterial };

        _FragTex = GetMaterialProperty(mats, "_FragTex");
        _FragTexStrength = GetMaterialProperty(mats, "_FragTexStrength");
        _FragPow = GetMaterialProperty(mats, "_FragPow");

        _DisAmount = GetMaterialProperty(mats, "_DisAmount");
        

        _FragmentScale = GetMaterialProperty(mats, "_FragmentScale");
        _DistanceToPlane = GetMaterialProperty(mats, "_DistanceToPlane");
             


        _Color = GetMaterialProperty(mats, "_Color");
        _MainTex = GetMaterialProperty(mats, "_MainTex");
        _Emission = GetMaterialProperty(mats, "_Emission");

    }

    public override void OnInspectorGUI()
    {
        if (isVisible == false)
            return;


        base.OnInspectorGUI();
        

        bFParameters = EditorGUILayout.Foldout(bFParameters, "Fragmentum Parameters");
        if (bFParameters)
        {
            GUILayout.Space(5);
            /////////////////////////////////////////////////////////////////////////////////
            _FragTex.textureValue = TextureProperty(_FragTex, "   Fragment Texture(R)");

            _FragTexStrength.floatValue = RangeProperty(_FragTexStrength, string.Format("   Fragment Texture Strength  [{0}]", _FragTexStrength.floatValue.ToString("F3")));
            _FragPow.floatValue = RangeProperty(_FragPow, string.Format("   Fragment Area Pow             [{0}]", _FragPow.floatValue.ToString("F3")));

            _DisAmount.floatValue = FloatProperty(_DisAmount, "   Displace Amount");


                        
            
            _FragmentScale.floatValue = FloatProperty(_FragmentScale, "   Fragment Scale");

            //Lock
            if (keyWords.Contains("V_FR_ACTIVATOR_NONE") == false)
            {
                bool bLock = false;
                if (targetMaterial.GetFloat("_Lock") == 1)
                    bLock = true;

                bLock = EditorGUILayout.Toggle("   Lock Displace", bLock);

                targetMaterial.SetFloat("_Lock", bLock ? 1 : 0);

                if (keyWords.Contains("V_FR_ACTIVATOR_PLANE"))
                {
                    _DistanceToPlane.floatValue = FloatProperty(_DistanceToPlane, "   Distance To Activator");
                }
            }


            //Rotation 
        }

        CommonParameters();

    }

    void CommonParameters()
    {
        GUILayout.Space(5);
        bVParameters = EditorGUILayout.Foldout(bVParameters, "Visual Parameters");
        if(bVParameters)
        {
            GUILayout.Space(5);

            
            _Color.colorValue = ColorProperty(_Color, "   Main Color");
            _MainTex.textureValue = TextureProperty(_MainTex, "   Main Texture(RGB) Alpha(A)");
                
            _Emission.floatValue = FloatProperty(_Emission, "   Emission");
        }        
                
    }
}
