﻿using ByteBee.Framework.Converting;
using ByteBee.Framework.Converting.Abstractions;
using NUnit.Framework;

namespace ByteBee.Framework.Tests.Converting.Standard.Int16CastingTests
{
    [TestFixture]
    public sealed partial class Int16CastingTest
    {
        private ITypeConverter<short> _converter;

        [SetUp]
        public void Setup()
        {
            _converter = new StandardConverterFactory().Create<short>();
        }

        [TearDown]
        public void TearDown()
        {
        }
    }
}