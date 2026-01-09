namespace PixelCrew.UI.Contollers
{
    public class CallMenuController : BaseCallController<MainMenuWindow>
    {
        protected override string ResourcePath => "UI/MainMenuWindow";
        protected override bool CallInMainMenu => true;
    }
}