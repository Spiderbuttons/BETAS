using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

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
    public string? Condition;

    [ContentSerializer(Optional = true)]
    public ResultOp? ChangeResult = null;
    
    [ContentSerializer(Optional = true)]
    public string? Action;

    [ContentSerializer(Optional = true)]
    public List<string>? Actions;
}