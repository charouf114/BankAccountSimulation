using Application.Authentification.Commands.Authenticate;
using Domain.Entities;
using Domain.Enum;
using Newtonsoft.Json;
using System.Net.Http.Json;
using System.Security.Cryptography;
using System.Text;

namespace IntegrationTests
{
    public class BankAccountTests : BaseTestFixture
    {
        [Test]
        public async Task BankAccountOperations()
        {
            // Scenario
            // 01 -  Create An Account + Card from DB Directly 
            // 01.1- Try To authenticate With Wrong Credential
            // 01.2- Try To authenticate With right Credential and get Token
            // 02 -  Deposit Money (100 EUR)
            // 03 -  Try to WithDrawal with Amount > Balance => Failed (150 EUR)
            // 04 - Deposit Money (100 EUR)
            // 05 - Try to WithDrawal with Amount < Balance => Success (150 EUR)
            // 06 - Try to WithDrawal with Amount > Balance => Failed  (150 EUR)
            // 07 - Try to WithDrawal with Amount < Balance => Success (30 EUR)
            // 08 - Get Card History (Balance = 20 & 6 transactions)
            // 09 - Update The Card To Be Expired
            // 10 - Try Deposit => Failed 
            // 11 -Try Withdrawal => Failed
            // 12 - Try Get Card History => Failed
            // 13- Try To re-authenticate With right Credential and expired card

            // 01 -  Create An Account + Card from DB Directly 
            Account account;
            Card card;
            using (var hmac = new HMACSHA512())
            {
                account = new Account()
                {
                    Id = Guid.NewGuid(),
                    Owner = "Achref Ben chaaben",
                    IBAN = "Fake IBAN",
                    RIB = "Fake RIB",
                    Balance = 0m
                };

                card = new Card
                {
                    Id = Guid.NewGuid(),
                    AccountId = account.Id,
                    Owner = "Achref Ben chaaben",
                    CardNumber = "1111111111111111",
                    CardStatus = CardState.Enabled,
                    ExpirationDate = new DateTime(2027, 03, 12),
                    SecretCode = hmac.ComputeHash(Encoding.UTF8.GetBytes("1111")),
                    Salt = hmac.Key,
                };

                datalayer.Accounts.Add(account);
                datalayer.Cards.Add(card);
                datalayer.SaveChanges();
            }
            // 01.1- Try To authenticate With Wrong Credential
            var result = await httpClient.Authenticate(new AuthenticateCommand()
            {
                CardNumber = "1111111111111111",
                SecretCode = "2222"
            });
            var authenticateResponse = await result.Content.ReadFromJsonAsync<AuthenticateResponse>();

            Assert.IsFalse(authenticateResponse.Success);
            Assert.AreEqual("Wrong Code", authenticateResponse.Message);

            // 01.2- Try To authenticate With right Credential and get Token
            result = await httpClient.Authenticate(new AuthenticateCommand()
            {
                CardNumber = "1111111111111111",
                SecretCode = "1111"
            });

            authenticateResponse = await result.Content.ReadFromJsonAsync<AuthenticateResponse>();

            Assert.IsTrue(authenticateResponse.Success);
            Assert.IsNotEmpty(authenticateResponse.AccessToken);
            Assert.IsNotNull(authenticateResponse.AccessToken);

            var token = authenticateResponse.AccessToken;

            // 02 -  Deposit Money 
            result = await httpClient.DepositMoney(token, new Domain.Dtos.TransactionInput()
            {
                Amount = 100m,
                Currency = "EUR",
                CardId = card.Id,
            });

            var addTransactionResponse = await result.Content.ReadFromJsonAsync<AddTransactionResponse>();
            Assert.IsTrue(addTransactionResponse.Success);
            Assert.AreEqual("Transaction Added Sucessefully", addTransactionResponse.Message);

            // 03 -  Try to WithDrawal with Amount > Balance => Failed
            result = await httpClient.WithDrawalMoney(token, new Domain.Dtos.TransactionInput()
            {
                Amount = 150m,
                Currency = "EUR",
                CardId = card.Id,
            });

            addTransactionResponse = await result.Content.ReadFromJsonAsync<AddTransactionResponse>();
            Assert.IsFalse(addTransactionResponse.Success);
            Assert.AreEqual("Transaction Rejected", addTransactionResponse.Message);

            // 04 - Deposit Money
            result = await httpClient.DepositMoney(token, new Domain.Dtos.TransactionInput()
            {
                Amount = 100m,
                Currency = "EUR",
                CardId = card.Id,
            });

            addTransactionResponse = await result.Content.ReadFromJsonAsync<AddTransactionResponse>();
            Assert.IsTrue(addTransactionResponse.Success);
            Assert.AreEqual("Transaction Added Sucessefully", addTransactionResponse.Message);

            // 05 - Try to WithDrawal with Amount < Balance => Success
            result = await httpClient.WithDrawalMoney(token, new Domain.Dtos.TransactionInput()
            {
                Amount = 150m,
                Currency = "EUR",
                CardId = card.Id,
            });

            addTransactionResponse = await result.Content.ReadFromJsonAsync<AddTransactionResponse>();
            Assert.IsTrue(addTransactionResponse.Success);
            Assert.AreEqual("Transaction Added Sucessefully", addTransactionResponse.Message);

            // 06 - Try to WithDrawal with Amount > Balance => Failed
            result = await httpClient.WithDrawalMoney(token, new Domain.Dtos.TransactionInput()
            {
                Amount = 150m,
                Currency = "EUR",
                CardId = card.Id,
            });

            addTransactionResponse = await result.Content.ReadFromJsonAsync<AddTransactionResponse>();
            Assert.IsFalse(addTransactionResponse.Success);
            Assert.AreEqual("Transaction Rejected", addTransactionResponse.Message);

            // 07 - Try to WithDrawal with Amount < Balance => Success
            result = await httpClient.WithDrawalMoney(token, new Domain.Dtos.TransactionInput()
            {
                Amount = 30m,
                Currency = "EUR",
                CardId = card.Id,
            });

            addTransactionResponse = await result.Content.ReadFromJsonAsync<AddTransactionResponse>();
            Assert.IsTrue(addTransactionResponse.Success);
            Assert.AreEqual("Transaction Added Sucessefully", addTransactionResponse.Message);

            // 08 - Get Card History
            result = await httpClient.GetHistory(token);
            var resultAsString = await result.Content.ReadAsStringAsync();

            var history = JsonConvert.DeserializeObject<CardHistoryResponse>(resultAsString);
            Assert.IsNotNull(history);

            Assert.IsTrue(history.Success);
            Assert.AreEqual(20m, history.Account.Balance);
            Assert.AreEqual(6, history.Transactions.Count);
            Assert.AreEqual(2, history.Transactions.Count(t => t.Status == TransactionState.Rejected));
            Assert.AreEqual(4, history.Transactions.Count(t => t.Status == TransactionState.Accepted));

            // 09 - Update The Card To Be Expired
            card.ExpirationDate = DateTime.UtcNow.AddDays(-1);
            datalayer.Cards.Update(card);
            datalayer.SaveChanges();

            // 10 - Try Deposit => Failed 
            result = await httpClient.WithDrawalMoney(token, new Domain.Dtos.TransactionInput()
            {
                Amount = 150m,
                Currency = "EUR",
                CardId = card.Id,
            });

            addTransactionResponse = await result.Content.ReadFromJsonAsync<AddTransactionResponse>();
            Assert.IsFalse(addTransactionResponse.Success);
            Assert.AreEqual("Card Not Enabled", addTransactionResponse.Message);

            // 11 -Try Withdrawal => Failed
            result = await httpClient.WithDrawalMoney(token, new Domain.Dtos.TransactionInput()
            {
                Amount = 150m,
                Currency = "EUR",
                CardId = card.Id,
            });

            addTransactionResponse = await result.Content.ReadFromJsonAsync<AddTransactionResponse>();
            Assert.IsFalse(addTransactionResponse.Success);
            Assert.AreEqual("Card Not Enabled", addTransactionResponse.Message);

            // 12 - Try Get Card History => Failed
            result = await httpClient.GetHistory(token);
            resultAsString = await result.Content.ReadAsStringAsync();

            history = JsonConvert.DeserializeObject<CardHistoryResponse>(resultAsString);
            Assert.IsNotNull(history);
            Assert.IsFalse(history.Success);
            Assert.AreEqual("Card Not Enabled", history.Message);

            // 13- Try To re-authenticate With right Credential and expired card
            result = await httpClient.Authenticate(new AuthenticateCommand()
            {
                CardNumber = "1111111111111111",
                SecretCode = "1111"
            });

            authenticateResponse = await result.Content.ReadFromJsonAsync<AuthenticateResponse>();

            Assert.IsFalse(authenticateResponse.Success);
            Assert.AreEqual("Card Not Enabled", authenticateResponse.Message);
        }
    }
}