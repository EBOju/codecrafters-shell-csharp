using System.Collections.Generic;

namespace Shell;

public interface IBuiltInRegistry
{
    List<IBuiltInCommand> BuiltIns { get; }
}