using System.Collections.Generic;
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
    public string Condition;

    [ContentSerializer(Optional = true)]
    public ResultOp ChangeResult = null;
    
    [ContentSerializer(Optional = true)]
    public string Action;

    [ContentSerializer(Optional = true)]
    public List<string> Actions;
}