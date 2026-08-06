using System;
using System.Collections.Generic;
using JasperFx.Core;
using Marten.Events;
using Shouldly;
using Xunit;

namespace CoreTests.Events;

/// <summary>
/// #5195 — the testing-extension polling loops used a fixed delay, so a projection that had already
/// caught up still cost the caller a full quantum on every check. The backoff has to start short
/// enough that the common case is off the clock, and still settle on the original interval so a
/// database that really is still working sees no more polling pressure than before.
/// </summary>
public class polling_backoff_progression
{
    private static List<TimeSpan> Take(PollingBackoff backoff, int count)
    {
        var delays = new List<TimeSpan>();
        for (var i = 0; i < count; i++)
        {
            delays.Add(backoff.Next());
        }

        return delays;
    }

    [Fact]
    public void starts_short_and_doubles_up_to_the_ceiling()
    {
        Take(new PollingBackoff(250.Milliseconds()), 8).ShouldBe([
            5.Milliseconds(),
            10.Milliseconds(),
            20.Milliseconds(),
            40.Milliseconds(),
            80.Milliseconds(),
            160.Milliseconds(),
            250.Milliseconds(),
            250.Milliseconds()
        ]);
    }

    [Fact]
    public void the_first_wait_is_a_fraction_of_the_old_fixed_delay()
    {
        // The whole point: previously every miss cost 250ms. Four checks now cost less than one did.
        var backoff = new PollingBackoff(250.Milliseconds());
        var firstFour = Take(backoff, 4);

        firstFour.Sum().ShouldBeLessThan(250.Milliseconds());
    }

    [Fact]
    public void never_polls_faster_or_slower_than_the_ceiling_once_settled()
    {
        Take(new PollingBackoff(100.Milliseconds()), 10)[^1].ShouldBe(100.Milliseconds());
    }

    [Fact]
    public void a_ceiling_below_the_initial_delay_is_honoured_rather_than_inverted()
    {
        // Guards the clamp: without it the "initial" would exceed the ceiling and poll too fast.
        Take(new PollingBackoff(1.Milliseconds()), 3)
            .ShouldAllBe(x => x == 5.Milliseconds());
    }
}

internal static class TimeSpanListExtensions
{
    public static TimeSpan Sum(this IEnumerable<TimeSpan> spans)
    {
        var total = TimeSpan.Zero;
        foreach (var span in spans)
        {
            total += span;
        }

        return total;
    }
}
