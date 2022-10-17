Shader "Custom/Refracting"
{
    Properties
    {
        _Color("Color", Color) = (1,1,1,1)
        _DistortionTex("Disortion Texture(RG)", 2D) = "grey" {}
        _DistortionPower("Distortion Power", Range(0, 0.1)) = 1
        //_width("Width", Range(0, 0.5)) = 0.25
    }

        SubShader
        {
            Tags {"Queue" = "Transparent" "RenderType" = "Transparent" }

            GrabPass { "_GrabPassTexture" }

            Pass {

                CGPROGRAM
               #pragma vertex vert
               #pragma fragment frag

               #include "UnityCG.cginc"
                //직사각형 펄스 함수
                #define pulse(a, b, x) (step((a),(x)) - step((b),(x)))

                 struct appdata {
                     half4 vertex                : POSITION;
                     half4 texcoord              : TEXCOORD0;
                 };

                 struct v2f {
                     half4 vertex                : SV_POSITION;
                     half2 uv                    : TEXCOORD0;
                     half4 grabPos               : TEXCOORD1;
                 };

                 sampler2D _DistortionTex;
                 half4 _DistortionTex_ST;
                 sampler2D _GrabPassTexture;
                 half _DistortionPower;
                 fixed4 _Color;
                 float _width;

                 v2f vert(appdata v)
                 {
                     v2f o = (v2f)0;

                     o.vertex = UnityObjectToClipPos(v.vertex);
                     o.uv = TRANSFORM_TEX(v.texcoord, _DistortionTex);
                     o.grabPos = ComputeGrabScreenPos(o.vertex);
                     return o;
                 }

                 fixed4 frag(v2f i) : SV_Target
                 {
                     half2 uv = i.grabPos.xy / i.grabPos.w;

                     //왜곡 텍스처 UV 스크롤
                     float2 uvTmp = i.uv;
                     uvTmp.y += _Time.x * 2;

                     //rg는 0~1의 범위에 있으므로, -0.5~0.5로 어긋나는
                     half2 distortion = tex2D(_DistortionTex, uvTmp).rg - 0.5;

                     //왜곡 강도
                     distortion *= _DistortionPower;

                     //원외(uv 좌표내의 반경 0.5 밖)의 픽셀은 파기
                     float dst = distance(float2(0.5, 0.5), i.uv);
                     if (dst > 0.5)
                         discard;
                     /*
                     */

                     //원 안쪽 반경
                     float insideRad = 0.5 -_width;

                     //후면 왜곡
                     uv += distortion;
                     fixed4 distortColor = tex2D(_GrabPassTexture, uv) * step(dst,insideRad);

                     //원연
                     fixed4 circleOutline = _Color * pulse(insideRad,0.5,dst);

                     return distortColor + circleOutline;
                 }
                     ENDCG
            }
        }
            
	/*
	Properties{
		_Color("Color", Color) = (1,1,1,1)
		_MainTex("Albedo (RGB)", 2D) = "white" {}
		_Glossiness("Smoothness", Range(0,1)) = 0.5
		_Metallic("Metallic", Range(0,1)) = 0.0

			//노이즈의 강도를 조절하기위해서 
			_NoiseValue("NoiseValue", Range(0,1)) = 0.0

	}
		SubShader{
			//Tags { "RenderType"="Opaque" }

		   //설정을 투명 설정으로 바꿔줘야 한다.
			Tags { "RenderType" = "Transparent"  "Queue" = "Transparent" }

			GrabPass{}//카메라 화면을 받아오는 부분

			LOD 200

			CGPROGRAM
			// Physically based Standard lighting model, and enable shadows on all light types
			//#pragma surface surf Standard fullforwardshadows

			//투명재질은 빛의 영향을 받지 않는다.때문에 기본 조명 모델을 사용하지 않고
			//직접 아무기능을 하지않는 조명 함수를 통해 조명을 계산하기로 설정을 바꾼다.
			#pragma surface surf nolight noambient

			// Use shader model 3.0 target, to get nicer looking lighting
			#pragma target 3.0
			sampler2D _MainTex;
		//유니티에서 미리 만들어준 키워드로
		//현제 메인카메라의 화면을 받아온다.
		sampler2D _GrabTexture;
		//struct Input {
		//	float2 uv_MainTex;
		//};
		struct Input
		{
			float2 uv_MainTex;
			float4 screenPos;
		};

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;

		float _NoiseValue;

			void surf(Input IN, inout SurfaceOutput o)
			{
			//노이즈 텍스쳐는 메인텍스쳐에서 받아온다.
			fixed4 noise = tex2D(_MainTex, IN.uv_MainTex);

			//스크린의 UV 좌표를 받아오고
			float3 screenUV = IN.screenPos.rgb / (IN.screenPos.a + 0.001);

			o.Emission = tex2D(_GrabTexture, float2(
				(screenUV.x) + noise.x * _NoiseValue,
				(screenUV.y) + noise.y * _NoiseValue));
		}
		//사용자 정의 라이팅 모델 함수
		//서피스 셰이더에서 조명을 제어하기위한 함수 모델명이 지정되어있고. 뒤에  위에서 선언한 nolight를 붙인다.
		// Lighting+"사용자 정의 이름"

		float4 Lightingnolight(SurfaceOutput s, float3 lightDir, float atten)
		{
			return float4(0, 0, 0, 1);
		}
		ENDCG
		}
	*/
			FallBack "Regacy shaders/Transparent/Diffuse"
}
