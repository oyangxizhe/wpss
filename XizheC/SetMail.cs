using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XizheC
{
    public class SetMail
    {
public SetMail()
{

 MailAddress from = new MailAddress("uay022@126.com");
            MailAddress to = new MailAddress("13511634094@163.com");
            System.Net.Mail.MailMessage mm = new System.Net.Mail.MailMessage(from ,to );
            mm.Subject = "ok wetwetwetwetwetwtwt";
            mm.Body = "testefwerwerwe wtwetwetwt";
            SmtpClient sc = new SmtpClient("smtp.126.com",25);
            System.Net.NetworkCredential nc = new System.Net.NetworkCredential("uay022@126.com", "");
            sc.Credentials = nc;
            //Console .Write ("{0}+{1}+{2}",to.User ,to.Host ,sc.Host );
            sc.Send(mm);


}
    }
}
