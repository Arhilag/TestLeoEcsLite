using System;
using Voody.UniLeo.Lite;

public class ModelProvider : MonoProvider<ModelComponent>
{
    private void Awake()
    {
        value.modelTransform = transform;
    }
}