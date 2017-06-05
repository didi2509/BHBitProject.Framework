using BR.GOV.MG.BBP.SGAP.SeleniumTest;
using BR.GOV.MG.BBP.SGAP.SeleniumTest.Interfaces;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BBP.Test.Selenium.Enums;

namespace BBP.Test.Selenium.Extensions
{
    public static class WebDriverExtensions
    {

        #region [Javascript]

        /// <summary>
        /// Evnia código javascript para ser executado na página
        /// </summary>
        /// <param name="webDriver"></param>
        /// <param name="script">script a ser executado</param>
        public static void ExecuteScript(this IWebDriver webDriver, string script)
        {
            ((IJavaScriptExecutor)webDriver).ExecuteScript(script);
        }


        /// <summary>
        /// Executa o comando Trigger do jQuery para disparar um evento via Script, se necessário espera um tempo informado antes de disparar o comando
        /// </summary>
        /// <param name="webDriver"></param>
        /// <param name="selector">Seletor do objeto no qual será acionado o evento</param>
        /// <param name="waitTime">Tempo a ser esperado antes de executar a ação</param>
        /// <param name="triggerEvent">Evento do objeto a ser disparado</param>
        public static void JQueryTriggerBySelector(this IWebDriver webDriver, string selector, int maxMilliSecondsWaitTime = 300, TriggerEvent triggerEvent = TriggerEvent.Click)
        {
            if (maxMilliSecondsWaitTime > 0) Thread.Sleep(maxMilliSecondsWaitTime);

            string executeEvent = $"$(\"{selector}\").trigger('{ SeleniumUtils.GetEventName(triggerEvent) }')";
            ((IJavaScriptExecutor)webDriver).ExecuteScript(executeEvent);
        }

        #endregion

        #region [Methods]

        #region [Find]

        /// <summary>
        /// Busca uma coleção de elementos através do attribute name
        /// </summary>
        /// <param name="webDriver"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static ReadOnlyCollection<IWebElement> FindElementsByName(this IWebDriver webDriver, string name)
        {
            return webDriver.FindElements(By.Name(name));
        }

        /// <summary>
        /// Busca uma coleção de elementos através do seletor informado
        /// </summary>
        /// <param name="webDriver"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        public static ReadOnlyCollection<IWebElement> FindElementsBySelector(this IWebDriver webDriver, string selector)
        {
            return webDriver.FindElements(By.CssSelector(selector));
        }

        /// <summary>
        /// Busca um elemento através do attribute name
        /// </summary>
        /// <param name="webDriver"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static ICustomWebElement FindElementByName(this IWebDriver webDriver, string name)
        {
            return new CustomWebElement(() => webDriver.FindElement(By.Name(name)));
        }

        /// <summary>
        /// Busca um elementos através de seu id (lembrando que o mesmo deve ser único na página)
        /// </summary>
        /// <param name="webDriver"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static ICustomWebElement FindElementByID(this IWebDriver webDriver, string id)
        {
            return new CustomWebElement(() => webDriver.FindElement(By.Id(id)));
        }


        /// <summary>
        /// Busca um elemento através de seu seletor
        /// </summary>
        /// <param name="webDriver"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        public static ICustomWebElement FindElementBySelector(this IWebDriver webDriver, string selector)
        {
            return new CustomWebElement(() => webDriver.FindElement(By.CssSelector(selector)));
        }

        /// <summary>
        /// Busca um elemento através de seu XPath
        /// </summary>
        /// <param name="webDriver"></param>
        /// <param name="xpath"></param>
        /// <returns></returns>
        public static ICustomWebElement FindElementByXPath(this IWebDriver webDriver, string xpath)
        {
            return new CustomWebElement(() => webDriver.FindElement(By.XPath(xpath)));
        }

        #endregion

        #endregion

        #region [Wait Methods]

        #region [Find]

        /// <summary>
        /// Busca um elemento através de seu attribute name, aguardando até que o mesmo seja renderizado
        /// </summary>
        /// <param name="webDriver"></param>
        /// <param name="name"></param>
        /// <param name="maxMilliSecondsWaitTime">Tempo máximo a ser esperado até que o componente tenha sido renderizado</param>
        /// <returns></returns>
        public static ICustomWebElement WaitFindByName(this IWebDriver webDriver, string name, int maxMilliSecondsWaitTime = 30000)
        {
            CustomWebElement customWebelement = new CustomWebElement(() =>
            {
                return SeleniumUtils.Wait(() => webDriver.FindElement(By.Name(name)), name,maxMilliSecondsWaitTime);
            });

            return customWebelement;
        }

        /// <summary>
        /// Busca um elemento através de seu attribute name, aguardando até que o mesmo seja renderizado
        /// </summary>
        /// <param name="webDriver"></param>
        /// <param name="name"></param>
        /// <param name="maxMilliSecondsWaitTime">Tempo máximo a ser esperado até que o componente tenha sido renderizado</param>
        /// <returns></returns>
        public static ICustomWebElement WaitFindByXPath(this IWebDriver webDriver, string xpath, int maxMilliSecondsWaitTime = 30000)
        {
            CustomWebElement customWebelement = new CustomWebElement(() =>
            {
                return SeleniumUtils.Wait(() => webDriver.FindElement(By.XPath(xpath)), xpath,maxMilliSecondsWaitTime);
            });

            return customWebelement;
        }

