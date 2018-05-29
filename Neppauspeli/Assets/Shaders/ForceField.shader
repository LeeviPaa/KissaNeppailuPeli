// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "ForceField"
{
	Properties
	{
		_Power("Power", Float) = 2
		[HDR]_GlowColor("GlowColor", Color) = (1,1,1,1)
		[HDR]_BaseColor("BaseColor", Color) = (1,1,1,1)
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Back
		Blend SrcAlpha OneMinusSrcAlpha
		AlphaToMask On
		CGPROGRAM
		#pragma target 3.0
		#pragma surface surf Standard keepalpha noshadow 
		struct Input
		{
			float3 worldPos;
			float3 worldNormal;
		};

		uniform float4 _BaseColor;
		uniform float _Power;
		uniform float4 _GlowColor;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float3 ase_worldPos = i.worldPos;
			float3 ase_worldViewDir = normalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			float3 ase_worldNormal = i.worldNormal;
			float fresnelNDotV1 = dot( normalize( ase_worldNormal ), ase_worldViewDir );
			float fresnelNode1 = ( 0.0 + 1.0 * pow( 1.0 - fresnelNDotV1, _Power ) );
			o.Albedo = ( _BaseColor * fresnelNode1 ).rgb;
			o.Emission = ( fresnelNode1 * _GlowColor ).rgb;
			o.Alpha = ( _BaseColor.a * fresnelNode1 );
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=15301
0;92;2023;1286;549.9439;457.9496;1;True;True
Node;AmplifyShaderEditor.RangedFloatNode;3;-82,278;Float;False;Property;_Power;Power;1;0;Create;True;0;0;False;0;2;1.02;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;8;151,419;Float;False;Property;_GlowColor;GlowColor;2;1;[HDR];Create;True;0;0;False;0;1,1,1,1;0,0.6887212,1,1;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.FresnelNode;1;142,148;Float;False;Tangent;4;0;FLOAT3;0,0,1;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;9;135.3,-132.7;Float;False;Property;_BaseColor;BaseColor;3;1;[HDR];Create;True;0;0;False;0;1,1,1,1;0.5377358,0.5377358,0.5377358,1;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;11;435.0561,-58.94958;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;7;431,311;Float;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;10;433.0561,68.05042;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;708,10;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;ForceField;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;0;False;0;Custom;0.5;True;False;0;True;Transparent;;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;-1;False;-1;-1;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;0;0;True;0;0;0;False;-1;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;1;3;3;0
WireConnection;11;0;9;0
WireConnection;11;1;1;0
WireConnection;7;0;1;0
WireConnection;7;1;8;0
WireConnection;10;0;9;4
WireConnection;10;1;1;0
WireConnection;0;0;11;0
WireConnection;0;2;7;0
WireConnection;0;9;10;0
ASEEND*/
//CHKSM=DA4D204B2FDE72355F99A92FD16B3E4E485B53C1