using Microsoft.Xna.Framework.Content;

namespace BETAS.Models;

public class ResultOp
{
    [ContentSerializer(Optional = false)]
    public string Operation;

    [ContentSerializer(Optional = false)]
    public string Value;
}