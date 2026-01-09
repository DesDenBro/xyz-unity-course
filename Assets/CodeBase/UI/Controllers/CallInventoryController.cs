using PixelCrew.UI.Inventory;

namespace PixelCrew.UI.Contollers
{
    public class CallInventoryController : BaseCallController<InventoryWindow>
    {
        protected override string ResourcePath => "UI/InventoryWindow";
        protected override bool CallInMainMenu => false;
    }
}