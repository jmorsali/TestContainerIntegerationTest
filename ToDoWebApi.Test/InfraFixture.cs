using System;
using Xunit;

namespace ToDoWebApi.Test
{
    [CollectionDefinition(InfraFixture.INFRA_FIXTURE_KEY)]
    public class InfraFixtureDefinition : ICollectionFixture<InfraFixture>
    {
    }


    public class InfraFixture : IDisposable
    {
        public const string INFRA_FIXTURE_KEY = "INFRA_FIXTURE";

        public Infra InfraInstance { get; }

        public InfraFixture()
        {
            InfraInstance = new Infra();
        }

        public void Dispose()
        {
            InfraInstance.Dispose();
        }
    }
}