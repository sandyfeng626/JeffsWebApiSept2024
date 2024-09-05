namespace RecordsAndStuff;

public class UnitTest1
{
    [Fact]
    public void Test1()
    {
        var account = new BankAccount();

        var statement = account.GetCurrentBalance();

        Assert.Equal(0, statement.Balance);


        account.Deposit(500);

        statement = account.GetCurrentBalance();
        //statement.Balance += 800;
        Assert.Equal(500, statement.Balance);

        //var updatedStatement = statement with { IsOverdraft = true };
        //Assert.Equal(500, updatedStatement.Balance);
        //Assert.True(updatedStatement.IsOverdraft);
        //Assert.False(statement.IsOverdraft);

        var expectedStatement = new AccountBalance { Balance = 500, IsOverdraft = false };

        Assert.Equal(expectedStatement, statement);
        //Assert.Equal(expectedStatement.Balance, statement.Balance);  
        //Assert.Equal(expectedStatement.IsOverdraft, statement.IsOverdraft);

    }

    [Fact]
    public void ConstructorVersionOfRecord()
    {
        var myStatement = new Statement(300.23M, false);

        Assert.Equal(300.23M, myStatement.Balance);

        myStatement.Balance = 10000000;
    }
}

public record AccountBalance
{

    public required decimal Balance { get; init; }
    public bool IsOverdraft { get; init; }
}

public record Statement(decimal Balance, bool IsOverdraft);

public class BankAccount
{
    private decimal _balance = 0;
    public void Deposit(decimal amount)
    {
        _balance += amount;
    }
    public void Withdraw(decimal amount)
    {
        _balance -= amount;
    }
    public AccountBalance GetCurrentBalance()
    {
        return new AccountBalance
        {
            Balance = _balance,
            IsOverdraft = false
        }; // exit the initializer

    }
}