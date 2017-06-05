using System;
using System.Collections.Generic;
using System.Web;
using System.Configuration;
using System.Net.Mail;

namespace BBP.Util.Mail
{
    public class MailSender
    {

        #region [Properties]

        protected MailMessage mail;

        #region [MailConfig]

        /// <summary>
        /// Configuration of the mail 
        /// </summary>
        public struct MailConfiguration
        {
            public int Port;
            public string User;
            public string Password;
            public string SMTPAddress;
            public bool SSL;
            public bool IsHTML;
            public int Priority;
            public string MailFrom;
            public string NameFrom;
            public string Subject;
            public string MailBody;
        }

        protected MailConfiguration Configuracoes;
        
        #endregion
        
        #endregion

        #region [Constructors]

        public MailSender(MailConfiguration configuracoes)
        {
            mail = new MailMessage();
            this.Configuracoes = configuracoes;

        }

        #endregion

        #region Methods

        public void AddTo(string mailTo)
        {
            mail.To.Add(mailTo);
        }

        public void RemoveTo(string mailTo)
        {
            mail.To.Remove(new MailAddress(mailTo));
        }

        public void Attach(string completeFilePath)
        {
            mail.Attachments.Add(new Attachment(completeFilePath));
        }

        public void Detach(string completeFilePath)
        {
            mail.Attachments.Remove(new Attachment(completeFilePath));
        }

        public void Send()
        {

            mail.From = new MailAddress(Configuracoes.MailFrom , Configuracoes.NameFrom, System.Text.Encoding.UTF8);

            mail.Subject = Configuracoes.Subject;

            mail.SubjectEncoding = System.Text.Encoding.UTF8;

            mail.Body = Configuracoes.MailBody;

            mail.BodyEncoding = System.Text.Encoding.UTF8;

            mail.IsBodyHtml = Configuracoes.IsHTML;

            //Prioridade do E-Mail
            if (Configuracoes.Priority < 2) //baixa
                mail.Priority = MailPriority.Low;
            else if (Configuracoes.Priority == 2) //normal
                mail.Priority = MailPriority.Normal;
            else if (Configuracoes.Priority > 2)//alta
                mail.Priority = MailPriority.High;

            SmtpClient client = new SmtpClient();  //Adicionando as credenciais do seu e-mail e senha:

            client.Credentials = new System.Net.NetworkCredential(Configuracoes.User, Configuracoes.Password);

            client.Port = Configuracoes.Port; // Esta porta é a utilizada pelo Gmail para envio

            client.Host = Configuracoes.SMTPAddress; //Definindo o provedor que irá disparar o e-mail

            client.EnableSsl = Configuracoes.SSL; //Gmail trabalha com Server Secured Layer

            //client.UseDefaultCredentials = false;

            //client.DeliveryMethod  = SmtpDeliveryMethod.Network;

            try
            {
                client.Send(mail);
            }

            catch (Exception ex)
            {
                string strEx = "";

                strEx = ex.Message + ex.StackTrace;
                int count = 0;

                Exception e = ex.InnerException;

                while (e != null && count < 6)
                {
                    count++;
                    strEx = strEx + e.Message + e.StackTrace;
                    e = e.InnerException;
                }

                throw new Exception(strEx);
            }

        }

        #endregion
    }
}