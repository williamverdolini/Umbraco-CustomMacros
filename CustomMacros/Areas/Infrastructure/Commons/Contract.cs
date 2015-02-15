using System;
using System.Diagnostics;

namespace CustomMacros.Areas.Infrastructure.Commons
{
    /// <summary>
    /// Classe Custom che simula il comportamento System.Diagnostic.Contract
    /// </summary>
    public class Contract
    {
        public static void Requires<TException>(bool Predicate, string Message)
            where TException : Exception, new()
        {
            if (!Predicate)
            {
                Debug.WriteLine(Message);
                throw new TException();
            }
        }
    }
    // snippet code from: http://stackoverflow.com/questions/18793558/building-with-code-contracts

}