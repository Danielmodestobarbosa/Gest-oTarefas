using System;
using System.Collections.Generic;
using System.Text;

namespace Tarefas.Domain.Common.Eexceptions;

public class DomainException : Exception
{
    public DomainException(string message) : base(message)
    {
    }

    //Método estático para validações de "pré-condição"
    public static void When (bool hasError, string message)
    {
        if (hasError)
        {
            throw new DomainException(message);
        }    
    }

}

