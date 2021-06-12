// Unity built-in shader source. Copyright (c) 2016 Unity Technologies. MIT license (see license.txt)

Shader "Customs/SpritePalette"
{
	
	
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        [MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
        [HideInInspector] _RendererColor ("RendererColor", Color) = (1,1,1,1)
        [HideInInspector] _Flip ("Flip", Vector) = (1,1,1,1)
        [PerRendererData] _AlphaTex ("External Alpha", 2D) = "white" {}
        [PerRendererData] _EnableExternalAlpha ("Enable External Alpha", Float) = 0

		_ColorLight ("Color Light", Color) = (0.69,0.69,0.69,1)	
		_ColorMain ("Color Main", Color) = (0.55,0.55,0.55,1)		
		_ColorDark ("Color Dark", Color) = (0.329,0.329,0.329,1)
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }

        Cull Off
        Lighting Off
        ZWrite Off
        Blend One OneMinusSrcAlpha

        Pass
        {
        CGPROGRAM
            #pragma vertex SpriteVert
            #pragma fragment SpriteFragPalette
            #pragma target 2.0
            #pragma multi_compile_instancing
            #pragma multi_compile_local _ PIXELSNAP_ON
            #pragma multi_compile _ ETC1_EXTERNAL_ALPHA
            #include "UnitySprites.cginc"
			
			fixed4 _ColorDark;
			fixed4 _ColorMain;
			fixed4 _ColorLight;
			
			fixed4 SpriteFragPalette(v2f IN) : SV_Target
			{
				fixed4 c = SampleSpriteTexture (IN.texcoord) * IN.color;
				
				//Dark
				half3 delta = abs(c.rgb - half3(0.329,0.329,0.329));
				c.rgb = (delta.r + delta.g + delta.b) < 0.01 ? _ColorDark.rgb : c.rgb;

				//Main
				delta = abs(c.rgb - half3(0.55,0.55,0.55));
				c.rgb = (delta.r + delta.g + delta.b) < 0.01 ? _ColorMain.rgb : c.rgb;
								
				//Light
				delta = abs(c.rgb - half3(0.69,0.69,0.69));
				c.rgb = (delta.r + delta.g + delta.b) < 0.01 ? _ColorLight.rgb : c.rgb;
				
				c.rgb *= c.a;
				return c;
			}
			
        ENDCG
        }
    }
}
