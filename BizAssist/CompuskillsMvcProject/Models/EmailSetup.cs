using System;

namespace CompuskillsMvcProject.Models
{
    public class EmailSetup
    {
        public string SenderName { get; set; }
        public string Sender { get; set; }
		public string RecieverName { get; set; }
		public string Reciever { get; set; }
	   public string Subject { get; set; }
	  public string Content { get; set; }
        public string EmailPassword { get; set; }
    }
}



