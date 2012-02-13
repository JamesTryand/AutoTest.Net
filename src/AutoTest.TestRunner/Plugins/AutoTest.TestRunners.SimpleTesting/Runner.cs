using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoTest.TestRunners.Shared;
using AutoTest.TestRunners.Shared.Logging;
using AutoTest.TestRunners.Shared.Communication;
using AutoTest.TestRunners.Shared.AssemblyAnalysis;

namespace AutoTest.TestRunners.SimpleTesting
{
	public class Runner : IAutoTestNetTestRunner
	{


		private ILogger _internalLogger;
		private ITestFeedbackProvider _channel = null;
		private Func<string, IReflectionProvider> _reflectionProviderFactory = (assembly) => { return Reflect.On(assembly); };
		private bool _isInitialized = false;
		private static bool _runtimeInitialized = false;

		public Runner()
		{

		}

		#region IAutoTestNetTestRunner Members

		public string Identifier
		{
			get { return "SimpleTesting"; }
		}

		public void SetLogger(Shared.Logging.ILogger logger)
		{
			_internalLogger = logger;
		}

		public void SetReflectionProvider(Func<string, Shared.AssemblyAnalysis.IReflectionProvider> reflectionProviderFactory)
		{
			_reflectionProviderFactory = reflectionProviderFactory;
		}

		public void SetLiveFeedbackChannel(Shared.Communication.ITestFeedbackProvider channel)
		{
			_channel = channel;
		}

		public bool IsTest(string assembly, string member)
		{
			using (var locator = _reflectionProviderFactory(assembly))
			{
				var method = locator.LocateMethod(member);
				if (method == null)
					return false;
				return method.Attributes.Contains("Xunit.FactAttribute") &&
					!method.IsAbstract;
			}
		}

		public bool ContainsTestsFor(string assembly, string member)
		{
			using (var locator = _reflectionProviderFactory(assembly))
			{
				var cls = locator.LocateClass(member);
				if (cls == null)
					return false;
				return !cls.IsAbstract && cls.Attributes.Contains("Microsoft.VisualStudio.TestTools.UnitTesting.TestClassAttribute");
			}
		}

		public bool ContainsTestsFor(string assembly)
		{
			using (var parser = _reflectionProviderFactory(assembly))
			{
				return parser.GetReferences().Count(x => x.Name.Equals("Microsoft.VisualStudio.QualityTools.UnitTestFramework")) > 0;
			}
		}

		public bool Handles(string identifier)
		{
			return Identifier.ToLower().Equals(identifier.ToLower());
		}

		public IEnumerable<Shared.Results.TestResult> Run(Shared.Options.RunSettings settings)
		{
			return new SimpleTestingRunner(_internalLogger, _reflectionProviderFactory, _channel).Run(settings);
		}

		#endregion
	}
}
