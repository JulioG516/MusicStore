using System;
using ReactiveUI;

namespace MusicStore.Models;

public static class Interactions
{
    public static readonly Interaction<Exception, ErrorRecoveryOption> Errors =
        new Interaction<Exception, ErrorRecoveryOption>();
}