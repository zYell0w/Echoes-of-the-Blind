Shader "Custom/OutlineWave"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _BaseColor ("Base Color", Color) = (0.1, 0.1, 0.1, 1)
        _OutlineColor ("Outline Color", Color) = (1, 1, 1, 1)
        _OutlineWidth ("Outline Width", Range(0, 0.1)) = 0.02
        _WaveSpeed ("Wave Speed", Float) = 5.0
        _WaveFalloff ("Wave Falloff", Float) = 2.0
        _AfterglowIntensity ("Afterglow Intensity", Range(0, 2)) = 0.5
        _AfterglowDuration ("Afterglow Duration", Float) = 1.0
        _AfterglowColor ("Afterglow Color", Color) = (0.8, 0.9, 1.0, 1)
        _AfterglowWidth ("Afterglow Width", Range(0, 0.2)) = 0.05
    }
    
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float3 worldPos : TEXCOORD1;
                float3 worldNormal : TEXCOORD2;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _BaseColor;
            float4 _OutlineColor;
            float _OutlineWidth;
            float _WaveSpeed;
            float _WaveFalloff;
            float _AfterglowIntensity;
            float _AfterglowDuration;
            float4 _AfterglowColor;
            float _AfterglowWidth;

            // Wave data (set from WaveController script)
            float4 _WaveOrigin;
            float _WaveTime;
            float _WaveIntensity;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                
                // Convert to world space for wave calculations
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                o.worldNormal = normalize(mul(unity_ObjectToWorld, float4(v.normal, 0.0)).xyz);
                
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Calculate distance from wave origin
                float dist = distance(i.worldPos, _WaveOrigin.xyz);
                
                // Calculate wave front position
                float waveFront = _WaveTime * _WaveSpeed;
                float waveDistance = abs(dist - waveFront);
                
                // Calculate outline strength based on wave
                float outlineStrength = 0.0;
                float afterglowStrength = 0.0;
                
                // Main outline effect - only show if wave intensity is > 0
                if (waveDistance < _OutlineWidth && _WaveIntensity > 0)
                {
                    // Create a pulse effect
                    outlineStrength = (1.0 - waveDistance / _OutlineWidth) * _WaveIntensity;
                    
                    // Add falloff based on distance from origin
                    float distanceFalloff = 1.0 / (1.0 + dist * _WaveFalloff);
                    outlineStrength *= distanceFalloff;
                    
                    // Enhance outline based on surface normal facing camera
                    float3 viewDir = normalize(_WorldSpaceCameraPos - i.worldPos);
                    float fresnel = 1.0 - abs(dot(i.worldNormal, viewDir));
                    outlineStrength *= fresnel;
                }
                
                // Afterglow effect - appears BEHIND the outline (further from wave origin)
                // Calculate position for afterglow (slightly behind the main outline)
                float afterglowFront = waveFront - _OutlineWidth;
                float afterglowDistance = dist - afterglowFront;
                
                // Afterglow appears in the region between the back of the outline and further back
                // Only show afterglow if wave has passed this point and intensity is > 0
                if (afterglowDistance > 0 && afterglowDistance < _AfterglowWidth && _WaveIntensity > 0)
                {
                    // Calculate how long ago this point was passed by the wave
                    float timeSinceWavePassed = (dist - waveFront) / _WaveSpeed;
                    
                    if (timeSinceWavePassed > 0 && timeSinceWavePassed < _AfterglowDuration)
                    {
                        // Afterglow strength based on distance from outline and time
                        float distanceFactor = 1.0 - (afterglowDistance / _AfterglowWidth);
                        float timeFactor = 1.0 - (timeSinceWavePassed / _AfterglowDuration);
                        
                        afterglowStrength = distanceFactor * timeFactor * _AfterglowIntensity * _WaveIntensity;
                        
                        // Add distance falloff to afterglow
                        float distanceFalloff = 1.0 / (1.0 + dist * _WaveFalloff);
                        afterglowStrength *= distanceFalloff;
                        
                        // Strong fresnel effect to make afterglow hug the mesh
                        float3 viewDir = normalize(_WorldSpaceCameraPos - i.worldPos);
                        float fresnel = 1.0 - abs(dot(i.worldNormal, viewDir));
                        afterglowStrength *= fresnel * fresnel; // Squared for stronger effect on edges
                    }
                }

                // Sample texture and apply base color (dark)
                fixed4 col = tex2D(_MainTex, i.uv) * _BaseColor;
                
                // Apply afterglow first (behind the outline)
                col.rgb = lerp(col.rgb, _AfterglowColor.rgb, afterglowStrength);
                
                // Then apply outline on top (in front of afterglow)
                col.rgb = lerp(col.rgb, _OutlineColor.rgb, outlineStrength);
                
                return col;
            }
            ENDCG
        }
    }
}