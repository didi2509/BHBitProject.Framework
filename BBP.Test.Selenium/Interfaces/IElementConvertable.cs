using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BBP.Test.Selenium.Interfaces
{
    public interface IElementConvertable
    {
        SelectElement ConvertToSelectElement();
    }
}
