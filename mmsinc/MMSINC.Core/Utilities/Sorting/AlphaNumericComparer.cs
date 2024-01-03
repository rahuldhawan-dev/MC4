using System.Collections;
using System.Collections.Generic;
using System.Globalization;

namespace MMSINC.Utilities.Sorting
{
    public class AlphaNumericComparer : IComparer<string>, IComparer
    {
        private StringParser _parser1;
        private StringParser _parser2;
        private NaturalComparerOptions _naturalComparerOptions;

        private enum TokenType
        {
            Nothing,
            Numerical,
            String
        }

        private class StringParser
        {
            private TokenType _tokenType;
            private string _stringValue;
            private decimal _numericalValue;
            private int _idx, _len;
            private string _source;
            private char _curChar;
            private AlphaNumericComparer _alphaNumericComparer;

            public StringParser(AlphaNumericComparer alphaNumericComparer)
            {
                _alphaNumericComparer = alphaNumericComparer;
            }

            public void Init(string source)
            {
                if (source == null)
                    source = string.Empty;
                _source = source;
                _len = source.Length;
                _idx = -1;
                _numericalValue = 0;
                NextChar();
                NextToken();
            }

            public TokenType TokenType => _tokenType;

            public decimal NumericalValue
            {
                get
                {
                    if (_tokenType == AlphaNumericComparer.TokenType.Numerical)
                    {
                        return _numericalValue;
                    }

                    throw new NaturalComparerException(
                        "Internal Error: NumericalValue called on a non numerical value.");
                }
            }

            public string StringValue => _stringValue;

            public void NextToken()
            {
                do
                {
                    //CharUnicodeInfo.GetUnicodeCategory 
                    if (_curChar == '\0')
                    {
                        _tokenType = AlphaNumericComparer.TokenType.Nothing;
                        _stringValue = null;
                        return;
                    }
                    else if (char.IsDigit(_curChar))
                    {
                        ParseNumericalValue();
                        return;
                    }
                    else if (char.IsLetter(_curChar))
                    {
                        ParseString();
                        return;
                    }
                    else
                    {
                        //ignore this character and loop some more 
                        NextChar();
                    }
                } while (true);
            }

            private void NextChar()
            {
                _idx += 1;
                _curChar = _idx >= _len ? '\0' : _source[_idx];
            }

            private void ParseNumericalValue()
            {
                var start = _idx;
                var NumberDecimalSeparator = NumberFormatInfo.CurrentInfo.NumberDecimalSeparator[0];
                var NumberGroupSeparator = NumberFormatInfo.CurrentInfo.NumberGroupSeparator[0];
                do
                {
                    NextChar();
                    if (_curChar == NumberDecimalSeparator)
                    {
                        // parse digits after the Decimal Separator 
                        do
                        {
                            NextChar();
                            if (!char.IsDigit(_curChar) && _curChar != NumberGroupSeparator)
                                break;
                        } while (true);

                        break;
                    }
                    else
                    {
                        if (!char.IsDigit(_curChar) && _curChar != NumberGroupSeparator)
                            break;
                    }
                } while (true);

                _stringValue = _source.Substring(start, _idx - start);
                _tokenType = decimal.TryParse(_stringValue, out _numericalValue)
                    ? AlphaNumericComparer.TokenType.Numerical
                    : AlphaNumericComparer.TokenType.String;
            }

