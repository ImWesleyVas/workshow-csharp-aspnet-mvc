using System;
using System.Collections.Generic;


namespace SallesWebMvc.Services.Exceptions
{
    public class IntegrityException : ApplicationException
    {
        public IntegrityException(string message) : base(message)
        {
        }
                   
    }
}
