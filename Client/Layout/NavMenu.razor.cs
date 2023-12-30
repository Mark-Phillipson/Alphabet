using Microsoft.AspNetCore.Components;

namespace Client.Layout {
   public partial class NavMenu {
      [Inject] public required NavigationManager NavigationManager { get; set; }

      private bool collapseNavMenu = true;

      private string? NavMenuCssClass => collapseNavMenu ? "collapse" : null;
      private void ToggleNavMenu() {
         collapseNavMenu = !collapseNavMenu;
      }
   }
}