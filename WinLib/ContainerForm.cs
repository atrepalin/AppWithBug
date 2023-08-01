using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinLib
{
    public interface IContainerForm
    {
        void Enclose(IntPtr hWnd);
        IntPtr EnclosedHandle { get; }
        string TitleText { get; set; }
        event EventHandler FormClosedSimple;
    }

    public static class ContainerForm
    {
        public static WinForm CreateContainerForm(bool darkTheme = false)
        {
            var form = new WinForm(darkTheme);
            form.Show();
            return form;
        }

        public static WinForm CreateContainerForm(int width, int height, bool darkTheme = false)
        {
            var form = new WinForm(darkTheme)
            {
                Width = width,
                Height = height
            };
            form.Show();
            return form;
        }

        public static bool HasParent(IntPtr hWnd) => WinForm.HasParent(hWnd);
    }
}