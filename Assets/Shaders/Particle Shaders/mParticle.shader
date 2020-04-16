// Compiled shader for all platforms, uncompressed size: 0.6KB

Shader "Mobile/Particles/mParticle" {
Properties {
 _MainTex ("Particle Texture", 2D) = "white" {}
}
SubShader { 
 Tags { "QUEUE"="Transparent" "IGNOREPROJECTOR"="true" "RenderType"="Transparent" }
 Pass {
  Tags { "QUEUE"="Transparent" "IGNOREPROJECTOR"="true" "RenderType"="Transparent" }
  Lighting Off
  ZWrite Off
  Cull Off
  Fog {
   Color (1,1,1,1)
  }
  Blend SrcAlpha OneMinusSrcAlpha
  ColorMaterial AmbientAndDiffuse
  SetTexture [_MainTex] { combine texture * primary }
 }
}
}