// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Foliage"
{
	Properties
	{
		_Foliage_Albedo("Foliage_Albedo", 2D) = "white" {}
		_Cutoff( "Mask Clip Value", Float ) = 0.5
		_Foliage_Opacity("Foliage_Opacity", 2D) = "white" {}
		_Foliage_Normal("Foliage_Normal", 2D) = "bump" {}
		_Foliage_Roughness("Foliage_Roughness", 2D) = "white" {}
		_Foliage_AO("Foliage_AO", 2D) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "TransparentCutout"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Off
		Blend SrcAlpha OneMinusSrcAlpha
		
		CGPROGRAM
		#pragma target 3.0
		#pragma surface surf Standard keepalpha noshadow 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform sampler2D _Foliage_Normal;
		uniform float4 _Foliage_Normal_ST;
		uniform sampler2D _Foliage_Albedo;
		SamplerState sampler_Foliage_Albedo;
		uniform float4 _Foliage_Albedo_ST;
		uniform sampler2D _Foliage_Roughness;
		SamplerState sampler_Foliage_Roughness;
		uniform float4 _Foliage_Roughness_ST;
		uniform sampler2D _Foliage_AO;
		SamplerState sampler_Foliage_AO;
		uniform float4 _Foliage_AO_ST;
		uniform sampler2D _Foliage_Opacity;
		SamplerState sampler_Foliage_Opacity;
		uniform float4 _Foliage_Opacity_ST;
		uniform float _Cutoff = 0.5;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_Foliage_Normal = i.uv_texcoord * _Foliage_Normal_ST.xy + _Foliage_Normal_ST.zw;
			o.Normal = UnpackNormal( tex2D( _Foliage_Normal, uv_Foliage_Normal ) );
			float2 uv_Foliage_Albedo = i.uv_texcoord * _Foliage_Albedo_ST.xy + _Foliage_Albedo_ST.zw;
			float4 tex2DNode1 = tex2D( _Foliage_Albedo, uv_Foliage_Albedo );
			float3 appendResult8 = (float3(tex2DNode1.r , tex2DNode1.g , tex2DNode1.b));
			o.Albedo = appendResult8;
			o.Emission = ( appendResult8 * float3( 0.2,0.2,0.2 ) );
			float2 uv_Foliage_Roughness = i.uv_texcoord * _Foliage_Roughness_ST.xy + _Foliage_Roughness_ST.zw;
			o.Smoothness = ( 1.0 - tex2D( _Foliage_Roughness, uv_Foliage_Roughness ).g );
			float2 uv_Foliage_AO = i.uv_texcoord * _Foliage_AO_ST.xy + _Foliage_AO_ST.zw;
			o.Occlusion = tex2D( _Foliage_AO, uv_Foliage_AO ).g;
			float2 uv_Foliage_Opacity = i.uv_texcoord * _Foliage_Opacity_ST.xy + _Foliage_Opacity_ST.zw;
			float4 tex2DNode2 = tex2D( _Foliage_Opacity, uv_Foliage_Opacity );
			o.Alpha = tex2DNode2.g;
			clip( tex2DNode2.g - _Cutoff );
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18503
363;507;1350;901;678.9839;467.5225;1;True;True
Node;AmplifyShaderEditor.SamplerNode;1;-533.677,-76.49608;Inherit;True;Property;_Foliage_Albedo;Foliage_Albedo;0;0;Create;True;0;0;False;0;False;-1;31bbfad8722befb4d8c5c9ba37d6b522;31bbfad8722befb4d8c5c9ba37d6b522;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;5;-547.6654,534.676;Inherit;True;Property;_Foliage_Roughness;Foliage_Roughness;4;0;Create;True;0;0;False;0;False;-1;6bd7966bb1898114c8379cd135d5afb6;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;4;-530.6654,125.676;Inherit;True;Property;_Foliage_Normal;Foliage_Normal;3;0;Create;True;0;0;False;0;False;-1;060c3924bc5e95448b5b156708223d6e;None;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;8;-230.7151,-86.64455;Inherit;False;FLOAT3;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.OneMinusNode;6;-241.6654,504.676;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;7;-539.6654,732.676;Inherit;True;Property;_Foliage_AO;Foliage_AO;5;0;Create;True;0;0;False;0;False;-1;f79027a60ec23014dadb0bff15eb9168;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;2;-534.373,323.1761;Inherit;True;Property;_Foliage_Opacity;Foliage_Opacity;2;0;Create;True;0;0;False;0;False;-1;2622ebb8c0d608149ac589d15d02f3bb;2622ebb8c0d608149ac589d15d02f3bb;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.WireNode;10;-168.9839,108.4775;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;9;-28.98389,-196.5225;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0.2,0.2,0.2;False;1;FLOAT3;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;0,0;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;Foliage;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Off;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;False;0;True;TransparentCutout;;Transparent;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;8;0;1;1
WireConnection;8;1;1;2
WireConnection;8;2;1;3
WireConnection;6;0;5;2
WireConnection;10;0;4;0
WireConnection;9;0;8;0
WireConnection;0;0;8;0
WireConnection;0;1;10;0
WireConnection;0;2;9;0
WireConnection;0;4;6;0
WireConnection;0;5;7;2
WireConnection;0;9;2;2
WireConnection;0;10;2;2
ASEEND*/
//CHKSM=3C69D7A2E4FD233BA35D993312C221109C313DF1