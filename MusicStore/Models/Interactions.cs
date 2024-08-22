using System;
using System.Reactive;
using ReactiveUI;

namespace MusicStore.Models;

public static class Interactions
{
    public static readonly Interaction<Exception, ErrorRecoveryOption> Errors =
        new Interaction<Exception, ErrorRecoveryOption>();

    public static readonly Interaction<Unit, string?> GetFolder =
        new Interaction<Unit, string?>();
}