using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;

namespace MMSINC.Testing.SeleniumMvc
{
    /// <summary>
    /// Represents a selenium object that can contain/query for elements.
    /// </summary>
    public interface IFindElements
    {
        IEnumerable<IBetterWebElement> FindElements(By constraint);
        IBetterWebElement FindElement(By constraint);
    }
}
