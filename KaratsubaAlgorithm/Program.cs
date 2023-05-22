var x = new BigInteger("1313234242425");
var y = new BigInteger("1234556789");
//var x = new BigInteger("123456");
//var y = new BigInteger("1234");
var result = x.Add(y);
Console.WriteLine(result);
public class BigInteger
{
    private int[] _numbers;
    private string _snumbers;
    private bool _isPositive;
    public BigInteger()
    {
        _numbers = new int[1];
        _snumbers = "0";
        _isPositive = true;
    }

    public BigInteger(string str)
    {
        _isPositive = (str[0] != '-');
        int dim = (_isPositive) ? str.Length : str.Length - 1;
        _numbers = new int[dim];
        _snumbers = "";
        for (int i = 0; i < dim; i++)
        {
            _numbers[dim - 1 - i] = int.Parse(str[i].ToString());
            _snumbers += str[dim - i - 1];
        }
    }

    public override string ToString()
    {
        string res = "";
        if (!_isPositive) res += "-";
        for (int i = 0; i < _snumbers.Length; i++)
        {
            res += _snumbers[_snumbers.Length - 1 - i];
        }
        return res;
    }
    public BigInteger Add(BigInteger another)
    {
        int[] firstNumber = _numbers;
        int[] secondNumber = another._numbers;
        int[] tmpResult = new int[Math.Max(firstNumber.Length, secondNumber.Length)];
        BigInteger result = new BigInteger();
        int remember = 0;

        if (_isPositive && !another._isPositive)
        {
            return null; // this.Subtraction 
        }
        else if (another._isPositive && !_isPositive) 
        {
            return null; // another.Subtraction
        }
        else
        {
            for (int i = 0; i < tmpResult.Length; i++)
            {
                int sum = remember;
                sum += i < firstNumber.Length ? firstNumber[i] : 0;
                sum += i < secondNumber.Length ? secondNumber[i] : 0;
                tmpResult[i] = sum % 10;
                remember = sum / 10;
            }
            result = new BigInteger(string.Join("", tmpResult.Reverse()));
            result._isPositive = !another._isPositive && !_isPositive ? false : true;
            return result;
        }
    }
}
