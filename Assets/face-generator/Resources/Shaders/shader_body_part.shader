Shader "Shard/Sprites/SpriteDropShadow"
{
	Properties
	{
		[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
		_SkinColor("SkinColor", Color) = (1,1,1,1)
		_ShadowColor("ShadowColor", Color) = (1,1,1,1)
		_HighlightColor("HighlightColor", Color) = (1,1,1,1)
		[MaterialToggle] _EnableDropShadow("Enable Drop Shadow", Float) = 0
		_DropShadowOffset ("ShadowOffset", Vector) = (0,-0.1,0,0)
		
		[MaterialToggle] _RGBColorize("Color from RGB Channels", Float) = 0
		[MaterialToggle] _DoNotColorize("Do Not Colorize", Float) = 0
		
		[MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
		
	}

	SubShader
	{
		Tags
		{ 
			"Queue"="Overlay" 
			"IgnoreProjector"="True" 
			"RenderType"="Transparent" 
			"PreviewType"="Plane"
			"CanUseSpriteAtlas"="True"
		}

		Cull Off
		Lighting Off
		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha

		// draw shadow
		Pass
		{
		CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile _ PIXELSNAP_ON
			#include "UnityCG.cginc"
			
			struct appdata_t
			{
				float4 vertex   : POSITION;
				float4 color    : COLOR;
				float2 texcoord : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex   : SV_POSITION;
				fixed4 color    : COLOR;
				float2 texcoord  : TEXCOORD0;
			};
			
			fixed4 _ShadowColor;
			float4 _DropShadowOffset;
			float4 _MainTex_TexelSize;
			
			v2f vert(appdata_t IN)
			{
				v2f OUT;
				OUT.vertex = UnityObjectToClipPos(IN.vertex+_DropShadowOffset*_MainTex_TexelSize.y*4);
				OUT.texcoord = IN.texcoord;
				OUT.color = IN.color *_ShadowColor;
				#ifdef PIXELSNAP_ON
				OUT.vertex = UnityPixelSnap (OUT.vertex);
				#endif

				return OUT;
			}

			sampler2D _MainTex;
			sampler2D _AlphaTex;
			float _AlphaSplitEnabled;
			float _EnableDropShadow;

			fixed4 SampleSpriteTexture (float2 uv)
			{
				fixed4 color = tex2D (_MainTex, uv);
				color.rgb = _ShadowColor.rgb;

				#if UNITY_TEXTURE_ALPHASPLIT_ALLOWED
				if (_AlphaSplitEnabled)
					color.a = tex2D (_AlphaTex, uv).r;
				#endif //UNITY_TEXTURE_ALPHASPLIT_ALLOWED

				return color;
			}

			fixed4 frag(v2f IN) : SV_Target
			{
				fixed4 c = SampleSpriteTexture (IN.texcoord);
				fixed4 pixelDown = tex2D(_MainTex, IN.texcoord - fixed2(0, _MainTex_TexelSize.y));
				c.rgb *= c.a;
				return lerp(fixed4(0,0,0,0),c,_EnableDropShadow);
			}
		ENDCG
		}

		// draw real sprite
		Pass
		{
		CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile _ PIXELSNAP_ON
			#include "UnityCG.cginc"
			
			struct appdata_t
			{
				float4 vertex   : POSITION;
				float4 color    : COLOR;
				float2 texcoord : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex   : SV_POSITION;
				fixed4 color    : COLOR;
				float2 texcoord  : TEXCOORD0;
			};
			
			v2f vert(appdata_t IN)
			{
				v2f OUT;
				OUT.vertex = UnityObjectToClipPos(IN.vertex);
				OUT.texcoord = IN.texcoord;
				OUT.color = IN.color;
				#ifdef PIXELSNAP_ON
				OUT.vertex = UnityPixelSnap (OUT.vertex);
				#endif

				return OUT;
			}

			sampler2D _MainTex;
			sampler2D _AlphaTex;
			float _AlphaSplitEnabled;
			fixed4 _SkinColor;
			fixed4 _ShadowColor;
			fixed4 _HighlightColor;
			float _RGBColorize;
			float _DoNotColorize;

			fixed4 SampleSpriteTexture (float2 uv)
			{
				fixed4 color = tex2D (_MainTex, uv);

				#if UNITY_TEXTURE_ALPHASPLIT_ALLOWED
				if (_AlphaSplitEnabled)
					color.a = tex2D (_AlphaTex, uv).r;
				#endif //UNITY_TEXTURE_ALPHASPLIT_ALLOWED

				return color;
			}

			fixed4 frag(v2f IN) : SV_Target
			{
				fixed4 main = tex2D (_MainTex, IN.texcoord);
				
				fixed4 nonRGB = main.a * IN.color;
				
				float div = main.r / main.g / main.b;
				int bin = saturate(sign(main.r/div));

				fixed4 skin = main.a * ((main.r * _SkinColor)+(main.b * _ShadowColor)+(main.g * _HighlightColor));
				fixed4 obj = main.a * IN.color;
				fixed4 output = lerp(skin,obj,bin);
				
				fixed4 col = lerp(nonRGB,output,_RGBColorize);
				
				//return main;
				return lerp(col,main,_DoNotColorize);
			}
		ENDCG
		}
	}
}