using System;

namespace projFront.Models.Exception
{
    public class NumeroNotaInvalidoException : IOException
    {
        public NumeroNotaInvalidoException(string message) : base(message)
        {
            
        }
    }
}
