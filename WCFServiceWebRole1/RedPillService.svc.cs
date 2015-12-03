using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Threading.Tasks;

namespace WCFServiceWebRole1
{    
    [ServiceBehavior(Namespace = "http://KnockKnock.readify.net", ConfigurationName = "IRedPill", InstanceContextMode = InstanceContextMode.PerSession)]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class RedPillService : IRedPill
    {
        private Dictionary<long, long> FibList = new Dictionary<long, long>();
        private static int Max = 92;

        /// <summary>
        /// Constructor to precalculate all Fibonacci numbers and store in the list.
        /// </summary>
        public RedPillService()
        {
            for (long i = -Max; i <= Max; i++)
            {
                FibList.Add(i, FibonacciNumber(i));
            }
        }

        #region Fibonacci number calculation
        /// <summary>
        /// calculate the Fibonacci number
        /// </summary>
        /// <param name="n">Input number</param>
        /// <returns>Calculated Fibonacci number between -92 and 92</returns>
        private long FibonacciNumber(long n)
        {
            if (n > Max || n < -Max)
            {
                throw new ArgumentException("Fib number beyond range.");
            }
            else
            {
                long a = 0;
                long b = 1;
                long result = 0;
                if (n == 0)
                {
                    result = 0;
                }
                else if (n > 0)
                {
                    result = FibonacciPositive(n, a, b);
                }
                else // in case of the input n is minus value
                {
                    result = FibonacciPositive(n * -1, a, b);

                    //shift the positive (n*-1) 1 bit right, then 1 bit left, to check is odd or even
                    //if the n is even, return the minus value.
                    if ((((n * -1) >> 1) << 1) == (n * -1))
                    {
                        result = result * -1;
                    }
                }

                return result;
            }
        }

        /// <summary>
        /// Calculate the Fibonacci number 
        /// </summary>
        /// <param name="n">The input argument n</param>
        /// <param name="a">The seed value for F(0)=0</param>
        /// <param name="b">The seed value for F(1)=1</param>
        /// <returns></returns>
        private long FibonacciPositive(long n, long a, long b)
        {
            long data = a;
            for (int i = 0; i < n; i++)
            {
                long temp = data;
                data = b;
                b = temp + b;
            }
            return data;
        }

        /// <summary>
        /// The service operation to calculate Fibonacci number using task based async method.
        /// </summary>
        /// <param name="n">The input argument n</param>
        /// <returns>Calculated Fibonacci number</returns>
        public Task<long> FibonacciNumberAsync(long n)
        {
            return Task.Run(() =>
            {
                if (n > Max || n < -Max)
                {
                    throw new ArgumentException("Fib number beyond range.");
                }
                else
                {
                    return FibList[n];
                }
            });
        }
        #endregion Fibonacci number calculation

        #region Reverse word
        private string ReverseWords(string s)
        {            
            if (s!=null)
            {
                char[] sArray = s.ToCharArray();
                int length = s.Length - 1;

                //xor bit operator for faster revert
                for (int i = 0; i < length; i++, length--)
                {
                    sArray[i] ^= sArray[length];
                    sArray[length] ^= sArray[i];
                    sArray[i] ^= sArray[length];
                }
                return new string(sArray);
            }
            else
                throw new ArgumentNullException("string is null.");
        
        }

        /// <summary>
        /// The service operation to reverse all words. 
        /// </summary>
        /// <param name="s">Input string</param>
        /// <returns>All reversed string</returns>
        public Task<string> ReverseWordsAsync(string s)
        {
            return Task.Run(() =>
            {
                //Consider the string split by space, each word need to be reverted seperatly and combined together
                var rtnStr = from word in s.Split(' ') select ReverseWords(word);

                return string.Join(" ", rtnStr.ToList());               
            });
        }

        #endregion Reverse word

        #region Output the unique Token
        private Guid WhatIsYourToken()
        {            
            return Guid.Parse("9dcfb858-8563-4468-98be-63ad8094e530");
        }

        public Task<Guid> WhatIsYourTokenAsync()
        {
            return Task.Run(() =>
            {
                return WhatIsYourToken();
            });
        }
        #endregion Output the unique Token

        #region Decide what the triangle shape is
        private TriangleType WhatShapeIsThis(int a, int b, int c)                    
        {
            if (a <= 0 || b <= 0 || c <= 0 || ((b + c) <= a) || ((a + c) <= b) || ((a + b) <= c))
            {
                return TriangleType.Error;
            }
            else
            {
                // hashset ignores the int value already exists in the set.
                HashSet<int> lines = new HashSet<int>();
                lines.Add(a);
                lines.Add(b);
                lines.Add(c);

                return lines.Count == 1 ? TriangleType.Equilateral : lines.Count == 2 ? TriangleType.Isosceles : TriangleType.Scalene;
            }                     
        }

        /// <summary>
        /// The Service operation to decide which triangle shape is based on the input 3 values.
        /// </summary>
        /// <param name="a">The triangle first line length</param>
        /// <param name="b">The triangle second line length</param>
        /// <param name="c">The triangle third line length</param>
        /// <returns></returns>
        public Task<TriangleType> WhatShapeIsThisAsync(int a, int b, int c)
        {
            return Task.Run(() =>
            {
                return WhatShapeIsThis(a, b, c);
            });
        }
        #endregion Decide what the triangle shape is
    }
}
