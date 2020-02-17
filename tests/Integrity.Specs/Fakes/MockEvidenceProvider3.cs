using System.Collections.Generic;
using System.Threading.Tasks;
using RailwaySharp;
using Integrity;

class MockEvidenceProvider3 : EvidenceProvider
{
    readonly bool _fail;

    public MockEvidenceProvider3(bool fail, IEnumerable<EvidenceProvider> services)
        : base(services) => _fail = fail;

    public override Task<Result<Evidence, string>> VerifyAsync() =>
        Task.FromResult(_fail
            ? Result<Evidence, string>.FailWith("Evidence 3 not proven.")
            : Result<Evidence, string>.Succeed(new Evidence(GetType(), _fail)));
}