            private void ParseString()
            {
                var start = _idx;
                var roman = (_alphaNumericComparer._naturalComparerOptions & NaturalComparerOptions.RomanNumbers) != 0;
                var romanValue = 0;
                var lastRoman = int.MaxValue;
                var cptLastRoman = 0;
                do
                {
                    if (roman)
                    {
                        var thisRomanValue = RomanLetterValue(_curChar);
                        if (thisRomanValue > 0)
                        {
                            bool handled = false;

                            if ((thisRomanValue == 1 || thisRomanValue == 10 || thisRomanValue == 100))
                            {
                                NextChar();
                                var nextRomanValue = RomanLetterValue(_curChar);
                                if (nextRomanValue == thisRomanValue * 10 || nextRomanValue == thisRomanValue * 5)
                                {
                                    handled = true;
                                    if (nextRomanValue <= lastRoman)
                                    {
                                        romanValue += nextRomanValue - thisRomanValue;
                                        NextChar();
                                        lastRoman = thisRomanValue / 10;
                                        cptLastRoman = 0;
                                    }
                                    else
                                    {
                                        roman = false;
                                    }
                                }
                            }
                            else
                            {
                                NextChar();
                            }

                            if (!handled)
                            {
                                if (thisRomanValue <= lastRoman)
                                {
                                    romanValue += thisRomanValue;
                                    if (lastRoman == thisRomanValue)
                                    {
                                        cptLastRoman += 1;
                                        switch (thisRomanValue)
                                        {
                                            case 1:
                                            case 10:
                                            case 100:
                                                if (cptLastRoman > 4)
                                                    roman = false;

                                                break;
                                            case 5:
                                            case 50:
                                            case 500:
                                                if (cptLastRoman > 1)
                                                    roman = false;

                                                break;
                                        }
                                    }
                                    else
                                    {
                                        lastRoman = thisRomanValue;
                                        cptLastRoman = 1;
                                    }
                                }
                                else
                                {
                                    roman = false;
                                }
                            }
                        }
                        else
                        {
                            roman = false;
                        }
                    }
                    else
                    {
                        NextChar();
                    }

                    if (!char.IsLetter(_curChar)) break;
                } while (true);

                _stringValue = _source.Substring(start, _idx - start);
                if (roman)
                {
                    _numericalValue = romanValue;
                    _tokenType = AlphaNumericComparer.TokenType.Numerical;
                }
                else
                {
                    _tokenType = AlphaNumericComparer.TokenType.String;
                }
            }
        }

        public AlphaNumericComparer(NaturalComparerOptions NaturalComparerOptions)
        {
            _naturalComparerOptions = NaturalComparerOptions;
            _parser1 = new StringParser(this);
            _parser2 = new StringParser(this);
        }

        public AlphaNumericComparer() : this(NaturalComparerOptions.Default) { }

        int IComparer<string>.Compare(string string1, string string2)
        {
            _parser1.Init(string1);
            _parser2.Init(string2);
            int result;
            do
            {
                if (_parser1.TokenType == TokenType.Numerical && _parser2.TokenType == TokenType.Numerical)
                {
                    // both string1 and string2 are numerical 
                    result = decimal.Compare(_parser1.NumericalValue, _parser2.NumericalValue);
                }
                else
                {
                    result = string.Compare(_parser1.StringValue, _parser2.StringValue);
                }

                if (result != 0)
                {
                    return result;
                }
                else
                {
                    _parser1.NextToken();
                    _parser2.NextToken();
                }
            } while (!(_parser1.TokenType == TokenType.Nothing && _parser2.TokenType == TokenType.Nothing));

            //identical 
            return 0;
        }

        private static int RomanLetterValue(char c)
        {
            switch (c)
            {
                case 'I':
                    return 1;
                case 'V':
                    return 5;
                case 'X':
                    return 10;
                case 'L':
                    return 50;
                case 'C':
                    return 100;
                case 'D':
                    return 500;
                case 'M':
                    return 1000;
                default:
                    return 0;
            }
        }

        int IComparer.Compare(object x, object y)
        {
            return ((IComparer<string>)this).Compare((string)x, (string)y);
        }
    }

    public class NaturalComparerException : System.Exception
    {
        public NaturalComparerException(string msg) : base(msg) { }
    }

    [System.Flags]
    public enum NaturalComparerOptions
    {
        None,
        RomanNumbers,

        //DecimalValues <- we could put this as an option 
        //IgnoreSpaces <- we could put this as an option 
        //IgnorePunctuation <- we could put this as an option 
        Default = None
    }
}
