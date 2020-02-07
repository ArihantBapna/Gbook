using System;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Gbook.ClassFiles
{
    public static class NavigationExtensions
    {
        public static async Task PushModalAsyncSingle(this INavigation nav, Page page, bool animated = false)
        {
            if (nav.ModalStack.Count == 0 ||
                nav.ModalStack.Last().GetType() != page.GetType())
            {
                await nav.PushModalAsync(page, animated);
            }
        }

        public static async Task PushAsyncSingle(this INavigation nav, Page page, bool animated = false)
        {
            if (nav.NavigationStack.Count == 0 ||
                nav.NavigationStack.Last().GetType() != page.GetType())
            {
                await nav.PushAsync(page, animated);
            }
        }
    }
}
