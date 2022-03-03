using System;

namespace CarWebApp.Exceptions
{
    public class ForbidException : Exception
    {
        public ForbidException(string? message) : base(message)
        {
        }
    }
}