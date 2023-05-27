//var x = new BigInteger("1313234242425");
//var y = new BigInteger("1234556789");
var x = new BigInteger("-12345");
var y = new BigInteger("1234");
BigInteger result = x * y;
Console.WriteLine(result);
int[] array = { 10, 80, 30, 90, 40, 50, 70 };
QuickSort(array, 0, array.Length - 1);
Console.WriteLine(string.Join(" ", array));

void QuickSort(int[] array, int startIndex, int endingIndex)
{
    if (startIndex < endingIndex)
    {
        int partitionIndex = Partition(array, startIndex, endingIndex);
        QuickSort(array, startIndex, partitionIndex - 1);
        QuickSort(array, partitionIndex + 1, endingIndex);
    }
}

void Swap(ref int firstNum, ref int secondNum)
{
    int tmp = firstNum;
    firstNum = secondNum;
    secondNum = tmp;
}

int Partition(int[] array, int startIndex, int endingIndex)
{
    int topNumber = array[endingIndex];
    int index = startIndex - 1;

    for (int i = startIndex; i <= endingIndex - 1; i++)
    {
        if (array[i] < topNumber)
        {
            index++;
            Swap(ref array[index], ref array[i]);
        }
    }

    Swap(ref array[index + 1], ref array[endingIndex]);
    return index + 1;
}

public class BigInteger
{
    private int[] _numbers;
    private string _snumbers;
    private bool _isPositive;
    private bool multiplyIsPositive;
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
        if (!_isPositive || !multiplyIsPositive) res += "-";
        for (int i = 0; i < _snumbers.Length; i++)
        {
            res += _snumbers[_snumbers.Length - 1 - i];
        }
        res = res[0] == '-' && res[1] == '0' ? "-" + res.TrimStart('-').TrimStart('0') : res;
        res = res[0] == '0' || res[1] == '0' ? res.TrimStart('0') : res;
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
        else if (!_isPositive && another._isPositive)
        {
            _isPositive = false;
            another._isPositive = false;
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

    private string GetSecondHalf(int halfLength)
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
    private string GetFirstHalf(int halfLength)
    {
        string result = "";
        string finalResult = "";

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
        string firstNumStr = string.Join("", _numbers);
        string secondNumStr = string.Join("", another._numbers);

        if ((!_isPositive && another._isPositive) || (_isPositive && !another._isPositive))
        {
            multiplyIsPositive = false;
        }
        else if (!_isPositive && !another._isPositive)
        {
            multiplyIsPositive = true;
        }

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

}
