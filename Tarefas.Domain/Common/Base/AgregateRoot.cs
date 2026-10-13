using System;
using System.Collections.Generic;
using System.Text;

//Classe base abstrata para todas as Raízes de Agreado (Aggregate Roots) no domínio da aplicação.
//Serve como um marcador de intenção para impor regras de consistencia transacional
namespace Tarefas.Domain.Common.Base
{
    public abstract class AggregateRoot : Entity
    {
        //A proteção da classe garante que o Agregate só poder ser instanciada
        //por suas classes concretas
        protected AggregateRoot() : base()
        {
        }

        //Construtor opcional para ORMs que injetam o id
        protected AggregateRoot(Guid id) : base(id)
        {
        }
    }
}

