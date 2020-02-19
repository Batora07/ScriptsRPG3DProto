Shader "CookbookShaders/OutlineAndDissolve" {
	Properties{
		_Color("Main Color", Color) = (.5,.5,.5,1)
		_OutlineColor("Outline Color", Color) = (0,0,0,1)
		_Outline("Outline width", Range(.002, 0.2)) = .005
		_OutlineZ("Outline Z", Range(-.002, 0)) = -.001// outline z offset
		_MainTex("Base (RGB)", 2D) = "white" { }
	_Offset("Outline Noise Offset", Range(0.5, 10)) = .005// noise offset
		_NoiseTex("Noise (RGB)", 2D) = "white" { }// noise texture
	_Ramp("Toon Ramp (RGB)", 2D) = "gray" {}

	[Toggle(NOISE)] _NOISE("Enable Noise?", Float) = 0

		_ColorDissolve("Color", Color) = (1,1,1,1)
		_MainTexDissolve("Albedo (RGB)", 2D) = "white" {}
		_SliceGuide("Slice Guide (RGB)", 2D) = "white" {}
		_SliceAmount("Slice Amount", Range(0.0, 1.0)) = 0

		_BurnSize("Burn Size", Range(0.0, 1.0)) = 0.15
		_BurnRamp("Burn Ramp (RGB)", 2D) = "white" {}
		_BurnColor("Burn Color", Color) = (1,1,1,1)

		_EmissionAmount("Emission amount", float) = 2.0
	}

		CGINCLUDE
#include "UnityCG.cginc"
#pragma shader_feature NOISE
		struct appdata {
		float4 vertex : POSITION;
		float3 normal : NORMAL;
		float4 texcoord : TEXCOORD0;// texture coordinates
	};

	struct v2f {
		float4 pos : SV_POSITION;
		UNITY_FOG_COORDS(0)
			fixed4 color : COLOR;
	};

	uniform float _Outline;
	uniform float _OutlineZ;// outline z offset
	uniform float4 _OutlineColor;
	sampler2D _NoiseTex;// noise texture
	float _Offset; // noise offset

	v2f vert(appdata v) {
		v2f o;
		o.pos = UnityObjectToClipPos(v.vertex);

		float3 norm = normalize(mul((float3x3)UNITY_MATRIX_IT_MV, v.normal));
		float2 offset = TransformViewToProjection(norm.xy);

		float4 tex = tex2Dlod(_NoiseTex, float4(v.texcoord.xy, 0, 0) * _Offset);// noise texture based on texture coordinates and offset

#ifdef UNITY_Z_0_FAR_FROM_CLIPSPACE //to handle recent standard asset package on older version of unity (before 5.5)
#if NOISE // switch for noise
		o.pos.xy += offset * _Outline * (tex.r);// add noise
#else
		o.pos.xy += offset * _Outline;// or not
#endif
		o.pos.z += _OutlineZ;// push away from camera
#else
		o.pos.xy += offset * o.pos.z * _Outline;
#endif
		o.color = _OutlineColor;
		UNITY_TRANSFER_FOG(o, o.pos);
		return o;
	}

	fixed4 _ColorDissolve;
	sampler2D _MainTexDissolve;
	sampler2D _SliceGuide;
	sampler2D _BumpMap;
	sampler2D _BurnRamp;
	fixed4 _BurnColor;
	float _BurnSize;
	float _SliceAmount;
	float _EmissionAmount;

	struct Input {
		float2 uv_MainTex;
	};

//	ENDCG

		SubShader{
		Tags{ "RenderType" = "Opaque" }
		UsePass "Toon/Lit/FORWARD"
		Pass{
		Name "OUTLINE"
		Tags{ "LightMode" = "Always" }
		LOD 200
		Cull Off// we dont want to cull
		ZWrite On
		ColorMask RGB


		CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#pragma multi_compile_fog
#pragma surface surf Lambert
#pragma target 3.0

	fixed4 frag(v2f i) : SV_Target
	{
		UNITY_APPLY_FOG(i.fogCoord, i.color);
		return i.color;
	}

	void surf(Input IN, inout SurfaceOutput o) {
		fixed4 c = tex2D(_MainTexDissolve, IN.uv_MainTex) * _ColorDissolve;
		half test = tex2D(_SliceGuide, IN.uv_MainTex).rgb - _SliceAmount;
		clip(test);

		if (test < _BurnSize && _SliceAmount > 0) {
			o.Emission = tex2D(_BurnRamp, float2(test * (1 / _BurnSize), 0)) * _BurnColor * _EmissionAmount;
		}

		o.Albedo = c.rgb;
		o.Alpha = c.a;
	}
	
	}
ENDCG

	Fallback "Toon/Basic"
	FallBack "Diffuse"
}



