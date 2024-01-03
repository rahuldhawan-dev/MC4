using System;

namespace MMSINC.Controls
{
    public interface IMenu
    {
        void AddKeyAndMethod(string value, EventHandler handler);
    }
}
