using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[Serializable]
[PostProcess(typeof(PixelRenderer), PostProcessEvent.AfterStack, "Custom/Pixel")]
public sealed class PixelPostProcess : PostProcessEffectSettings
{
    [Range(64, 1080), Tooltip("Amount of Pixels.")]
    public IntParameter posterize = new IntParameter { value = 160 };
    
    [Range(4, 256), Tooltip("Bit of Color.")]
    public IntParameter colorQuality = new IntParameter { value = 8 };
}
public sealed class PixelRenderer : PostProcessEffectRenderer<PixelPostProcess>
{
    public override void Render(PostProcessRenderContext context)
    {
        var sheet = context.propertySheets.Get(Shader.Find("Hidden/Custom/Pixel"));
        sheet.properties.SetFloat("_Posterize", settings.posterize);
        sheet.properties.SetFloat("_ColorQuality", settings.colorQuality);
        context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
    }
}