using NUnit.Framework;
using System.Collections.Generic;

namespace Hollibaugh_WorkSample.Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestKEYS()
        {
            MultiValueDictionary dict = new MultiValueDictionary();
            dict.HandleInput("ADD foo bar");
            dict.HandleInput("ADD baz bang");
            string result = dict.HandleInput("KEYS").Replace("\n", "").Replace("\r", "");
            string expected = "foobaz";
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void TestMembers()
        {
            MultiValueDictionary dict = new MultiValueDictionary();
            List<string> expected = new List<string>();
            List<string> results = new List<string>();

            results.Add(dict.HandleInput("ADD foo bar"));
            expected.Add("Added");

            results.Add(dict.HandleInput("ADD foo baz"));
            expected.Add("Added");

            results.Add(dict.HandleInput("Members foo"));
            expected.Add("barbaz");

            results.Add(dict.HandleInput("Members bar"));
            expected.Add("ERROR, key does not exist.");

            StringListsEqual(expected, results);
        }

        [Test]
        public void TestADD()
        {
            MultiValueDictionary dict = new MultiValueDictionary();
            List<string> expected = new List<string>();
            List<string> results = new List<string>();

            results.Add(dict.HandleInput("ADD foo bar"));
            expected.Add("Added");

            results.Add(dict.HandleInput("ADD foo baz"));
            expected.Add("Added");

            results.Add(dict.HandleInput("ADD foo bar"));
            expected.Add("ERROR, member already exists for key");

            StringListsEqual(expected, results);

        }


        [Test]
        public void TestREMOVE()
        {
            MultiValueDictionary dict = new MultiValueDictionary();
            List<string> expected = new List<string>();
            List<string> results = new List<string>();

            results.Add(dict.HandleInput("ADD foo bar"));
            expected.Add("Added");

            results.Add(dict.HandleInput("ADD foo baz"));
            expected.Add("Added");

            results.Add(dict.HandleInput("REMOVE foo bar"));
            expected.Add("Removed");

            results.Add(dict.HandleInput("REMOVE foo bar"));
            expected.Add("ERROR, member does not exist");


            results.Add(dict.HandleInput("KEYS"));
            expected.Add("foo");

            results.Add(dict.HandleInput("REMOVE foo baz"));
            expected.Add("Removed");

            results.Add(dict.HandleInput("KEYS"));
            expected.Add("empty set");

            results.Add(dict.HandleInput("REMOVE boom pow"));
            expected.Add("ERROR, key does not exist");

            StringListsEqual(expected, results);
        }

        [Test]
        public void TestREMOVEALL()
        {
            MultiValueDictionary dict = new MultiValueDictionary();
            List<string> expected = new List<string>();
            List<string> results = new List<string>();

            results.Add(dict.HandleInput("ADD foo bar"));
            expected.Add("Added");

            results.Add(dict.HandleInput("ADD foo baz"));
            expected.Add("Added");

            results.Add(dict.HandleInput("KEYS"));
            expected.Add("foo");

            results.Add(dict.HandleInput("REMOVEALL foo"));
            expected.Add("Removed");

            results.Add(dict.HandleInput("KEYS"));
            expected.Add("empty set");

            results.Add(dict.HandleInput("REMOVEALL foo"));
            expected.Add("ERROR, key does not exist");

            StringListsEqual(expected, results);
        }

        [Test]
        public void TestCLEAR()
        {
            MultiValueDictionary dict = new MultiValueDictionary();
            List<string> expected = new List<string>();
            List<string> results = new List<string>();

            results.Add(dict.HandleInput("ADD foo bar"));
            expected.Add("Added");

            results.Add(dict.HandleInput("ADD bang zip"));
            expected.Add("Added");

            results.Add(dict.HandleInput("KEYS"));
            expected.Add("foobang");

            results.Add(dict.HandleInput("CLEAR"));
            expected.Add("Cleared");

            results.Add(dict.HandleInput("KEYS"));
            expected.Add("empty set");

            results.Add(dict.HandleInput("CLEAR"));
            expected.Add("Cleared");

            results.Add(dict.HandleInput("KEYS"));
            expected.Add("empty set");

            StringListsEqual(expected, results);
        }

        [Test]
        public void TestKEYEXISTS()
        {
            MultiValueDictionary dict = new MultiValueDictionary();
            List<string> expected = new List<string>();
            List<string> results = new List<string>();

            results.Add(dict.HandleInput("KEYEXISTS foo"));
            expected.Add("False");

            results.Add(dict.HandleInput("ADD foo bar"));
            expected.Add("Added");

            results.Add(dict.HandleInput("KEYEXISTS foo"));
            expected.Add("True");

            StringListsEqual(expected, results);
        }

        [Test]
        public void TestMEMBEREXISTS()
        {
            MultiValueDictionary dict = new MultiValueDictionary();
            List<string> expected = new List<string>();
            List<string> results = new List<string>();

            results.Add(dict.HandleInput("MEMBEREXISTS foo bar"));
            expected.Add("False");

            results.Add(dict.HandleInput("ADD foo bar"));
            expected.Add("Added");

            results.Add(dict.HandleInput("MEMBEREXISTS foo bar"));
            expected.Add("True");

            results.Add(dict.HandleInput("MEMBEREXISTS foo baz"));
            expected.Add("False");

            StringListsEqual(expected, results);
        }

        [Test]
        public void TestALLMEMBERS()
        {
            MultiValueDictionary dict = new MultiValueDictionary();
            List<string> expected = new List<string>();
            List<string> results = new List<string>();

            results.Add(dict.HandleInput("ALLMEMBERS"));
            expected.Add("empty set");

            results.Add(dict.HandleInput("ADD foo bar"));
            expected.Add("Added");

            results.Add(dict.HandleInput("ADD foo baz"));
            expected.Add("Added");

            results.Add(dict.HandleInput("ALLMEMBERS"));
            expected.Add("barbaz");

            results.Add(dict.HandleInput("ADD bang bar"));
            expected.Add("Added");

            results.Add(dict.HandleInput("ADD bang baz"));
            expected.Add("Added");

            results.Add(dict.HandleInput("ALLMEMBERS"));
            expected.Add("barbazbarbaz");

            StringListsEqual(expected, results);
        }

        [Test]
        public void TestITEMS()
        {
            MultiValueDictionary dict = new MultiValueDictionary();
            List<string> expected = new List<string>();
            List<string> results = new List<string>();

            results.Add(dict.HandleInput("ITEMS"));
            expected.Add("empty set");

            results.Add(dict.HandleInput("ADD foo bar"));
            expected.Add("Added");

            results.Add(dict.HandleInput("ADD foo baz"));
            expected.Add("Added");

            results.Add(dict.HandleInput("ITEMS"));
            expected.Add("foo: barfoo: baz");

            results.Add(dict.HandleInput("ADD bang bar"));
            expected.Add("Added");

            results.Add(dict.HandleInput("ADD bang baz"));
            expected.Add("Added");

            results.Add(dict.HandleInput("ITEMS"));
            expected.Add("foo: barfoo: bazbang: barbang: baz");

            StringListsEqual(expected, results);
        }

        void StringListsEqual(List<string> lstOne, List<string> lstTwo)
        {
            for (int i = 0; i < lstOne.Count; i++)
            {
                if (lstOne[i].Replace("\n", "").Replace("\r", "") != lstTwo[i].Replace("\n", "").Replace("\r", ""))
                {
                    Assert.Fail("Expected: " + lstOne[i] + "\nBut was: " + lstTwo[i]);
                    return;
                }
            }
            Assert.Pass();
        }
    }
}