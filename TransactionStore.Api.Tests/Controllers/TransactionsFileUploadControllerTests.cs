using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using TransactionStore.Api.Tests.Helpers;
using TransactionStore.Client.Contracts;
using TransactionStore.Data.Entities;
using Xunit;
using TransactionStatus = TransactionStore.Data.Enums.TransactionStatus;

namespace TransactionStore.Api.Tests.Controllers
{
    public class TransactionsFileUploadControllerTests
    {
        public TransactionsFileUploadControllerTests()
        {
            _factory = new TransactionStoreWebFactory();
        }

        private const string CorrectCsvFilePath = "TestData/test.csv";
        private const string CorrectXmlFilePath = "TestData/test.xml";
        private const string IncorrectCsvFilePath = "TestData/test-incorrect.csv";
        private const string IncorrectXmlFilePath = "TestData/test-incorrect.xml";

        private readonly TransactionStoreWebFactory _factory;

        [Fact]
        public async Task Should_UploadCsvFile_When_UploadFileCalled()
        {
            // Arrange
            var fileInfo = new FileInfo(CorrectCsvFilePath);

            var formFile = fileInfo.ToFormFile();

            // Act
            await _factory.FileUploadClient.UploadFileAsync(formFile);

            //Assert
            var transactions = await _factory.Context.Transactions.ToListAsync();
            transactions.Count.Should().Be(3);
            transactions.Select(t => t.TransactionId).Should()
                .Contain(new[] {"Invoice0000003", "Invoice0000002", "Invoice0000004"});
        }

        [Fact]
        public async Task Should_UploadCsvFileAndUpdateRecords_When_RecordExists()
        {
            // Arrange
            var fileInfo = new FileInfo(CorrectCsvFilePath);

            var formFile = fileInfo.ToFormFile();

            await _factory.PopulateContextAsync(new PaymentTransactionEntity
            {
                TransactionDate = DateTime.UtcNow,
                TransactionId = "Invoice0000002",
                Currency = "EUR",
                Amount = 200,
                Status = TransactionStatus.R
            });

            await _factory.Context.SaveChangesAsync();

            _factory.DetachAllEntities();

            // Act
            await _factory.FileUploadClient.UploadFileAsync(formFile);

            //Assert
            var transactions = await _factory.Context.Transactions.ToListAsync();
            transactions.Count.Should().Be(3);
            transactions.Select(t => t.TransactionId).Should()
                .Contain(new[] {"Invoice0000003", "Invoice0000002", "Invoice0000004"});
            var updatedEntity = transactions.First(x => x.TransactionId == "Invoice0000002");
            updatedEntity.Amount.Should().Be(22);
            updatedEntity.Currency.Should().Be("USD");
        }

        [Fact]
        public async Task Should_UploadXmlFile_When_UploadFileCalled()
        {
            // Arrange
            var fileInfo = new FileInfo(CorrectXmlFilePath);

            var formFile = fileInfo.ToXmlFormFile();

            // Act
            await _factory.FileUploadClient.UploadFileAsync(formFile);

            //Assert
            var transactions = await _factory.Context.Transactions.ToListAsync();
            transactions.Count.Should().Be(2);
            transactions.Select(t => t.TransactionId).Should()
                .Contain(new[] {"Inv00001", "Inv00002"});
        }

        [Fact]
        public async Task Should_UploadXmlFileAndUpdateRecords_When_RecordExists()
        {
            // Arrange
            var fileInfo = new FileInfo(CorrectXmlFilePath);

            var formFile = fileInfo.ToXmlFormFile();

            await _factory.PopulateContextAsync(new PaymentTransactionEntity
            {
                TransactionDate = DateTime.UtcNow,
                TransactionId = "Inv00002",
                Currency = "EUR",
                Amount = 200,
                Status = TransactionStatus.R
            });

            await _factory.Context.SaveChangesAsync();

            _factory.DetachAllEntities();

            // Act
            await _factory.FileUploadClient.UploadFileAsync(formFile);

            //Assert
            var transactions = await _factory.Context.Transactions.ToListAsync();
            transactions.Count.Should().Be(2);
            transactions.Select(t => t.TransactionId).Should()
                .Contain(new[] {"Inv00001", "Inv00002"});
            var updatedEntity = transactions.First(x => x.TransactionId == "Inv00002");
            updatedEntity.Amount.Should().Be(10000);
            updatedEntity.Currency.Should().Be("USD");
        }

        [Fact]
        public async Task ShouldNot_UploadCsvFile_When_FileHasNotValidRecords()
        {
            // Arrange
            var fileInfo = new FileInfo(IncorrectCsvFilePath);

            var formFile = fileInfo.ToFormFile();

            // Act
            try
            {
                await _factory.FileUploadClient.UploadFileAsync(formFile);
            }
            //Assert
            catch (TransactionStoreRequestException<ValidationProblemDetails> e)
            {
                var x = await _factory.Context.Transactions.ToListAsync();

                e.Result.Errors[nameof(PaymentTransaction.Amount)].First().Should().Be("Can't parse element. Line 2");
                e.Result.Errors[nameof(PaymentTransaction.Currency)].First().Should()
                    .Be("Value USDF has the wrong currency format. Line 3");
                e.Result.Errors[nameof(PaymentTransaction.Status)].First().Should().Be("Can't parse element. Line 1");
            }
        }

        [Fact]
        public async Task ShouldNot_UploadXmlFile_When_FileHasNotValidRecords()
        {
            // Arrange
            var fileInfo = new FileInfo(IncorrectXmlFilePath);

            var formFile = fileInfo.ToXmlFormFile();

            // Act
            try
            {
                await _factory.FileUploadClient.UploadFileAsync(formFile);
            }
            //Assert
            catch (TransactionStoreRequestException<ValidationProblemDetails> e)
            {
                var x = await _factory.Context.Transactions.ToListAsync();

                e.Result.Errors[nameof(PaymentTransaction.Amount)].First().Should().Be("Can't parse <Amount></Amount> element. Line 6");
                e.Result.Errors[nameof(PaymentTransaction.Currency)].First().Should()
                    .Be("Value USFD has the wrong currency format. Line 11");
            }
        }
    }
}