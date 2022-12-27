using System.Collections.Generic;
using System.Linq;
using Linearstar.MikuMikuMoving.AnimateCaptionPlugin.Animation;
using MikuMikuPlugin;

namespace Linearstar.MikuMikuMoving.AnimateCaptionPlugin;

public record SceneState(
    float Frame,
    Caption? SelectedCaption,
    ICaption? SelectedICaption,
    IReadOnlyList<CaptionProperties> Captions
)
{
    public static SceneState FromScene(Scene scene, float currentFrame)
    {
        var caption = scene.SelectedCaptions.FirstOrDefault(x => x.GetIndex() != -1);
        
        return new(
            currentFrame,
            caption?.Clone(),
            caption,
            scene.Captions.Select(CaptionProperties.FromCaption).ToArray()
        );
    }
}