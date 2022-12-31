using MikuMikuPlugin;

namespace Linearstar.MikuMikuMoving.Framework;

public static class MotionLayerExtensions
{
    public static bool? GetSelected(this MotionLayer motionLayer)
    {
        dynamic obj = ExposedObject.Create(motionLayer)!;
        if (obj.Controller == null) return null;
        
        return (bool)obj.Controller.ObjectList[obj.ModelID].CurrentSequence.GetBoneSelect(obj.BoneID, motionLayer.LayerID);
    }
    
    public static void SetSelected(this MotionLayer motionLayer, bool value)
    {
        if (motionLayer.GetSelected() == value) return;

        dynamic obj = ExposedObject.Create(motionLayer)!;
        if (obj.Controller == null) return;
        
        obj.Controller.ObjectList[obj.ModelID].CurrentSequence.SetBoneSelect(obj.BoneID, motionLayer.LayerID, value);
    }
}