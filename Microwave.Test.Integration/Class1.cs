using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Microwave.Test.Integration
{
    [TestFixture]
    public class Class1
    {
        [Test]
        public void Press_NoSubscribers_NoThrow()
        {
            int var = 1;
        }
    }
}
