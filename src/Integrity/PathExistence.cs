using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO.Abstractions;
using RailwaySharp;

namespace Integrity
{
    internal enum PathItemType
    {
        File,
        Directory
    }

    public sealed class PathItem
    {
        internal PathItem(PathItemType tag, string value)
        {
            Tag = tag;
            Value = value;
        }

        internal PathItemType Tag { get; private set; }

        internal string Value { get; set; }

        public static IEnumerable<PathItem> Files(params string[] paths)
        {
            Guard.AgainstArraySize(nameof(paths), 1, paths);

            return _(); IEnumerable<PathItem> _()
            {
                foreach (var path in paths) yield return File(path);
            }
        }

        public static PathItem File(string path)
        {
            Guard.AgainstNull(nameof(path), path);
            Guard.AgainstEmptyWhiteSpace(nameof(path), path);

            return new PathItem(PathItemType.File, path);
        }

        public static IEnumerable<PathItem> Directories(params string[] paths)
        {
            Guard.AgainstArraySize(nameof(paths), 1, paths);

            return _(); IEnumerable<PathItem> _()
            {
                foreach (var path in paths) yield return Directory(path)
;            }
        }

        public static PathItem Directory(string path)
        {
            Guard.AgainstNull(nameof(path), path);
            Guard.AgainstEmptyWhiteSpace(nameof(path), path);

            return new PathItem(PathItemType.Directory, path);
        }
    }

    public sealed class PathExistence : EvidenceProvider
    {
        readonly IFileSystem _fileSystem;
        readonly IEnumerable<PathItem> _paths;
    
        public PathExistence(IFileSystem fileSystem, IEnumerable<EvidenceProvider> dependencies, IEnumerable<PathItem> paths) : base(dependencies)
        {
            Guard.AgainstNull(nameof(paths), paths);

            _fileSystem = fileSystem;
            _paths = paths;
        } 

        public PathExistence(IEnumerable<EvidenceProvider> dependencies, IEnumerable<PathItem> paths)
            : this(new FileSystem(), dependencies, paths) { }

        public PathExistence(IFileSystem fileSystem, IEnumerable<PathItem> paths)
            : this(fileSystem, Enumerable.Empty<EvidenceProvider>(), paths) { }

        public PathExistence(IEnumerable<PathItem> paths)
            : this(new FileSystem(), Enumerable.Empty<EvidenceProvider>(), paths) { }

        public override Task<Result<Evidence, string>> VerifyAsync()
        {
            foreach (var path in _paths)
            {
                switch (path.Tag) {
                    default:
                        if (!_fileSystem.File.Exists(path.Value)) {
                            return Task.FromResult(
                                Result<Evidence, string>.FailWith($"{path.Value} file is not found."));
                        }
                        continue;
                    case PathItemType.Directory:
                        if (!_fileSystem.Directory.Exists(path.Value)) {
                            return Task.FromResult(
                                Result<Evidence, string>.FailWith($"{path.Value} directory is not found."));
                        }
                        continue;
                }
            }
            return Task.FromResult(
                Result<Evidence, string>.Succeed(new Evidence(GetType(), _paths)));
        }
    }
}