using MikuMikuPlugin;

namespace Linearstar.MikuMikuMoving.Framework;

public static class SceneExtensions
{
    public static string Localize(this Scene scene, string ja, string en) =>
        scene.Language == "ja" ? ja : en;
}