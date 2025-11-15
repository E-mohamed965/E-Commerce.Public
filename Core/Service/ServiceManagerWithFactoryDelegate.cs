using ServiceAbstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    internal class ServiceManagerWithFactoryDelegate(  Func<IProductService> ProductFactory 
                                                     , Func<IBasketService> BasketFactory 
                                                     , Func<IAuthenticationService> AutheticationFactory
                                                     , Func<IOrderService> OrderFactory
                                                     , Func<IPaymentService> PaymentFactory) 
    {
        public IProductService ProductService => ProductFactory.Invoke();

        public IBasketService BasketService => BasketFactory.Invoke();

        public IAuthenticationService AuthenticationService => AutheticationFactory.Invoke();

        public IOrderService OrderService => OrderFactory.Invoke();

        public IPaymentService PaymentService => PaymentFactory.Invoke();
    }
}
