using BR.GOV.MG.BBP.SGAP.SeleniumTest.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.ObjectModel;
using System.Drawing;
using BBP.Test.Selenium.Interfaces;

namespace BR.GOV.MG.BBP.SGAP.SeleniumTest
{
    /// <summary>
    /// Implementa a interface ICustomWebElement
    /// </summary>
    public class CustomWebElement : ICustomWebElement
    {
        #region propriedades

        /// <summary>
        /// IWebElement manipulado internamente para suprir as necessidades da interface
        /// </summary>
        protected IWebElement webelement { get; set; }

        #endregion

        #region construtores

        /// <summary>
        /// 
        /// </summary>
        /// <param name="webelement"></param>
        public CustomWebElement(IWebElement webelement)
        {
            this.webelement = webelement;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="LoadElement">Função responsável por carregar o IWebElement</param>
        /// <param name="isLoadelement">Indica se o elemento interno deverá ser carregado com a função fornecida em LoadElement</param>
        public CustomWebElement(Func<IWebElement> LoadElement, bool isLoadelement = true)
        {
            this.Reload = LoadElement;

            if (isLoadelement)
                this.webelement = LoadElement();
        }

        #endregion

        #region Interface IWebElement

        public bool Displayed
        {
            get { return this.webelement.Displayed; }
        }

        public bool Enabled
        {
            get { return this.webelement.Enabled; }
        }

        public Point Location
        {
            get { return this.webelement.Location; }
        }

        public Func<IWebElement> _Reload
        {
            get;
            set;
        }


        public bool Selected
        {
            get { return this.webelement.Selected; }
        }

        public Size Size
        {
            get { return this.webelement.Size; }
        }

        public string TagName
        {
            get { return this.webelement.TagName; }
        }

        public string Text
        {
            get { return this.webelement.Text; }
        }



        public void Clear()
        {
            this.webelement.Clear();
        }

        public void Click()
        {
            this.webelement.Click();
        }

        public IWebElement FindElement(By by)
        {
            return this.webelement.FindElement(by);
        }

        public ReadOnlyCollection<IWebElement> FindElements(By by)
        {
            return this.webelement.FindElements(by);
        }

        public string GetAttribute(string attributeName)
        {
            return this.webelement.GetAttribute(attributeName);
        }

        public string GetCssValue(string propertyName)
        {
            return this.webelement.GetCssValue(propertyName);
        }

        public void SendKeys(string text)
        {
            this.webelement.SendKeys(text);
        }

        public void Submit()
        {
            this.webelement.Submit();
        }

        #endregion

        #region Conversores

        /// <summary>
        /// Converte o objeto para o tipo SelectElement
        /// </summary>
        /// <returns></returns>
        public SelectElement ConvertToSelectElement()
        {
            return (SelectElement)this.webelement;
        } 

        #endregion

        #region ICustomWebElement

        /// <summary>
        /// Método resposável por carregar elemento
        /// </summary>
        public Func<IWebElement> Reload
        {
            get
            {
                this.webelement = _Reload();
                return () => this.webelement;
            }

            set { _Reload = value; }
        }

        /// <summary>
        /// Indica se o elemento foi encontrado
        /// </summary>
        public bool IsFound
        {
            get
            {
                return this.webelement != null;
            }

        }

        #endregion
    }
}