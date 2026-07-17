// -----------------------------------------------------------------------------
// Shared CommonModApi utility: Tuple
// Namespace/file: ruina-roguelike-reborn-main\CommonModApi\Tuple.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
namespace CommonModApi

{ 
/// <summary>Tuple</summary>
public static class Tuple
{
  public static Tuple<T1, T2> Create<T1, T2>(T1 item1, T2 second)
  {
    return new Tuple<T1, T2>(item1, second);
  }
}
}
