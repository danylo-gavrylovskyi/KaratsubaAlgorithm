//var x = new BigInteger("1313234242425");
//var y = new BigInteger("1234556789");
var x = new BigInteger("12345");
var y = new BigInteger("1234");
//var result = x.Add(y);
//var result = x.Substraction(y);
BigInteger result = x * y;
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
        if (!_isPositive)
        {
            str = str.TrimStart('-');
        }
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
        res = res[0] == '0' ? res.TrimStart('0') : res;
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
            another._isPositive = true;
            return Substraction(another);
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
            result._isPositive = !another._isPositive && !_isPositive ? false : _isPositive;
            return result;
        }
    }

    public BigInteger Substraction(BigInteger another)
    {
        int[] firstNumber = _numbers;
        int[] secondNumber = another._numbers;
        int[] tmpResult = new int[Math.Max(firstNumber.Length, secondNumber.Length)];
        BigInteger result = new BigInteger();
        int remember = 0;

        if (_isPositive && !another._isPositive)
        {
            another._isPositive = true;
            return Add(another);
        }
        else if (!_isPositive && another._isPositive)
        {
            return Add(another);
        }
        else
        {
            for (int i = 0; i < tmpResult.Length; i++)
            {
                int sum = remember;
                sum += i < firstNumber.Length ? firstNumber[i] : 0;
                sum -= i < secondNumber.Length ? secondNumber[i] : 0;
                tmpResult[i] = (10 + sum) % 10;
                remember = sum < 0 ? -1 : 0;
            }
            result = new BigInteger(string.Join("", tmpResult.Reverse()));
            result._isPositive = !another._isPositive && !_isPositive ? false : _isPositive;
            return result;
        }
    }

    private BigInteger ShiftLeft(int length)
    {
        BigInteger result = new BigInteger();
        result._numbers = new int[_numbers.Length + length];
        Array.Copy(_numbers, 0, result._numbers, length, _numbers.Length);
        result._snumbers = string.Join("", result._numbers.Reverse());
        return result;
    }

    private BigInteger AddZeros(BigInteger number, int amount)
    {
        int[] result = new int[number._numbers.Length + amount];
        for (int i = amount; i < result.Length; i++)
        {
            result[i] = number._numbers[i - amount];
        }
        number._numbers = result;
        number._snumbers = string.Join("", result);
        return number;
    }

    public string GetSecondHalf(int halfLength)
    {
        string result = "";
        string finalResult = "";

        result = string.Join("", _numbers).Substring(0, halfLength);

        if (result == "") return "0";
        for (int i = result.Length - 1; i >= 0; i--)
        {
            finalResult += result[i];
        }
        return finalResult;
    }
    public string GetFirstHalf(int halfLength)
    {
        string result = "";
        string finalResult = "";

        for (int i = 0; i < _numbers.Length; i++)
        {
            result += _numbers[i].ToString();
        }

        result = string.Join("", _numbers).Substring(halfLength);

        if (result == "") return "0";
        for (int i = result.Length - 1; i >= 0; i--)
        {
            finalResult += result[i];
        }
        return finalResult;
    }
    public BigInteger Karatsuba(BigInteger another)
    {
        int[] firstNumber = _numbers;
        int[] secondNumber = another._numbers;
        string firstNumStr = string.Join("", _numbers);
        string secondNumStr = string.Join("", another._numbers);

        int maxLength = Math.Max(_numbers.Length, another._numbers.Length);
        if (_numbers.Length > another._numbers.Length)
        {
            Array.Resize(ref another._numbers, maxLength);
        }
        else if (_numbers.Length < another._numbers.Length)
        {
            Array.Resize(ref _numbers, maxLength);
        }

        if (maxLength == 1)
        {
            int result = int.Parse(firstNumStr) * int.Parse(secondNumStr);
            return new BigInteger(result.ToString());
        }

        int halfLength = maxLength / 2;
        var firstNumFirstHalf = new BigInteger(GetFirstHalf(halfLength));
        var firstNumSecondHalf = new BigInteger(GetSecondHalf(halfLength));
        var secondNumFirstHalf = new BigInteger(another.GetFirstHalf(halfLength));
        var secondNumSecondHalf = new BigInteger(another.GetSecondHalf(halfLength));

        BigInteger resultOfFirstHalfs = firstNumFirstHalf * secondNumFirstHalf;
        BigInteger resultOfSecondHalfs = firstNumSecondHalf * secondNumSecondHalf;
        BigInteger resultOfAllHalfs = (firstNumFirstHalf + firstNumSecondHalf) * (secondNumFirstHalf + secondNumSecondHalf) - resultOfSecondHalfs - resultOfFirstHalfs;

        BigInteger finalResult = AddZeros(resultOfFirstHalfs, halfLength * 2) + AddZeros(resultOfAllHalfs, halfLength) + resultOfSecondHalfs;
        return finalResult;
    }

    public static BigInteger operator +(BigInteger firstNumber, BigInteger secondNumber) => firstNumber.Add(secondNumber);
    public static BigInteger operator -(BigInteger firstNumber, BigInteger secondNumber) => firstNumber.Substraction(secondNumber);
    public static BigInteger operator *(BigInteger firstNumber, BigInteger secondNumber) => firstNumber.Karatsuba(secondNumber);

    //public BigInteger Karatsuba(BigInteger another)
    //{
    //    int[] firstNumber = _numbers;
    //    int[] secondNumber = another._numbers;
    //    string firstNumStr = string.Join("", _numbers);
    //    string secondNumStr = string.Join("", another._numbers);

    //    int maxLength = Math.Max(_numbers.Length, another._numbers.Length);
    //    if (_numbers.Length > another._numbers.Length)
    //    {
    //        Array.Resize(ref another._numbers, maxLength);
    //    }
    //    else if (_numbers.Length < another._numbers.Length)
    //    {
    //        Array.Resize(ref _numbers, maxLength);
    //    }

    //    if (maxLength == 1)
    //    {
    //        int result = int.Parse(firstNumStr) * int.Parse(secondNumStr);
    //        return new BigInteger(result.ToString());
    //    }

    //    int halfLength = maxLength / 2;
    //    var firstNumFirstHalf = new BigInteger(string.Join("", firstNumber[halfLength..]));
    //    var firstNumSecondHalf = new BigInteger(string.Join("", firstNumber[0..halfLength]));
    //    var secondNumFirstHalf = new BigInteger(string.Join("", secondNumber[halfLength..]));
    //    var secondNumSecondHalf = new BigInteger(string.Join("", secondNumber[0..halfLength]));

    //    BigInteger resultOfFirstHalfs = firstNumFirstHalf.Karatsuba(secondNumFirstHalf);
    //    BigInteger resultOfSecondHalfs = firstNumSecondHalf.Karatsuba(secondNumSecondHalf);
    //    //BigInteger resultOfAllHalfs = (firstNumFirstHalf + firstNumSecondHalf).Karatsuba(secondNumFirstHalf + secondNumSecondHalf) - resultOfFirstHalfs - resultOfSecondHalfs;
    //    BigInteger resultOfAllHalfs = (firstNumFirstHalf + firstNumSecondHalf).Karatsuba(secondNumFirstHalf + secondNumSecondHalf);

    //    //BigInteger finalResult = AddZeros(resultOfFirstHalfs, halfLength * 2) + AddZeros(resultOfAllHalfs, halfLength) + resultOfSecondHalfs;
    //    BigInteger finalResult = AddZeros(resultOfSecondHalfs, halfLength * 2) + AddZeros(resultOfAllHalfs - resultOfSecondHalfs - resultOfFirstHalfs, halfLength) + resultOfFirstHalfs;
    //    return finalResult;
    //}

    //public BigInteger Karatsuba(BigInteger another)
    //{
    //    string firstNumber = string.Join("", _numbers.Reverse());
    //    string secondNumber = string.Join("", another._numbers.Reverse());

    //    int maxLength = Math.Max(firstNumber.Length, secondNumber.Length);
    //    firstNumber = firstNumber.PadLeft(maxLength, '0');
    //    secondNumber = secondNumber.PadLeft(maxLength, '0');

    //    if (maxLength == 1)
    //    {
    //        int result = int.Parse(firstNumber) * int.Parse(secondNumber);
    //        return new BigInteger(result.ToString());
    //    }

    //    int halfLength = maxLength / 2;
    //    string firstNumFirstHalf = firstNumber.Substring(halfLength);
    //    string firstNumSecondHalf = firstNumber.Substring(0, halfLength);
    //    string secondNumFirstHalf = secondNumber.Substring(halfLength);
    //    string secondNumSecondHalf = secondNumber.Substring(0, halfLength);

    //    BigInteger resultOfFirstHalfs = new BigInteger(firstNumFirstHalf).Karatsuba(new BigInteger(secondNumFirstHalf));
    //    BigInteger resultOfSecondHalfs = new BigInteger(firstNumSecondHalf).Karatsuba(new BigInteger(secondNumSecondHalf));
    //    BigInteger resultOfAllHalfs = ((new BigInteger(firstNumFirstHalf).Add
    //        (new BigInteger(firstNumSecondHalf))).Karatsuba
    //        (new BigInteger(secondNumFirstHalf).Add
    //        (new BigInteger(secondNumSecondHalf))).Substraction
    //        (resultOfFirstHalfs)).Substraction
    //        (resultOfSecondHalfs);


    //    BigInteger finalResult = resultOfFirstHalfs.ShiftLeft(2 * halfLength).Add(resultOfAllHalfs.ShiftLeft(halfLength)).Add(resultOfSecondHalfs);

    //    return finalResult;
    //}

}
