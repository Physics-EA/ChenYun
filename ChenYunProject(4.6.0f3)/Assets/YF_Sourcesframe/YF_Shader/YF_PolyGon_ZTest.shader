

Shader "Custom/YF_PolyGon_ZTest" {
    Properties {
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _Color ("Main Color", Color) = (1,1,1,1)
        _EdgeColor ("Rim Color", Color) = (1, 1, 1, 1)
        //_RimWidth ("Rim Width", Float) = 0.7
    }
    SubShader {
       
       Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
        	
		 	  lighting off
       		  zwrite on
     		  cull off
     		  
     		  
     		  Pass{
     		  ColorMask 0
     		  }
     		  
     		  Pass{
		 	  
		 	  
		 	  blend one one  
            CGPROGRAM
          
                #pragma vertex vert
                #pragma fragment frag
                #include "UnityCG.cginc"
				
				
				
				
                struct appdata {
                    float4 vertex : POSITION;
                   // float3 normal : NORMAL;
                    fixed4 color : COLOR;
                    float2 texcoord : TEXCOORD0;
                };

                struct v2f {
                    float4 pos : SV_POSITION;
                    float2 uv : TEXCOORD0;
                    fixed4 color : COLOR0;
                };

                uniform float4 _MainTex_ST;
                uniform fixed4 _EdgeColor;
               // float _RimWidth;

                v2f vert (appdata v) {
                    v2f o;
                    o.pos = mul (UNITY_MATRIX_MVP, v.vertex);

                    //float3 viewDir = normalize(ObjSpaceViewDir(v.vertex));
                   // float dotProduct = 1 - dot(v.normal, viewDir);
                   
                   o.color =  v.color;
                    //o.color = smoothstep(1 - _RimWidth, 1.0, dotProduct) + v.color;
                    o.color *= _EdgeColor;
                 //   o.color.a = v.color.a;

                    o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
                    return o;
                }

                uniform sampler2D _MainTex;
                uniform fixed4 _Color;

                fixed4 frag(v2f i) : COLOR {
                    fixed4 texcol = tex2D(_MainTex, i.uv);
                    texcol *= _Color;
                    texcol.rgb += i.color;
                   
                    return texcol;
                }
            ENDCG
        }
        
       
    }
    Fallback "Transparent/VertexLit"
}