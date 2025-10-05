using UnityEngine;

public static class CameraHelpers
{
    public static bool IsObjectVisible(this Camera camera, Transform obj)
    {
        var viewPos = camera.WorldToViewportPoint(obj.position);
        
        // Slightly more than 0 and 1 to account for game object size
        const float min = -0.1f; 
        const float max = 1.1f;
        
        return viewPos.x is >= min and <= max && viewPos.y is >= min and <= max && viewPos.z > min;

        // Checking renderer instead
        // return GeometryUtility.TestPlanesAABB(GeometryUtility.CalculateFrustumPlanes(camera), renderer.bounds);
    }
}
