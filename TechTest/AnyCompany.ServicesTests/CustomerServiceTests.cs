using Microsoft.VisualStudio.TestTools.UnitTesting;
using AnyCompany.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnyCompany.Data;
using Moq;
using Shouldly;

namespace AnyCompany.Services.Tests
{
    [TestClass()]
    public class CustomerServiceTests
    {
        private CustomerService _sut;
        private Mock<IUoW> _uow = new Mock<IUoW>();
        private Mock<ICustomerRepository> _mockCustomerRepository;
        private CustomerRequest _fullRequest = new CustomerRequest { Country = "UK", Name = "Bob", DateOfBirth = DateTime.Now };
        private CustomerRequest _missingCountry = new CustomerRequest { Name = "Bob", DateOfBirth = DateTime.Now };
        private CustomerRequest _missingName = new CustomerRequest { Country = "UK", DateOfBirth = DateTime.Now };
        private CustomerRequest _missingDoB = new CustomerRequest { Country = "UK", Name = "Bob" };

        [TestInitialize]
        public void Setup()
        {
            _mockCustomerRepository = new Mock<ICustomerRepository>();
            _uow.Setup(x => x.Customers).Returns(_mockCustomerRepository.Object);
            _sut = new CustomerService(_uow.Object);
            
        }

        [TestMethod()]
        public void AddCustomer_EmptyNameShouldThrow()
        {
            Should.Throw(() => _sut.AddCustomer(_missingName), typeof(Exception));
        }

        [TestMethod()]
        public void AddCustomer_EmptyCountryShouldThrow()
        {
            Should.Throw(() => _sut.AddCustomer(_missingCountry), typeof(Exception));
        }

        [TestMethod()]
        public void AddCustomer_EmptyDateShouldThrow()
        {
            Should.Throw(() => _sut.AddCustomer(_missingDoB), typeof(Exception));
        }

        [TestMethod]
        public void AddCustomer_FullRequestCallsCreateAndCommit()
        {
            _sut.AddCustomer(_fullRequest);
            _mockCustomerRepository.Verify(x => x.Create(It.IsAny<Customer>()), Times.Once);
            _uow.Verify(x => x.Commit(), Times.AtLeastOnce);
        }
    }
}