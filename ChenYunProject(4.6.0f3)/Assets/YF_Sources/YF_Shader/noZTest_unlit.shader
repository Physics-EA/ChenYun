Shader "Custom/noZTest_unlit" {	
    Properties {
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _Color ("Main Color", Color) = (1,1,1,1)
        _RimColor ("Rim Color", Color) = (1, 1, 1, 1)
        _RimWidth ("Rim Width", Float) = 0.7
    }
    SubShader {
       
       Tags {
       		"Queue"="Transparent" 
       		"IgnoreProjector"="True" 
       		"RenderType"="Transparent" 
       		}
        	
		 	  lighting off
     		  cull back
     		  zwrite off
     		  ztest off
     		 
     		  
     		  Pass{
		 	  
		 	  
		 	  blend Srcalpha OneMinusSrcAlpha
			 
			      

            CGPROGRAM
			
                #pragma vertex vert
                #pragma fragment frag
                #include "UnityCG.cginc"
				
				
				
				
                struct appdata {
                    float4 vertex : POSITION;
                    float3 normal : NORMAL;
                    fixed4 color : COLOR;
                    float2 texcoord : TEXCOORD0;
                };

                struct v2f {
                    float4 pos : SV_POSITION;
                    float2 uv : TEXCOORD0;
                    fixed4 color : COLOR0;
                };

                uniform float4 _MainTex_ST;
                uniform fixed4 _RimColor;
                float _RimWidth;

                v2f vert (appdata v) {
                    v2f o;
                    o.pos = mul (UNITY_MATRIX_MVP, v.vertex);

                    float3 viewDir = normalize(ObjSpaceViewDir(v.vertex));
                    float dotProduct = 1 - dot(v.normal, viewDir);
                   
                    o.color = smoothstep(1 - _RimWidth, 1.0, dotProduct);
                    o.color *= _RimColor;
                 //   o.color.a = v.color.a;

                    o.uv = v.texcoord.xy;
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