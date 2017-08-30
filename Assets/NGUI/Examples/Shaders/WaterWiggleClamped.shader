// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "FX/WaterWiggleClamped"
{
	Properties
	{
		_MainTex ("Sprite Texture", 2D) = "white" {}
		_Color ("Tint", Color) = (1,1,1,1)
		_WiggleTex ("Base (RGB)", 2D) = "white" {}
		_WiggleStrength ("Wiggle Strength", Range (0.01, 0.1)) = 0.01
		_WiggleSpeed ("Wiggle Speed", Float) = 1
		_WhiteColor ("WhiteColor", Color) = (1, 1, 1, 1)
		_WiggleMask ("Wiggle Mask", 2D) = "White" {}
	}

	SubShader
	{
		Tags
		{
			"Queue"="Transparent+6" 
			"IgnoreProjector"="True" 
			"RenderType"="Transparent" 
		}

		Cull Off
		Lighting Off
		ZWrite Off
		Fog { Mode Off }
		Blend One OneMinusSrcAlpha

		Pass
		{	
		CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
			
			struct appdata_t
			{
				float4 vertex   : POSITION;
				float4 color    : COLOR;
				float2 texcoord : TEXCOORD0;
				float2 texcoord1 : TEXCOORD1;
			};

			struct v2f
			{
				float4 vertex   : SV_POSITION;
				fixed4 color    : COLOR;
				half2 texcoord  : TEXCOORD0;
				half2 displacement : TEXCOORD1;
			};
			
			fixed4 _Color;
			sampler2D _MainTex;
			sampler2D _WiggleTex;
			sampler2D _AnimMask;
			sampler2D _WiggleMask;
			fixed _WiggleStrength;
			float _WiggleSpeed;
			fixed4 _WhiteColor;

			v2f vert(appdata_t IN)
			{
				v2f OUT;
				OUT.vertex = UnityObjectToClipPos(IN.vertex);
				OUT.texcoord = IN.texcoord;
				OUT.displacement = IN.texcoord1;
				OUT.color = IN.color * _Color;

				return OUT;
			}


			fixed4 frag(v2f IN) : SV_Target
			{
				fixed2 tc2 = IN.displacement;
				tc2.x -= _SinTime * _WiggleSpeed;
				tc2.y += _CosTime * _WiggleSpeed;
				fixed4 wiggle = tex2D(_WiggleTex, tc2);
				
				fixed4 wm = tex2D(_WiggleMask,IN.displacement);
				
				IN.texcoord.x += wiggle.r * _WiggleStrength*0.5f * wm.r;
				IN.texcoord.y += wiggle.b * _WiggleStrength*0.0f;
									
				
				fixed4 c = tex2D(_MainTex, IN.texcoord) * _Color;
				if (c.r >= _WhiteColor.r && c.g >= _WhiteColor.g && c.b >= _WhiteColor.b)
					c.rgb += dot(c.xyz, wiggle.xyz) * wiggle.r;
				c.rgb *= c.a;
				return c;
			}
		ENDCG
		}
	}
}
