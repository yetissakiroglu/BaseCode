using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Economy.Core.Core
{
    public record ImageItemDto(
    string Name,
    string RelativePath,
    long SizeBytes,
    DateTimeOffset LastModifiedUtc
);
}
