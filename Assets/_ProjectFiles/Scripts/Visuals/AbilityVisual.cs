using System;
using System.Collections;
using System.Collections.Generic;
using Egsp.Core.Ui;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasRenderer))]
public class AbilityVisual : Visual
{
    private Image _image;
    private int _opacityId = Shader.PropertyToID("_Opacity");
    
    private void Awake()
    {
        _image = GetComponent<Image>();
    }

    [Sirenix.OdinInspector.Button("Set opacity")]
    public void SetOpacity(float opacity)
    {
        if (_image == null)
            _image = GetComponent<Image>();
        
        var mat = _image.material;
        mat.SetFloat(_opacityId, opacity);
    }
}
