public static class NoticePopupFactory
{
    public static UI_NoticePopup CreatePopup(WorldType worldType)
    {
        switch (worldType)
        {
            case WorldType.Pelmanus:
                return Managers.UI.ShowPopupUI<UI_PelmanusNoticePopup>();
            case WorldType.Gang:
                return Managers.UI.ShowPopupUI<UI_GangrilNoticePopup>();
            default:
                return Managers.UI.ShowPopupUI<UI_NoticePopup>();
        }
    }
}