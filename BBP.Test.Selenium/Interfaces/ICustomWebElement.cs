using OpenQA.Selenium;
using System;
using BBP.Test.Selenium.Interfaces;

namespace BR.GOV.MG.BBP.SGAP.SeleniumTest.Interfaces
{
    /// <summary>
    /// Interface para Webelements customizados, herda da interface IWebElement
    /// </summary>
    public interface ICustomWebElement : IWebElement, IElementConvertable
    {
        /// <summary>
        /// Função responsável por encontrar o elemento
        /// </summary>
        Func<IWebElement> Reload { get; set; }

        /// <summary>
        /// Indica se o elemento foi encontrado
        /// </summary>
        bool IsFound { get; }
    }
}
