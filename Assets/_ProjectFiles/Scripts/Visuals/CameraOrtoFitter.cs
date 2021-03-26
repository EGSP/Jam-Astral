
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraOrtoFitter : MonoBehaviour
{
    public enum FitMode
    {
        Width,
        Height
    }

    [SerializeField] private float units;
    [SerializeField] private FitMode fitMode;

    // Start is called before the first frame update
    private void Awake()
    {
        ChangeScreenSize();
    }

#if UNITY_EDITOR
    private Resolution _oldResolution;
    private void Update()
    {
        if (_oldResolution.width == Screen.width && _oldResolution.height == Screen.height) return;
      
        ChangeScreenSize();
  
        _oldResolution.width = Screen.width;
        _oldResolution.height = Screen.height;
    }
#endif

    private void OnDrawGizmos()
    {
        if (fitMode == FitMode.Width)
        {
            Gizmos.color = Color.yellow;
            var widthHalf = units / 2;
            Gizmos.DrawLine(transform.position, transform.position + Vector3.right * widthHalf);
            Gizmos.DrawLine(transform.position, transform.position - Vector3.right * widthHalf);
        }
        else
        {
            Gizmos.color = Color.yellow;
            var heightHalf = units / 2;
            Gizmos.DrawLine(transform.position, transform.position + Vector3.up * heightHalf);
            Gizmos.DrawLine(transform.position, transform.position - Vector3.up * heightHalf);
        }
    }

#if ODIN_INSPECTOR
    [Button("Change screen")]
#endif
    private void ChangeScreenSize()
    {
        var _camera = GetComponent<Camera>();
        if (_camera == null)
            return;
        
        switch (fitMode)
        {
            case FitMode.Width:
                _camera.orthographicSize = ScreenFitter.GetSizeByWidth(units, Camera.main);
                break;
            case FitMode.Height:
                _camera.orthographicSize = ScreenFitter.GetSizeByHeight(units);
                break;
        }
    }
}

public static class ScreenFitter
{
    // WIDTH
    // Width = height * aspectRatio
    // Portrait
    public static float GetSizeByWidth(float width, Camera camera)
    {    
        Vector2 screen = new Vector2((float)camera.pixelWidth,(float)camera.pixelHeight);

        return GetSizeByWidth(width, screen);
    }

    // Portrait
    public static float GetSizeByWidth(float width, Resolution resolution)
    {
        var aspectRatio = 0f;

        if (resolution.width != 0)
            aspectRatio = (float)resolution.height / (float)resolution.width;

        return GetSizeByWidth(width, aspectRatio);
    }

    // Portrait
    public static float GetSizeByWidth(float width, Vector2 screen)
    {
        var aspectRatio = 0f;

        if (screen.x!= 0)
            aspectRatio = screen.y / screen.x;

        return GetSizeByWidth(width, aspectRatio);
    }

    // Portrait
    public static float GetSizeByWidth(float width, float aspectRatio)
    {
        return width * aspectRatio * 0.5f;
    }

    
    // HEIGHT
    // Height = 2f * orhtoSize
    public static float GetSizeByHeight(float height)
    {
        return height/2;
    }
}
