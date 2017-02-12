using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.Azure.EventHubs.Processor.UnitTests
{
    public class ThrowingEventProcessor : IEventProcessor
    {
        private readonly Action<string> _logger;

        public ThrowingEventProcessor(Action<string> logger)
        {
            _logger = logger;
        }

        public Task OpenAsync(PartitionContext context)
        {
            return Task.CompletedTask;
        }

        public Task CloseAsync(PartitionContext context, CloseReason reason)
        {
            return Task.CompletedTask;
        }

        public Task ProcessEventsAsync(PartitionContext context, IEnumerable<EventData> messages)
        {
            _logger(nameof(ProcessEventsAsync));
            throw new Exception("failed at processing");
        }

        public Task ProcessErrorAsync(PartitionContext context, Exception error)
        {
            _logger(nameof(ProcessErrorAsync));
            throw new Exception("failed at failing");
        }
    }

    public class DummyEventProcessor : IEventProcessor
    {
        public Task OpenAsync(PartitionContext context)
        {
            return Task.CompletedTask;
        }

        public Task CloseAsync(PartitionContext context, CloseReason reason)
        {
            return Task.CompletedTask;
        }

        public Task ProcessEventsAsync(PartitionContext context, IEnumerable<EventData> messages)
        {
            return Task.CompletedTask;
        }

        public Task ProcessErrorAsync(PartitionContext context, Exception error)
        {
            return Task.CompletedTask;
        }
    }

    public class ThrowingEventProcessorFactory : IEventProcessorFactory
    {
        private readonly Action<string> _logger;
        private ThrowingEventProcessor _throwingEventProcessor;

        public ThrowingEventProcessorFactory(Action<string> logger)
        {
            _logger = logger;
        }

        public IEventProcessor CreateEventProcessor(PartitionContext context)
        {
            if (_throwingEventProcessor == null)
            {
                _throwingEventProcessor = new ThrowingEventProcessor(_logger);
                return _throwingEventProcessor;
            }
            return new DummyEventProcessor();
        }
    }
}