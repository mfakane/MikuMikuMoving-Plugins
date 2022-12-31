using MikuMikuPlugin;

namespace Linearstar.MikuMikuMoving.Framework;

public static class CameraExtensions
{
    public static bool? GetSelected(this CameraLayer cameraLayer)
    {
        dynamic obj = ExposedObject.Create(cameraLayer)!;
        if (obj.Controller == null) return null;
        
        return (bool)obj.Controller.ObjectList[obj.CameraID].CurrentSequence.GetBoneSelect(0, cameraLayer.LayerID);
    }
    
    public static void SetSelected(this CameraLayer cameraLayer, bool value)
    {
        if (cameraLayer.GetSelected() == value) return;

        dynamic obj = ExposedObject.Create(cameraLayer)!;
        if (obj.Controller == null) return;
        
        obj.Controller.ObjectList[obj.CameraID].CurrentSequence.SetBoneSelect(0, cameraLayer.LayerID, value);
    }
}