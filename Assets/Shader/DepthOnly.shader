Shader "DepthOnly"
{
	SubShader
	{
		Pass
        {
            Name "DepthOnly"
            Tags { "LightMode" = "ShadowCaster" }
            ZWrite On
        	ZTest Less
            ColorMask 0
        }
	}
}
