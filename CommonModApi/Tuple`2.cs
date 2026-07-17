// -----------------------------------------------------------------------------
// Shared CommonModApi utility: Tuple`2
// Namespace/file: ruina-roguelike-reborn-main\CommonModApi\Tuple`2.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
namespace CommonModApi {

/// <summary>Tuple</summary>

public sealed class Tuple<T1, T2>
{
  public T1 First;
  public T2 Second;

  public Tuple(T1 first, T2 second)
  {
    this.First = first;
    this.Second = second;
  }

  public override string ToString() => $"[{this.First}, {this.Second}]";

}
}