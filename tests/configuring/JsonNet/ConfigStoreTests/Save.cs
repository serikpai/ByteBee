﻿using System;
using ByteBee.Framework.Configuring.Contract;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace ByteBee.Framework.Configuring.Tests.JsonNet.ConfigStoreTests
{
    public sealed partial class ConfigStoreTest
    {
        [Test]
        public void Save_SourceIsNull_ArgumentNullException()
        {
            IConfiguration source = null;

            Action act = () => _store.Save(source);

            act.Should()
                .ThrowExactly<ArgumentNullException>("source was null")
                .WithMessage("*source*");
        }

        [Test]
        public void Save_NoSections_EmptyResult()
        {
            string fileContent = "";
            WriteAllTextMock(c => fileContent = c);

            var source = new Mock<IConfiguration>();
            source.Setup(s => s.GetSections())
                .Returns(() => new string[0]);

            _store.Save(source.Object);

            fileContent.Should().Be("{}", "no sections are defined");
        }

        [Test]
        public void Save_OneSection_OneObject()
        {
            string fileContent = "";
            WriteAllTextMock(c => fileContent = c);

            var source = new Mock<IConfiguration>();
            source.Setup(s => s.GetSections())
                .Returns(() => new[] {"foo"});
            
            _store.Save(source.Object);

            Console.WriteLine(fileContent);

            fileContent.Should()
                .Be("{\"foo\":{}}", "one section is defined");
        }

        [Test]
        public void Save_TwoSections_TwoObjects()
        {
            string fileContent = "";
            WriteAllTextMock(c => fileContent = c);

            var source = new Mock<IConfiguration>();
            source.Setup(s => s.GetSections())
                .Returns(() => new[] {"foo", "bar"});
            
            _store.Save(source.Object);

            fileContent.Should()
                .Be("{\"foo\":{},\"bar\":{}}", "foo and bar sections were defined");
        }

        [Test]
        public void Save_OneSectionsOneNullKey_OneObjectsWithSubObject()
        {
            string fileContent = "";
            WriteAllTextMock(c => fileContent = c);

            var source = new Mock<IConfiguration>();
            source.Setup(s => s.GetSections())
                .Returns(() => new[] {"foo"});
            source.Setup(s => s.GetKeys("foo"))
                .Returns(() => new[] {"bar"});
            
            _store.Save(source.Object);

            fileContent.Should()
                .Be("{\"foo\":{\"bar\":null}}", "foo.bar was specified");
        }

        [Test]
        public void Save_OneSectionsOneStringKey_ValidJsonFormat()
        {
            string fileContent = "";
            WriteAllTextMock(c => fileContent = c);

            var source = new Mock<IConfiguration>();
            source.Setup(s => s.GetSections())
                .Returns(() => new[] {"foo"});
            source.Setup(s => s.GetKeys("foo"))
                .Returns(() => new[] {"bar"});
            source.Setup(s => s.Get<object>("foo", "bar"))
                .Returns(() => "foobar");

            _store.Save(source.Object);

            fileContent.Should()
                .Be("{\"foo\":{\"bar\":\"foobar\"}}", "foo.bar was a string");
        }

        [Test]
        public void Save_OneSectionsOneIntKey_ValidJsonFormat()
        {
            string fileContent = "";
            WriteAllTextMock(c => fileContent = c);

            var source = new Mock<IConfiguration>();
            source.Setup(s => s.GetSections())
                .Returns(() => new[] {"foo"});
            source.Setup(s => s.GetKeys("foo"))
                .Returns(() => new[] {"bar"});
            source.Setup(s => s.Get<object>("foo", "bar"))
                .Returns(() => 42);

            _store.Save(source.Object);

            fileContent.Should()
                .Be("{\"foo\":{\"bar\":42}}", "foo.bar was a string");
        }

        [Test]
        public void Save_OneSectionsOneObjectKey_ValidJsonFormat()
        {
            string fileContent = "";
            WriteAllTextMock(c => fileContent = c);

            var source = new Mock<IConfiguration>();
            source.Setup(s => s.GetSections())
                .Returns(() => new[] {"foo"});
            source.Setup(s => s.GetKeys("foo"))
                .Returns(() => new[] {"bar"});
            source.Setup(s => s.Get<object>("foo", "bar"))
                .Returns(() => new {foo = "bar"});

            _store.Save(source.Object);

            fileContent.Should()
                .Be("{\"foo\":{\"bar\":{\"foo\":\"bar\"}}}", "foo.bar was a string");
        }

        [Test]
        public void Save_OneSectionsOneArrayKey_ValidJsonFormat()
        {
            string fileContent = "";
            WriteAllTextMock(c => fileContent = c);

            var source = new Mock<IConfiguration>();
            source.Setup(s => s.GetSections())
                .Returns(() => new[] {"foo"});
            source.Setup(s => s.GetKeys("foo"))
                .Returns(() => new[] {"bar"});
            source.Setup(s => s.Get<object>("foo", "bar"))
                .Returns(() => new[] {1, 2, 3});

            _store.Save(source.Object);

            fileContent.Should()
                .Be("{\"foo\":{\"bar\":[1,2,3]}}", "foo.bar was a string");
        }
    }
}