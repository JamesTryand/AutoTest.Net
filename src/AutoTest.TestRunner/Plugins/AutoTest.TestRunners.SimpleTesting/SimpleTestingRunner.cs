using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutoTest.TestRunners.SimpleTesting
{
	class SimpleTestingRunner
	{
		private Shared.Logging.ILogger _internalLogger;
		private Func<string, Shared.AssemblyAnalysis.IReflectionProvider> _reflectionProviderFactory;
		private Shared.Communication.ITestFeedbackProvider _channel;

		public SimpleTestingRunner(Shared.Logging.ILogger _internalLogger, Func<string, Shared.AssemblyAnalysis.IReflectionProvider> _reflectionProviderFactory, Shared.Communication.ITestFeedbackProvider _channel)
		{
			this._internalLogger = _internalLogger;
			this._reflectionProviderFactory = _reflectionProviderFactory;
			this._channel = _channel;
		}

		internal IEnumerable<Shared.Results.TestResult> Run(Shared.Options.RunSettings settings)
		{
			throw new NotImplementedException();
		}
	}
}
