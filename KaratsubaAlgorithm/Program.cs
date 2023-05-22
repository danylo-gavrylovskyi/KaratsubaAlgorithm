var x = new BigInteger("1313234242425");
Console.WriteLine(x);
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
    //public BigInteger Add(BigInteger another)

}
