﻿Shader "CookbookShaders/ScrollingShader" {
	Properties {
		_MainTint ("Diffuse Tint", Color) = (1,1,1,1)
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_ScrollXSpeed ("X Scroll Speed", Range(0,10)) = 2
		_ScrollYSpeed ("Y Scroll Speed", Range(0,10)) = 2
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		struct Input {
			float2 uv_MainTex;
		};

		fixed4 _MainTint;
		fixed _ScrollXSpeed;
		fixed _ScrollYSpeed;
		sampler2D _MainTex;

		void surf (Input IN, inout SurfaceOutputStandard o) {
			// Create a separate variable to store our UVs
			// before we pass them to the tex2D() function
			fixed2 scrolledUV = IN.uv_MainTex;

			// create variables that store the individual x and y 
			// components for the UV's scaled by time
			fixed xScrollValue = _ScrollXSpeed * _Time.y;
			fixed yScrollValue = _ScrollYSpeed * _Time.y;

			// Apply the final UV offset
			scrolledUV += fixed2(xScrollValue, yScrollValue);

			// Apply textures and tint
			half4 c = tex2D(_MainTex, scrolledUV);
			o.Albedo = c.rgb * _MainTint;
			o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}