        /// <summary>
        /// Busca um elemento através de seu seletor, aguardando até que o mesmo seja renderizado com um tiemout de 30 segundos
        /// </summary>
        /// <param name="webDriver"></param>
        /// <param name="selector"></param>
        /// <param name="maxMilliSecondsWaitTime">Tempo máximo a ser esperado até que o componente tenha sido renderizado</param>
        /// <returns></returns>
        public static ICustomWebElement WaitFindBySelector(this IWebDriver webDriver, string selector, int maxMilliSecondsWaitTime = 30000)
        {
            CustomWebElement customWebelement = new CustomWebElement(() => {
                return SeleniumUtils.Wait(() => webDriver.FindElement(By.CssSelector(selector)),selector, maxMilliSecondsWaitTime);
            });

            return customWebelement;
        }


        /// <summary>
        /// Busca uma lista de elementos através de um seletor
        /// </summary>
        /// <param name="webDriver"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        public static ReadOnlyCollection<IWebElement> WaitFindElementsBySelector(this IWebDriver webDriver, string selector, int maxMilliSecondsWaitTime = 30000)
        {
            return SeleniumUtils.Wait(() => webDriver.FindElements(By.CssSelector(selector)), selector, maxMilliSecondsWaitTime);
        }


        /// <summary>
        /// Busca um elemento em uma lista através de seu seletor e índice, aguardando até que o mesmo seja renderizado com um tiemout de 30 segundos
        /// </summary>
        /// <param name="webDriver"></param>
        /// <param name="selector"></param>
        /// <param name="indice">Índice do elemento a ser retornado</param>
        /// <param name="maxMilliSecondsWaitTime">Tempo máximo a ser esperado até que o componente tenha sido renderizado</param>
        /// <returns></returns>
        public static ICustomWebElement WaitFindElementsBySelector(this IWebDriver webDriver, string selector, int indice, int maxMilliSecondsWaitTime = 30000)
        {
            CustomWebElement customWebelement = new CustomWebElement(() =>
            {
                return SeleniumUtils.Wait(() =>
                {
                    var list = webDriver.FindElements(By.CssSelector(selector));

                    if (list.Count < indice)
                    {
                        return null;
                    }
                    else
                    {
                        return list[indice];
                    }
                }, selector, maxMilliSecondsWaitTime);
            });

            return customWebelement;
        }

        /// <summary>
        /// Encontra um elemento através de seu ID (lembrando que o ID deverá ser único na página) e aguarda até que o mesmo tenha sido renderizado
        /// </summary>
        /// <param name="webDriver"></param>
        /// <param name="id"></param>
        /// <param name="maxMilliSecondsWaitTime">Tempo máximo a ser esperado até que o componente tenha sido renderizado</param>
        /// <returns></returns>
        public static ICustomWebElement WaitFindById(this IWebDriver webDriver, string id, int maxMilliSecondsWaitTime = 30000)
        {
            CustomWebElement customWebelement = new CustomWebElement(() =>
            {
                return SeleniumUtils.Wait(() => webDriver.FindElement(By.Id(id)), id, maxMilliSecondsWaitTime);
            });

            return customWebelement;
        }

        #endregion

        #region [Page]

        /// <summary>
        /// Navega até uma URL, aguardando até que a URL do browser tenha sido de fato trocada
        /// </summary>
        /// <param name="web"></param>
        /// <param name="url"></param>
        /// <param name="contains">Indica a URL fonecida deverá fazer apenas parte da URL carregada</param>
        /// <param name="maxMilliSecondsWaitTime">Tempo máximo a ser esperado até que a url tenha sido trocada</param>
        /// <returns></returns>
        public static void WaitGoToUrl(this IWebDriver web, string url, bool contains = true, int maxMilliSecondsWaitTime = 30000)
        {
            SeleniumUtils.Wait(() =>
            {
                try
                {
                    web.Navigate().GoToUrl(url);
                    web.WaitPage(url, contains);
                    return new object();
                }
                catch { return null; }
            }, url, maxMilliSecondsWaitTime);
        }

        /// <summary>
        /// Aguarda até ´que a URL do Browser tenha sido trocada pela URL informada
        /// </summary>
        /// <param name="webDriver"></param>
        /// <param name="url"></param>
        /// <param name="contains">Indica se sua URL fornecida deverá fazer apenas parte da URL carregada</param>
        /// <param name="maxMilliSecondsWaitTime">Tempo máximo a ser esperado até que o componente tenha sido renderizado</param>
        /// <returns></returns>
        public static void WaitPage(this IWebDriver webDriver, string url, bool contains = false, int maxMilliSecondsWaitTime = 30000)
        {
            if (String.IsNullOrEmpty(url)) return;

            string webDriverURL = webDriver.Url.ToUpper();
            url = url.ToUpper();

            SeleniumUtils.Wait<object>(() =>
            {
                if ((contains && webDriverURL.Contains(url))
                     || (webDriverURL.ToUpper() == url)
                )
                {
                    Thread.Sleep(1000);
                    return new object();
                }
                else
                {
                    return null;
                }
            }, url, maxMilliSecondsWaitTime);
        }

        #endregion

        #endregion
    }
}
