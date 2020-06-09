using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using TransactionStore.Api.Tests.Helpers;
using TransactionStore.Data.Entities;
using TransactionStore.Data.Enums;
using Xunit;

namespace TransactionStore.Api.Tests.Controllers
{
    public class TransactionsControllerTests
    {
        public TransactionsControllerTests()
        {
            _factory = new TransactionStoreWebFactory();
        }

        private readonly TransactionStoreWebFactory _factory;


        private async Task PopulateDb()
        {
            await _factory.PopulateContextAsync(new PaymentTransactionEntity
            {
                TransactionId = "1",
                TransactionDate = new DateTime(2020, 01, 01),
                Amount = 200,
                Status = TransactionStatus.A,
                Currency = "USD"
            });

            await _factory.PopulateContextAsync(new PaymentTransactionEntity
            {
                TransactionId = "2",
                TransactionDate = new DateTime(2020, 02, 01),
                Amount = 200,
                Status = TransactionStatus.D,
                Currency = "EUR"
            });

            await _factory.PopulateContextAsync(new PaymentTransactionEntity
            {
                TransactionId = "3",
                TransactionDate = new DateTime(2020, 04, 01),
                Amount = 200,
                Status = TransactionStatus.D,
                Currency = "GBP"
            });

            await _factory.PopulateContextAsync(new PaymentTransactionEntity
            {
                TransactionId = "4",
                TransactionDate = new DateTime(2020, 05, 01),
                Amount = 200,
                Status = TransactionStatus.A,
                Currency = "USD"
            });
        }

        [Fact]
        public async Task Should_GetFiltered_When_CurrencyFilterProvided()
        {
            //Arrange
            await PopulateDb();

            //Act
            var result = await _factory.TransactionsClient.GetFilteredAsync("Currency eq 'USD'", "", "", "");

            //Assert
            result.Count.Should().Be(2);
            result.Items.Select(t => t.TransactionId).Should().Contain(new[] {"1", "4"});
        }

        [Fact]
        public async Task Should_GetFiltered_When_DateRangeFilterProvided()
        {
            //Arrange
            await PopulateDb();

            //Act
            var result = await _factory.TransactionsClient
                .GetFilteredAsync("TransactionDate gt 2020-01-23T03:30:00.000Z and TransactionDate lt 2020-04-26T03:30:00.000Z",
                    "", "", "");

            //Assert
            result.Count.Should().Be(2);
            result.Items.Select(t => t.TransactionId).Should().Contain(new[] {"2", "3"});
        }

        [Fact]
        public async Task Should_GetFiltered_When_StatusFilterProvided()
        {
            //Arrange
            await PopulateDb();

            //Act
            var result = await _factory.TransactionsClient.GetFilteredAsync("Status eq 'D'", "", "", "");

            //Assert
            result.Count.Should().Be(2);
            result.Items.Select(t => t.TransactionId).Should().Contain(new[] {"2", "3"});
        }
    }
}