using Microsoft.Xna.Framework.Content;

namespace BETAS.Models;

public class TargetMethod
{
    [ContentSerializer(Optional = false)]
    public string Type;

    [ContentSerializer(Optional = true)]
    public string Method;

    [ContentSerializer(Optional = true)]
    public string[] Parameters = [];

    [ContentSerializer(Optional = true)]
    public bool IsGetter = false;
    
    [ContentSerializer(Optional = true)]
    public bool IsSetter = false;

    [ContentSerializer(Optional = true)]
    public string Assembly = "Stardew Valley";
}