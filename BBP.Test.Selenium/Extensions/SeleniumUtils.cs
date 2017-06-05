using BR.GOV.MG.BBP.SGAP.SeleniumTest.Interfaces;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using BBP.Test.Selenium.Enums;

namespace BBP.Test.Selenium.Extensions
{
    internal static class SeleniumUtils
    {
        /// <summary>
        /// Tenta buscar um elemento de forma a esperar até que o mesmo esteja disponível
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="method">Metodo que retorna o elemento</param>
        /// <param name="waitTime">Tempo a ser esperado para se efetuar uma nova tentativa de buscar o elemento, em milisegundos</param>
        /// <param name="totalAttempts">Total de máxima de tentativas de buscar o elemento</param>
        /// <returns></returns>
        internal static T Wait<T>(Func<T> method, string elementSearch, int maxMilliSecondsWaitTime = 30000)
        {
            int cont = 0;

            T element = default(T);

            Stopwatch timer = new Stopwatch();
            timer.Start();

            while (element == null)
            {
                try
                {
                    element = method();
                }
                catch { }

                if (element == null)
                {
                    Thread.Sleep(350);
                    cont++;

                    //ToDo - permitir que a mensagem seja modificada pois nem sempre a mesma se remete a um elemento
                    if (timer.ElapsedMilliseconds > maxMilliSecondsWaitTime)
                        throw new Exception($"Elemento {elementSearch} não encontrado");
                }
            }

            timer.Stop();

            return element;
        }


        /// <summary>
        /// Retorna o nome do evento a ser disparado de acordo com o TriggerEvent informado
        /// </summary>
        /// <param name="triggerEvent"></param>
        /// <returns></returns>
        internal static string GetEventName(TriggerEvent triggerEvent)
        {
            switch (triggerEvent)
            {
                case TriggerEvent.Click: return "click";
                case TriggerEvent.MouseUp: return "mouseup";
                case TriggerEvent.KeyPress: return "keypress";
                default: return "click";
            }
        }
    }
}
