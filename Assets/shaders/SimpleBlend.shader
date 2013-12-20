Shader "SimpleBlend" {
Properties {
	_MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
}

SubShader {
	Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
	LOD 100
	
	ZWrite Off
	
	Blend One OneMinusSrcAlpha //One SrcAlpha
	//Blend SrcAlpha OneMinusSrcAlpha 
	//Cull Off
	
	Pass {
		Lighting Off
		SetTexture [_MainTex] { combine texture } 
	}
}
}
