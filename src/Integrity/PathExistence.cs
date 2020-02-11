using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using System.IO.Abstractions;
using CSharpx;
using RailwaySharp;

namespace Integrity
{
    public sealed class Paths
    {
        string _basePath = string.Empty;
        bool _basePathSet = false;
        public List<(bool, string)> Value { get; private set; }

        public Paths()
        {
            Value = new List<(bool, string)>();
        }

        public Paths SetBasePath(string value)
        {
            if (_basePathSet) throw new InvalidOperationException("Cannot set base path more than once.");
            Guard.AgainstNull(nameof(value), value);

            _basePath = value;
            _basePathSet = true;
            return this;
        }

        public Paths AddFile(string path)
        {
            Value.Add((true, Path.Combine(_basePath, path)));
            return this;
        }

        public Paths AddDirectory(string path)
        {
            Value.Add((false, Path.Combine(_basePath, path)));
            return this;
        }
    }

    public sealed class PathExistence : EvidenceProvider
    {
        readonly IFileSystem _fileSystem;
        readonly IEnumerable<(bool, string)> _paths;
    
        public PathExistence(IFileSystem fileSystem, IEnumerable<EvidenceProvider> dependencies,
            IEnumerable<(bool, string)> paths) : base(dependencies)
        {
            Guard.AgainstNull(nameof(paths), paths);

            _fileSystem = fileSystem;
            _paths = paths;
        } 

        public PathExistence(IEnumerable<EvidenceProvider> dependencies, IEnumerable<(bool, string)> paths)
            : this(new FileSystem(), dependencies, paths) { }

        public PathExistence(IFileSystem fileSystem, IEnumerable<(bool, string)> paths)
            : this(fileSystem, Enumerable.Empty<EvidenceProvider>(), paths) { }

        public PathExistence(IEnumerable<(bool, string)> paths)
            : this(new FileSystem(), Enumerable.Empty<EvidenceProvider>(), paths) { }

        public override Task<Result<Evidence, string>> VerifyAsync()
        {
            foreach (var item in _paths)
            {
                (var isFile, var path) = item;
                switch (isFile) {
                    default:
                        if (!_fileSystem.File.Exists(path)) {
                            return Task.FromResult(
                                Result<Evidence, string>.FailWith($"{path} file is not found."));
                        }
                        continue;
                    case false:
                        if (!_fileSystem.Directory.Exists(path)) {
                            return Task.FromResult(
                                Result<Evidence, string>.FailWith($"{path} directory is not found."));
                        }
                        continue;
                }
            }
            return Task.FromResult(
                Result<Evidence, string>.Succeed(new Evidence(GetType(), _paths)));
        }
    }
}