using Microsoft.VisualStudio.TestTools.UnitTesting;
using AnyCompany.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using AnyCompany.Data;
using Shouldly;

namespace AnyCompany.Services.Tests
{
    [TestClass()]
    public class OrderServiceTests
    {
        private OrderService _sut;
        private Mock<IUoW> _uow;
        private Mock<IOrderRepository> _mockOrderRepository;
        private Mock<ICustomerRepository> _mockCustomerRepository;
        private Mock<ICustomerService> _mockCustomerService;
        private OrderRequest _fullNewCusRequest = new OrderRequest { Amount = 1, CustomerCountry = "UK", CustomerId = null, CustomerName = "CusName", DateOfBirth = DateTime.Now };
        private OrderRequest _nonUkRequest = new OrderRequest { Amount = 1, CustomerCountry = "Brazil", CustomerId = -1, CustomerName = "B", DateOfBirth = DateTime.Now };
        private OrderRequest _negativeAmountRequest = new OrderRequest { Amount = -1, CustomerId = 1 };
        [TestInitialize]
        public void Setup()
        {
            _uow = new Mock<IUoW>();
            _mockOrderRepository = new Mock<IOrderRepository>();
            _mockCustomerRepository = new Mock<ICustomerRepository>();
            _mockCustomerService = new Mock<ICustomerService>();
            _mockCustomerService.Setup(c => c.GetCustomer(It.IsAny<int>())).Returns(new Customer { Country = "UK" });

            _mockOrderRepository.Setup(c => c.Create(It.IsAny<Order>()));

            _uow.Setup(x => x.Orders).Returns(_mockOrderRepository.Object);
            _uow.Setup(x => x.Customers).Returns(_mockCustomerRepository.Object);
            _mockCustomerService.Setup(x => x.AddCustomer(It.Is<CustomerRequest>(c => c.Country == _fullNewCusRequest.CustomerCountry && c.Name == _fullNewCusRequest.CustomerName))).Returns(2);
            _sut = new OrderService(_uow.Object, _mockCustomerService.Object);
        }

        [TestMethod()]
        public void AddOrder_NewCustomerCallsCustomerService()
        {
            _sut.AddOrder(_fullNewCusRequest);
            _mockCustomerService.Verify(c => c.AddCustomer(It.Is<CustomerRequest>(cu => cu.Country == _fullNewCusRequest.CustomerCountry && cu.Name == _fullNewCusRequest.CustomerName)), Times.Once);
            _uow.Verify(x => x.Commit(), Times.AtLeastOnce);
        }

        [TestMethod]
        public void AddOrder_NegativeAmountDoesntAddToDB()
        {
            Should.Throw(() =>_sut.AddOrder(_negativeAmountRequest), typeof(Exception));
            _mockOrderRepository.Verify(x => x.Create(It.IsAny<Order>()), Times.Never);
            _uow.Verify(x => x.Commit(), Times.Never);
        }

        [TestMethod]
        public void AddOrder_UKCountrySetsVAT()
        {
            _sut.AddOrder(_fullNewCusRequest);
            _mockOrderRepository.Verify(x => x.Create(It.Is<Order>(o => o.VAT == 0.2M)), Times.Once);
            _uow.Verify(x => x.Commit(), Times.AtLeastOnce);
        }

        [TestMethod]
        public void AddOrder_NonUKCountryNoVAT()
        {
            _sut.AddOrder(_nonUkRequest);
            _mockOrderRepository.Verify(x => x.Create(It.Is<Order>(o => o.VAT == 0)), Times.Once);
            _uow.Verify(x => x.Commit(), Times.AtLeastOnce);
        }
        
    }
}