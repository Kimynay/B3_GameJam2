// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Foliage"
{
	Properties
	{
		_Foliage_Albedo("Foliage_Albedo", 2D) = "white" {}
		_Cutoff( "Mask Clip Value", Float ) = 0.9
		_Foliage_Opacity("Foliage_Opacity", 2D) = "white" {}
		_Foliage_Normal("Foliage_Normal", 2D) = "bump" {}
		_Foliage_Roughness("Foliage_Roughness", 2D) = "white" {}
		_Foliage_AO("Foliage_AO", 2D) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "TransparentCutout"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" }
		Cull Off
		Blend SrcAlpha OneMinusSrcAlpha
		
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 4.6
		#pragma surface surf Standard keepalpha noshadow vertex:vertexDataFunc 
		struct Input
		{
			float3 worldPos;
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
		uniform float _Cutoff = 0.9;


		float3 mod2D289( float3 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }

		float2 mod2D289( float2 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }

		float3 permute( float3 x ) { return mod2D289( ( ( x * 34.0 ) + 1.0 ) * x ); }

		float snoise( float2 v )
		{
			const float4 C = float4( 0.211324865405187, 0.366025403784439, -0.577350269189626, 0.024390243902439 );
			float2 i = floor( v + dot( v, C.yy ) );
			float2 x0 = v - i + dot( i, C.xx );
			float2 i1;
			i1 = ( x0.x > x0.y ) ? float2( 1.0, 0.0 ) : float2( 0.0, 1.0 );
			float4 x12 = x0.xyxy + C.xxzz;
			x12.xy -= i1;
			i = mod2D289( i );
			float3 p = permute( permute( i.y + float3( 0.0, i1.y, 1.0 ) ) + i.x + float3( 0.0, i1.x, 1.0 ) );
			float3 m = max( 0.5 - float3( dot( x0, x0 ), dot( x12.xy, x12.xy ), dot( x12.zw, x12.zw ) ), 0.0 );
			m = m * m;
			m = m * m;
			float3 x = 2.0 * frac( p * C.www ) - 1.0;
			float3 h = abs( x ) - 0.5;
			float3 ox = floor( x + 0.5 );
			float3 a0 = x - ox;
			m *= 1.79284291400159 - 0.85373472095314 * ( a0 * a0 + h * h );
			float3 g;
			g.x = a0.x * x0.x + h.x * x0.y;
			g.yz = a0.yz * x12.xz + h.yz * x12.yw;
			return 130.0 * dot( m, g );
		}


		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float3 ase_worldPos = mul( unity_ObjectToWorld, v.vertex );
			float2 appendResult10 = (float2(ase_worldPos.x , ase_worldPos.z));
			float simplePerlin2D15 = snoise( ( ( appendResult10 * float2( 0.1,0.1 ) ) + ( _Time.y / 10.0 ) ) );
			simplePerlin2D15 = simplePerlin2D15*0.5 + 0.5;
			float3 temp_cast_0 = (( ( ( v.texcoord.xy.y * ( simplePerlin2D15 * 0.3 ) ) - 0.05 ) * 100.0 )).xxx;
			v.vertex.xyz += temp_cast_0;
			v.vertex.w = 1;
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_Foliage_Normal = i.uv_texcoord * _Foliage_Normal_ST.xy + _Foliage_Normal_ST.zw;
			o.Normal = UnpackNormal( tex2D( _Foliage_Normal, uv_Foliage_Normal ) );
			float2 uv_Foliage_Albedo = i.uv_texcoord * _Foliage_Albedo_ST.xy + _Foliage_Albedo_ST.zw;
			float4 tex2DNode1 = tex2D( _Foliage_Albedo, uv_Foliage_Albedo );
			float3 appendResult8 = (float3(tex2DNode1.r , tex2DNode1.g , tex2DNode1.b));
			o.Albedo = appendResult8;
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
28;504;1330;793;2643.34;-294.4667;1;True;False
Node;AmplifyShaderEditor.WorldPosInputsNode;9;-2829.805,432.2925;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleTimeNode;11;-2504.807,585.739;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;10;-2338.612,463.7344;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;13;-2208.729,609.7223;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;10;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;12;-2151.325,447.2296;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0.1,0.1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;14;-1941.412,511.4346;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;15;-1786.064,424.3682;Inherit;True;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;16;-1818.231,180.5486;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;17;-1490.28,412.6061;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0.3;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;18;-1270.741,284.5578;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;1;-533.677,-76.49608;Inherit;True;Property;_Foliage_Albedo;Foliage_Albedo;0;0;Create;True;0;0;False;0;False;-1;31bbfad8722befb4d8c5c9ba37d6b522;31bbfad8722befb4d8c5c9ba37d6b522;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;5;-547.6654,534.676;Inherit;True;Property;_Foliage_Roughness;Foliage_Roughness;4;0;Create;True;0;0;False;0;False;-1;6bd7966bb1898114c8379cd135d5afb6;6bd7966bb1898114c8379cd135d5afb6;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleSubtractOpNode;20;-1059.425,305.7127;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0.05;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;6;-241.6654,504.676;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;19;-860.0143,293.0299;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;100;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;4;-530.6654,125.676;Inherit;True;Property;_Foliage_Normal;Foliage_Normal;3;0;Create;True;0;0;False;0;False;-1;060c3924bc5e95448b5b156708223d6e;060c3924bc5e95448b5b156708223d6e;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;7;-539.6654,732.676;Inherit;True;Property;_Foliage_AO;Foliage_AO;5;0;Create;True;0;0;False;0;False;-1;f79027a60ec23014dadb0bff15eb9168;f79027a60ec23014dadb0bff15eb9168;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;2;-534.373,323.1761;Inherit;True;Property;_Foliage_Opacity;Foliage_Opacity;2;0;Create;True;0;0;False;0;False;-1;2622ebb8c0d608149ac589d15d02f3bb;2622ebb8c0d608149ac589d15d02f3bb;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;8;-188.7151,-43.64455;Inherit;False;FLOAT3;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;0,0;Float;False;True;-1;6;ASEMaterialInspector;0;0;Standard;Foliage;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Off;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.9;True;False;0;True;TransparentCutout;;Transparent;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;10;0;9;1
WireConnection;10;1;9;3
WireConnection;13;0;11;0
WireConnection;12;0;10;0
WireConnection;14;0;12;0
WireConnection;14;1;13;0
WireConnection;15;0;14;0
WireConnection;17;0;15;0
WireConnection;18;0;16;2
WireConnection;18;1;17;0
WireConnection;20;0;18;0
WireConnection;6;0;5;2
WireConnection;19;0;20;0
WireConnection;8;0;1;1
WireConnection;8;1;1;2
WireConnection;8;2;1;3
WireConnection;0;0;8;0
WireConnection;0;1;4;0
WireConnection;0;4;6;0
WireConnection;0;5;7;2
WireConnection;0;9;2;2
WireConnection;0;10;2;2
WireConnection;0;11;19;0
ASEEND*/
//CHKSM=53C0CEBF1F3E349C177EF63C44D6794D148EC8E0