using GenericRepositoryMVCApp.BLL;
using GenericRepositoryMVCApp.DAL;
using GenericRepositoryMVCApp.Data;
using GenericRepositoryMVCApp.Models;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace GenericRepoTests
{
    [TestClass]
    public class CustomerBLUnitTests
    {
        private CustomerBusinessLogic BusinessLogic;
        public CustomerBLUnitTests()
        {
            // DbSet
            var data = new List<Customer>
            {
                new Customer{FullName = "Zach"},
                new Customer{FullName = "Zech"},
                new Customer{FullName = "Zat"},
                new Customer{FullName = "Jane"}
            }.AsQueryable();

            var mockDbSet = new Mock<DbSet<Customer>>();

            mockDbSet.As<IQueryable<Customer>>().Setup(m => m.Provider).Returns(data.Provider);
            mockDbSet.As<IQueryable<Customer>>().Setup(m => m.Expression).Returns(data.Expression);
            mockDbSet.As<IQueryable<Customer>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockDbSet.As<IQueryable<Customer>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator);

            var mockContext = new Mock<GenericRepositoryMVCAppContext>();
            mockContext.Setup(c => c.Customers).Returns(mockDbSet.Object);

            BusinessLogic = new CustomerBusinessLogic(new CustomerRepository(mockContext.Object));
        }

        [DataRow("Za", 2)]
        [DataRow("Z", 3)]
        [DataRow("F", 0)]
        [TestMethod]
        public void GetCustomerStartingWith_ValidInput_ReturnsAllMatchingcustomers(string nameInput, int outputCount)
        {
            // arranged
            // act
            List<Customer> customers = BusinessLogic.GetCustomersStartingWith(nameInput).ToList();

            // Assert
            Assert.AreEqual(outputCount, customers.Count);
        }
    }
}