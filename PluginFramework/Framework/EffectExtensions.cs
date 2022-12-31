using MikuMikuPlugin;

namespace Linearstar.MikuMikuMoving.Framework;

public static class EffectExtensions
{
    public static bool? GetSelected(this Effect effect)
    {
        dynamic obj = ExposedObject.Create(effect)!;
        if (obj.Controller == null) return null;
        
        return (bool)obj.Controller.ObjectList[obj.ID].CurrentSequence.GetBoneSelect(0, 0);
    }
    
    public static void SetSelected(this Effect effect, bool value)
    {
        if (effect.GetSelected() == value) return;

        dynamic obj = ExposedObject.Create(effect)!;
        if (obj.Controller == null) return;
        
        obj.Controller.ObjectList[obj.ID].CurrentSequence.SetBoneSelect(0, 0, value);
    }
}