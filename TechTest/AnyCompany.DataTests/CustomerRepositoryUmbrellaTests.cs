using Microsoft.VisualStudio.TestTools.UnitTesting;
using AnyCompany.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using System.Data;
using System.Data.SqlClient;
using Shouldly;

namespace AnyCompany.Data.Tests
{
    [TestClass()]
    public class CustomerRepositoryUmbrellaTests
    {
        private CustomerRepositoryUmbrella _sut;
        private Mock<ISqlDataContext> _mockContext;
        private Mock<IDbCommand> _mockDbCommand;
        private Mock<IDataReader> _mockDataReader = new Mock<IDataReader>();
        private List<List<string>> _customers = new List<List<string>> { new List<string> { "Bob", "10/09/2018", "UK", "1" } };
    private List<string> _columns = new List<string> { "Name", "DateOfBirth", "Country", "Id" };

        private Mock<IDataReader> MockIDataReader(List<string> columns, List<List<string>> ojectsToEmulate)
        {
            var moq = new Mock<IDataReader>();

            // This var stores current position in 'ojectsToEmulate' list
            int count = -1;

            moq.Setup(x => x.Read())
                // Return 'True' while list still has an item
                .Returns(() => count < ojectsToEmulate.Count - 1)
                // Go to next position
                .Callback(() => count++);

            for (int columnIndex = 0; columnIndex < columns.Count; columnIndex++)
            {
                var column = columns[columnIndex];
                moq.Setup(x => x[column])
                    // Again, use lazy initialization via lambda expression
                    .Returns(() => { int indexOfColumn = columns.FindIndex(c=>c==column); return ojectsToEmulate[count][indexOfColumn]; });
            }
            return moq;
        }

        private void SetupDbCommand()
        {
            _mockDataReader = MockIDataReader(_columns, _customers);
            _mockDbCommand = new Mock<IDbCommand>();

            _mockDbCommand.Setup(x => x.ExecuteNonQuery()).Returns(1);
            _mockDbCommand.Setup(x => x.ExecuteReader()).Returns(_mockDataReader.Object);
            _mockContext.Setup(x => x.CreateCommand()).Returns(_mockDbCommand.Object);
        }

        [TestInitialize]
        public void Setup()
        {
            _mockContext = new Mock<ISqlDataContext>();
            
            _mockContext.Setup(x => x.CreateParameter(It.IsAny<string>(), It.IsAny<object>())).Returns((string x, object y) => { return new SqlParameter(x, y); });
            _mockContext.Setup(x => x.CreateOutputParameter(It.IsAny<string>(), It.IsAny<SqlDbType>())).Returns((string x, object y) => { return new SqlParameter(x, y); });
            _sut = new CustomerRepositoryUmbrella(_mockContext.Object);
        }

        [TestMethod()]
        public void GetAll_ReturnsEmptyIfNone()
        {
            _customers = new List<List<string>>();
            SetupDbCommand();
            var result = _sut.GetAll();
            result.ShouldBeEmpty();
        }

        [TestMethod()]
        public void GetAll_ReturnsSingleCustomerSuccessfully()
        {
            SetupDbCommand();
            var result = _sut.GetAll();
            result.ShouldHaveSingleItem();
        }

        [TestMethod()]
        public void GetAll_ReturnsMultiCustomersSuccessfully()
        {
            _customers.Add(new List<string> { "Bill", "10/08/1994", "UK", "4" });
            SetupDbCommand();
            var result = _sut.GetAll();
            result.Count().ShouldBe(2);
        }

        [TestMethod()]
        public void GetAll_ThrowsIfDateIncorrect()
        {
            _customers[0][1] = "100/100/10";
            SetupDbCommand();
            Should.Throw(() =>_sut.GetAll(), typeof(FormatException));
            
        }

        [TestMethod()]
        public void GetAllWithOrders_ReturnsEmptyIfNone()
        {
            _columns = new List<string> { "Customer_Id", "Country", "DateOfBirth", "Name", "Order_Id", "Amount", "VAT" };
            _customers = new List<List<string>> ();
            SetupDbCommand();
            var result = _sut.GetAllWithOrders();
            result.ShouldBeEmpty();
            
        }

        [TestMethod()]
        public void GetAllWithOrders_ReturnsSingleSuccessfully()
        {
            _columns = new List<string> { "Customer_Id", "Country", "DateOfBirth", "Name", "Order_Id", "Amount", "VAT" };
            _customers = new List<List<string>> { new List<string> { "1", "UK", "10/09/10", "Bob", "4", "20.50", "0.2" } };
            SetupDbCommand();
            var result = _sut.GetAllWithOrders();
            result.ShouldHaveSingleItem();
            result.First().Orders.ShouldHaveSingleItem();
        }

        [TestMethod()]
        public void GetAllWithOrders_ReturnsMultiOneCustomerSuccessfully()
        {
            _columns = new List<string> { "Customer_Id", "Country", "DateOfBirth", "Name", "Order_Id", "Amount", "VAT" };
            _customers = new List<List<string>> {
                new List<string> { "1", "UK", "10/09/10", "Bob", "4", "20.50", "0.2" },
                new List<string> { "1", "UK", "10/09/10", "Bob", "5", "22.50", "0.2" },
                new List<string> { "1", "UK", "10/09/10", "Bob", "7", "25.50", "0.2" }

            };
            SetupDbCommand();
            var result = _sut.GetAllWithOrders();
            result.ShouldHaveSingleItem();
            result.First().Orders.Count().ShouldBe(3);
        }

        [TestMethod()]
        public void GetAllWithOrders_ReturnsMultiOrdersMultiCustomerSuccessfully()
        {
            _columns = new List<string> { "Customer_Id", "Country", "DateOfBirth", "Name", "Order_Id", "Amount", "VAT" };
            _customers = new List<List<string>> {
                new List<string> { "1", "UK", "10/09/10", "Bob", "4", "20.50", "0.2" },
                new List<string> { "1", "UK", "10/09/10", "Bob", "5", "22.50", "0.2" },
                new List<string> { "2", "UK", "10/09/12", "Bill", "6", "29.50", "0.2" },
                new List<string> { "1", "UK", "10/09/10", "Bob", "7", "25.50", "0.2" },
                new List<string> { "2", "UK", "10/09/12", "Bill", "8", "39.50", "0.2" }

            };
            SetupDbCommand();
            var result = _sut.GetAllWithOrders();
            result.Count().ShouldBe(2);
            result.First().Orders.Count().ShouldBe(3);
            result.Last().Orders.Count().ShouldBe(2);
        }
    }
}