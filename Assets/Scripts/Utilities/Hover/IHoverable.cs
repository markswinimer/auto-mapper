public interface IHoverable
{
    void OnHoverEnter();
    void OnHoverExit();
    bool IsHoverable(); // This flag can be used to determine if this object should be checked for hovering
}