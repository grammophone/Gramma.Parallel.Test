using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Gramma.Parallel;

namespace Gramma.Parallel.Test
{
	/// <summary>
	/// Summary description for UnitTest1
	/// </summary>
	[TestClass]
	public class UnitTest
	{
		public UnitTest()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		private TestContext testContextInstance;

		/// <summary>
		///Gets or sets the test context which provides
		///information about and functionality for the current test run.
		///</summary>
		public TestContext TestContext
		{
			get
			{
				return testContextInstance;
			}
			set
			{
				testContextInstance = value;
			}
		}

		#region Additional test attributes
		//
		// You can use the following additional attributes as you write your tests:
		//
		// Use ClassInitialize to run code before running the first test in the class
		// [ClassInitialize()]
		// public static void MyClassInitialize(TestContext testContext) { }
		//
		// Use ClassCleanup to run code after all tests in a class have run
		// [ClassCleanup()]
		// public static void MyClassCleanup() { }
		//
		// Use TestInitialize to run code before running each test 
		// [TestInitialize()]
		// public void MyTestInitialize() { }
		//
		// Use TestCleanup to run code after each test has run
		// [TestCleanup()]
		// public void MyTestCleanup() { }
		//
		#endregion

		[TestMethod]
		public void TestSuccess()
		{
			var collection = Enumerable.Range(1, Environment.ProcessorCount);


			var mappedCollection = from x in collection.AsLongParallel()
														 select SuccessfulLongRunner(x);


			foreach (var x in mappedCollection)
			{
				Console.WriteLine(x);
			}

		}

		[TestMethod]
		[ExpectedException(typeof(AggregateException))]
		public void TestException()
		{
			var collection = Enumerable.Range(1, Environment.ProcessorCount);


			var mappedCollection = from x in collection.AsLongParallel()
														 select ErraticLongRunner(x);


			foreach (var x in mappedCollection)
			{
				Console.WriteLine(x);
			}
		}

		private int SuccessfulLongRunner(int x)
		{
			System.Threading.Thread.Sleep(TimeSpan.FromSeconds(1));

			return x * x;
		}

		private int ErraticLongRunner(int x)
		{
			if (x % Environment.ProcessorCount == 1)
			{
				System.Threading.Thread.Sleep(TimeSpan.FromSeconds(1));

				throw new ApplicationException("Sabotage.");
			}
			else
			{
				System.Threading.Thread.Sleep(TimeSpan.FromSeconds(2));

				return x * x;
			}
		}
	}
}
