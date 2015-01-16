#ifndef VACUUM_FRAGMENTUM_CG_INCLUDED
#define VACUUM_FRAGMENTUM_CG_INCLUDED


//************************************************************************
//Properties
//************************************************************************

#ifdef SHADER_API_D3D11
CBUFFER_START(constants)
	StructuredBuffer<float3> buf_Center;
	StructuredBuffer<float3> buf_Normal;
	StructuredBuffer<float2> buf_UV;	

	StructuredBuffer<float4> buf_RandomVector;	

	float _Devider;
CBUFFER_END
#endif



sampler2D _FragTex;
float4 _FragTex_ST;

sampler2D _MainTex;
float4 _MainTex_ST;

half _DisAmount;

float _FragTexStrength;
half _FragPow;
half _FragmentScale;
float _Lock;

#ifndef V_FR_ACTIVATOR_NONE
	half _ActivatorPow;
#endif

#if defined(V_FR_ACTIVATOR_PLANE)
	half4 _PlanePos;
	half4 _PlaneRot;
	half _DistanceToPlane;
#endif

#if defined(V_FR_ACTIVATOR_SPHERE)
	half4 _SphereObject; // xyz - pos, w - radius
#endif



//Properties
fixed4 _Color;
half _Shininess;

fixed _Emission;


//************************************************************************
//Macros
//************************************************************************

#define V_FR_INDEX (round(v.texcoord1.x * _Devider))

//Macros


//************************************************************************
//Structures
//************************************************************************
struct Input   
{			
	float2 MainTexUV;			 
};	 

//Structures


//************************************************************************
//Functions
//************************************************************************

inline void ScaleFragment(inout float3 vertex, int index, half amount)
{
	//Correct size 
	_FragmentScale = 1 - _FragmentScale;
	_FragmentScale *= abs(amount);
	_FragmentScale = min(_FragmentScale, 1);

		  
	//Vector toward center
	float3 toCenter = buf_Center[index] - vertex;
	  
	//Scale fragment
	vertex += toCenter * _FragmentScale;
}

inline float3 V_FR_WorldSpaceViewDir( in float4 v )
{
	return _WorldSpaceCameraPos.xyz - mul(_Object2World, v).xyz;
}

//Functions


//************************************************************************
//Verext 
//************************************************************************
void vert (inout appdata_full v, out Input o)
{
	UNITY_INITIALIZE_OUTPUT(Input, o);

	o.MainTexUV = TRANSFORM_TEX (v.texcoord, _MainTex);
	
			
	#ifdef V_FR_EDITOR_OFF
	  
		//Get Index 
		int index = V_FR_INDEX; 

	
		//Fragment Factor
		float4 fUV = float4(TRANSFORM_TEX (buf_UV[index], _FragTex), 0, 0);
		half fragmentArea = tex2Dlod(_FragTex, fUV).r;
		fragmentArea = pow(fragmentArea, _FragPow);
		fragmentArea *= _FragTexStrength;
		 
		
		float fragmentFactor = 1;

		#if defined(V_FR_ACTIVATOR_PLANE)
			float4 worldPos = mul(_Object2World, float4(buf_Center[index], 1));
			float3 toP = worldPos - _PlanePos; 
			float planeMult = dot(normalize(_PlaneRot), toP);
			
			
			planeMult += _DistanceToPlane;

			//Always have positive value
			planeMult = max(0, planeMult);
			

			fragmentFactor = planeMult;		
			
			if(_Lock == 1)
				fragmentFactor = min(fragmentFactor, 1);		
		#endif

		#if defined(V_FR_ACTIVATOR_SPHERE)
			float3 worldPos = mul(_Object2World, float4(buf_Center[index], 1)).xyz;
			float3 spherePos = _SphereObject.xyz;
			float sphereRadius = _SphereObject.w;

			float dist = distance(worldPos, spherePos);	

			if(sphereRadius > 0)
			{
				dist = min(dist, sphereRadius);
				fragmentFactor = -dist + sphereRadius;	
			}
			else
			{
			    dist = max(0, dist - abs(sphereRadius));
				fragmentFactor = dist;
			}				
			
			if(_Lock == 1)
				fragmentFactor = min(fragmentFactor, 1);							
		#endif

			
		//Update displace amount;
		_DisAmount *= fragmentArea * fragmentFactor;

		//Scale fragment
		ScaleFragment(v.vertex.xyz, index, fragmentArea * fragmentFactor);

	

		//Fragment move direction		
		float3 dir = buf_Normal[index];

		//Noise
						  

		//Rotate	
				

		v.vertex.xyz += dir * _DisAmount;
		
	
	
	#endif //V_FR_EDITOR_OFF  	     
}
//Vertex


void surf (Input IN, inout SurfaceOutput o) 
{
	fixed4 tex = tex2D(_MainTex, IN.MainTexUV);	
	tex.rgb *= _Color.rgb;
	
	o.Albedo = tex.rgb;		

	o.Emission = o.Albedo * _Emission;	
	o.Alpha = tex.a  * _Color.a;

}

#endif