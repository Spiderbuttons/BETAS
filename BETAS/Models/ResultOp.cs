using Microsoft.Xna.Framework.Content;
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace BETAS.Models;

public class ResultOp
{
    [ContentSerializer(Optional = false)]
    public string Operation;

    [ContentSerializer(Optional = false)]
    public string Value;
}