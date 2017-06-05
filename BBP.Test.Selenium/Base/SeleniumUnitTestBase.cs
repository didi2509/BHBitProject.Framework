using BR.GOV.MG.BBP.SGAP.SeleniumTest.Interfaces;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using BBP.Test.Selenium.Enums;


namespace BBP.Test.Selenium.Base
{
    /// <summary>
    /// Classe base para o teste do selenium
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class SeleniumUnitTestBase<T> where T : ISeleniumDriverCreator, new()
    {

        #region [Properties]

        /// <summary>
        /// Responsável por criar o driver do Selenium
        /// </summary>
        private ISeleniumDriverCreator _seleniumDriverCreator { get; set; }
        private ISeleniumDriverCreator seleniumDriverCreator { get { return (this._seleniumDriverCreator ?? (this._seleniumDriverCreator = new T())); } }

        #endregion

        /// <summary>
        /// Método que irá conter o teste
        /// </summary>
        /// <param name="driverNavigator">Navegador utilizado</param>
        public abstract void DoTest(IWebDriver driverNavigator);

        /// <summary>
        /// Executa o método Thread.Sleep, fornecendo os millisegundos a serem aguardados
        /// </summary>
        /// <param name="milliseconds"></param>
        public void Wait(int milliseconds)
        {
            Thread.Sleep(milliseconds);
        }

        /// <summary>
        /// Executa o teste de acordo com o navegador utilizado, basendo-se no que está implementado em DoTest
        /// </summary>
        /// <param name="typeDriver"></param>
        /// <param name="dispose"></param>
        /// <returns>Retorna o log do teste</returns>
        public List<KeyValuePair<string, string>> ExecuteTest(TypeTestBrowser typeDriver, bool dispose = true)
        {
            //Navegador
            IWebDriver driverNavigator = seleniumDriverCreator.CreateDriver(typeDriver);

            //Log
            List<KeyValuePair<string, string>> logs = null;

            try
            {
                DoTest(driverNavigator);
            }
            catch
            {
                throw;
            }
            finally
            {
                //O dispose fecha o Browser
                if (driverNavigator != null)
                {
                    logs = this.GetLogs(driverNavigator);

                    if (dispose)
                        driverNavigator.Dispose();
                }
            }

            return logs;
        }


        /// <summary>
        /// Método responsável por gerar o log do teste
        /// </summary>
        /// <param name="driverNavigator"></param>
        /// <returns></returns>
        public virtual List<KeyValuePair<string, string>> GetLogs(IWebDriver driverNavigator)
        {
            List<KeyValuePair<string, string>> logs = new List<KeyValuePair<string, string>>();

            try
            {
                //O dispose fecha o Browser
                if (driverNavigator != null)
                {
                    IOptions options = driverNavigator.Manage();

                    if (options.Logs != null && options.Logs.AvailableLogTypes != null)
                    {
                        options.Logs.AvailableLogTypes.ToList().ForEach(logType =>
                        {
                            options.Logs.GetLog(logType).ToList().ForEach(log => logs.Add(new KeyValuePair<string, string>(logType, log.Message)));
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                logs.Add(new KeyValuePair<string, string>("Problemas ao gerar o log", ex.Message));
            }

            return logs;
        }
    }
}
