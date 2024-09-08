using Microsoft.Xna.Framework.Content;

namespace BETAS.Models;

public class DynamicPatch
{
    [ContentSerializer(Optional = false)]
    public string Id;

    [ContentSerializer(Optional = false)]
    public TargetMethod Target;

    [ContentSerializer(Optional = false)]
    public string PatchType;

    [ContentSerializer(Optional = true)]
    public string Condition = null;

    [ContentSerializer(Optional = true)]
    public ResultOp ChangeResult = null;
    
    [ContentSerializer(Optional = true)]
    public string Action = null;

    [ContentSerializer(Optional = true)]
    public string[] Actions = null;
}