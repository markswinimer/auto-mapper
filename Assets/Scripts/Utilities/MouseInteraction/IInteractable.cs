public interface IInteractable
{
    void OnHoverEnter();
    void OnHoverExit();
    bool IsHoverable();

    void OnClick();
    bool IsClickable();
}
