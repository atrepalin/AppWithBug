using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinLib
{
    public enum FormBorderStyle
    {
        /// <summary>
        ///  No border.
        /// </summary>
        None = 0,

        /// <summary>
        ///  A fixed, single line border.
        /// </summary>
        FixedSingle = 1,

        /// <summary>
        ///  A fixed, three-dimensional border.
        /// </summary>
        Fixed3D = 2,

        /// <summary>
        ///  A thick, fixed dialog-style border.
        /// </summary>
        FixedDialog = 3,

        /// <summary>
        ///  A resizable border.
        /// </summary>
        Sizable = 4,

        /// <summary>
        ///  A tool window border that is not resizable.
        /// </summary>
        FixedToolWindow = 5,

        /// <summary>
        ///  A resizable tool window border.
        /// </summary>
        SizableToolWindow = 6,
    }
}
