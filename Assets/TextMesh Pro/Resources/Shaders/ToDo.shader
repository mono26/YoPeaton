
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: commented out 'float3 _WorldSpaceCameraPos', a built-in variable
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "custom/Juego1"
{
	Properties
	{	
		_MainTex("Tex2D", 2D) = "white" {}
		//_SecTex("Humo", 2D) = "black" {}
	}

	SubShader
	{
		Pass
		{
			ZWrite Off
			Blend SrcAlpha OneMinusSrcAlpha
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			sampler2D _MainTex;
			float3 WorldSpacePlayerPosition;
			float PlayerRadius;
			//sampler2D _SecTex;

			struct appdata
			{
				float4 vertexObject : POSITION;
				float2 uv : TEXCOORD0;	
			};

			struct v2f
			{				
				float4 vertexScreen : SV_POSITION;
				float4 vertexWorld : TEXCOORD1;
				float2 uv : TEXCOORD0;
			};
			
			v2f vert (appdata v)
			{				
				v2f o;
				o.vertexWorld = mul(unity_ObjectToWorld, v.vertexObject);
				o.vertexScreen = UnityObjectToClipPos(v.vertexObject);
				o.uv = v.uv;
				return o;
			}
			
			float4 frag (v2f i) : COLOR
			{
				float distancia = distance(i.vertexWorld, WorldSpacePlayerPosition);
				float4 tex = tex2D(_MainTex, float2(i.uv.x + _Time.x, i.uv.y)).rrrr * float4(0.2,0.2,0.2,0.6);
				if (distancia < PlayerRadius)
				{
					discard;
				}
				
				return tex;
			}

			ENDCG
		}
	}
}
