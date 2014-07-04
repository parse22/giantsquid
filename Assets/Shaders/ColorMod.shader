Shader "Custom/ColorMod" {
Properties 
{
	_MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
	_MasterMap("_MasterMap (RGB) Trans (A)", 2D) = "black" {}
	
	_Color("MainColor", Color) = (0,0.1188812,1,1)
	_DirtStrength("_DirtStrength", Range(0.1,1) ) = 1
	_EmissiveStrength("_EmissiveStrength", Range(0,1)) = 1
	
	

}
	
SubShader 
{
	Tags
	{
		"Queue"="Geometry"
		"IgnoreProjector"="False"
		"RenderType"="Opaque"
	}

		
	Cull Back
	ZWrite On
	
	Fog{}
	
	CGPROGRAM
	#pragma surface surf Lambert
	#pragma target 3.0
	
	sampler2D _MainTex;
	sampler2D _MasterMap;
	
	float4 _Color;
	float _DirtStrength;
	float _EmissiveStrength;
	

	
	
	struct Input 
	{
		float2 uv_MainTex;
		float2 uv_MasterMap;
		float3 viewDir;
	};

	
		

	void surf (Input IN, inout SurfaceOutput o) //Editor
	{
	
		//TODO: b = detail map, make darker
		//r = color, g = emission, b = specular
		fixed4 tex = tex2D(_MainTex, IN.uv_MainTex);
		fixed4 map = tex2D(_MasterMap, IN.uv_MasterMap);
		
		float4 Multiply0 = lerp(tex,normalize(_Color),map.r);

		o.Alpha = map.g;//o.Alpha = tex.a;
		o.Albedo = Multiply0 * (1-(map.b*_DirtStrength));
		o.Gloss = 0.0;
		o.Specular = 0.0;
		o.Emission = 2 * Multiply0 * map.g * _EmissiveStrength;//UNITY_SAMPLE_1CHANNEL(map.y, IN.uv_MasterMap); //tex.rgb * UNITY_SAMPLE_1CHANNEL(_MainTex, IN.uv_MainTex);//
		
	}
	ENDCG
	}
	
	Fallback "Diffuse"
}