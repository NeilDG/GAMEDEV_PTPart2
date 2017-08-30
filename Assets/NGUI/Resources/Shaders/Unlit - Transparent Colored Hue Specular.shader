// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Unlit/Transparent Colored Hue Specular"
{
	Properties
	{
		_MainTex ("Base (RGB), Alpha (A)", 2D) = "black" {}
	}
	
	SubShader
	{
		LOD 200

		Tags
		{
			"Queue" = "Transparent"
			"IgnoreProjector" = "True"
			"RenderType" = "Transparent"
		}
		
		Pass
		{
			Cull Off
			Lighting Off
			ZWrite Off
			Fog { Mode Off }
			Offset -1, -1
			Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag			
			#include "UnityCG.cginc"

			sampler2D _MainTex;
			float4 _MainTex_ST;
	
			struct appdata_t
			{
				float4 vertex : POSITION;
				float2 texcoord : TEXCOORD0;
				fixed4 color : COLOR;
			};
	
			struct v2f
			{
				float4 vertex : SV_POSITION;
				half2 texcoord : TEXCOORD0;
				fixed4 color : COLOR;
			};
			
			float4 Overlay (float4 a, float4 b) 
			{
			    float4 r = float4(0,0,0,1);
			    if (a.r > 0.5) { r.r = 1-(1-2*(a.r-0.5))*(1-b.r); }
			    else { r.r = (2*a.r)*b.r; }
			    if (a.g > 0.5) { r.g = 1-(1-2*(a.g-0.5))*(1-b.g); }
			    else { r.g = (2*a.g)*b.g; }
			    if (a.b > 0.5) { r.b = 1-(1-2*(a.b-0.5))*(1-b.b); }
			    else { r.b = (2*a.b)*b.b; }
			    
			    float ave = (b.r + b.g + b.b) / 3;
			    if (ave > 0.9) { r = lerp(r, float4(1,1,1,1), (ave - 0.9) / 0.1); }
			    			    			    
			    return r;
			}
	
			v2f vert (appdata_t v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.texcoord = v.texcoord;
				o.color = v.color;
				return o;
			}
				
			fixed4 frag (v2f IN) : COLOR
			{
                fixed4 col = tex2D(_MainTex, IN.texcoord);
                col = fixed4(Overlay(IN.color, col).xyz, col.a * IN.color.a);
                return col;
			}
			ENDCG
		}
	}

	SubShader
	{
		LOD 100

		Tags
		{
			"Queue" = "Transparent"
			"IgnoreProjector" = "True"
			"RenderType" = "Transparent"
		}
		
		Pass
		{
			Cull Off
			Lighting Off
			ZWrite Off
			Fog { Mode Off }
			Offset -1, -1
			ColorMask RGB
			Blend SrcAlpha OneMinusSrcAlpha
			ColorMaterial AmbientAndDiffuse
			
			SetTexture [_MainTex]
			{
				Combine Texture * Primary
			}
		}
	}
}
