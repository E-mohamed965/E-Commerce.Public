using DomainLayer.Models.OrderModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Specifications
{
    internal class OrderSpecification : BaseSpecification<Order,Guid>
    {
        //Get All Orders By Email
        public OrderSpecification(string email):base(O=>O.UserEmail==email)
        {
            AddInclude(O => O.Items);
            AddInclude(O => O.DeliveryMethod);

            AddOrderByDescending(O => O.OrdreDate);
        }
        //Get Order By Id
        public OrderSpecification( Guid id ): base(o=>o.Id==id)
        {
            AddInclude(O => O.Items);
            AddInclude(O => O.DeliveryMethod);

        }
    }
}
